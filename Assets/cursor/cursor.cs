using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cursor:MonoBehaviour
{
    /*-- externals --*/
    public Rigidbody _body; //set to SELF
    public Transform _cursorSprite; //set to the cursor child sprite
    public globalscontrol _globals;
    public Transform _cursorTile; //set to the cursor tile object that follows the cursor

    /*-- camera and movement related --*/
    Vector3 _moveVec=new Vector3();
    Vector3 t_moveVec=new Vector3();
    Vector3 _posvec=new Vector3();

    Vector3 _camAngle=new Vector3(30,0,0);
    readonly float c_cursorspeed=7.9f;
    /*  for each camera position (4 positions):
        0: multiplier for X position shift
        1: multiplier for Z position shift
        2: Y angle of camera (currently not being used)
        3: horizontal control cursor movement axis. 0 for X, 2 for z
        4: vertical control cursor movement axis
        5: horizontal control invert. 1 for normal, -1 for inverted
        6: vertical control invert*/
    readonly int [,] c_camPositions=new int[4,7]{
        {1,1,225,0,2,-1,1},{-1,1,135,2,0,-1,-1},{-1,-1,45,0,2,1,-1},{1,-1,-45,2,0,1,1}
    };

    float _targetcamYAngle=225f;
    int _currentcamPosition=0;
    float[] _camPositionsCurrent=new float[3]{0,0,0};
    /*-- camera/movement end --*/

    tile _previousTile; //the tile the cursor is over or was last over

    [NonSerialized]
    public bool keyFocus=false; //for keycontrol system

    /*-- command queue system --*/
    Action<tile> _currentCommand;
    Action _cancelCommand;

    /*-- teleport system --*/
    bool _teleporting=false; //if the cursor is in teleporting state
    Vector3 _destination; //teleport destination
    readonly float c_teleportCutoff=.1f; //when within this range of teleport destination, stop teleport

    void Update()
    {
        //set cursor always point to camera
        _cursorSprite.forward=-_globals.cam.transform.forward;

        keyControl();
        cursorTeleport();
        positionUpdate();
    }

    void keyControl()
    {
        if (!keyFocus)
        {
            //set the movement vectors to 0 when losing focus
            _moveVec.x=0;
            _moveVec.z=0;
            return;
        }

        _moveVec.x=Input.GetAxisRaw("Vertical");
        _moveVec.z=Input.GetAxisRaw("Horizontal");

        dpadControls();

        if (Input.GetButtonDown("confirm"))
        {
            //if there is a command queued and the current tile is selected, call the
            //command on the tile and dequeue the command
            if (_currentCommand!=null && _previousTile.selected)
            {
                _currentCommand(_previousTile);
                _currentCommand=null;
                _cancelCommand=null;
            }

            //if the current tile has a character, bring up its menu
            else if (_currentCommand==null && _previousTile.currentCharacter!=null)
            {
                _previousTile.currentCharacter.GetComponent<character>().openCharMenu();
            }
        }

        else if (Input.GetButtonDown("cancel"))
        {
            if (_cancelCommand!=null)
            {
                _cancelCommand();
            }

            _currentCommand=null;
            _cancelCommand=null;
        }

        else if (Input.GetButtonDown("rotateleft"))
        {
            _targetcamYAngle-=90;
            if (_currentcamPosition>=3)
            {
                _currentcamPosition=0;
            }

            else
            {
                _currentcamPosition++;
            }
        }

        else if (Input.GetButtonDown("rotateright"))
        {
            _targetcamYAngle+=90;
            if (_currentcamPosition<=0)
            {
                _currentcamPosition=3;
            }

            else
            {
                _currentcamPosition--;
            }
        }
    }

    //sub function for keycontrol handling dpad
    void dpadControls()
    {
        if (Input.GetButtonDown("dpadup"))
        {
            setTeleport(_previousTile.coords[0],_previousTile.coords[1]-1);
        }

        else if (Input.GetButtonDown("dpaddown"))
        {
            setTeleport(_previousTile.coords[0],_previousTile.coords[1]+1);
        }

        else if (Input.GetButtonDown("dpadright"))
        {
            setTeleport(_previousTile.coords[0]+1,_previousTile.coords[1]);
        }

        else if (Input.GetButtonDown("dpadleft"))
        {
            setTeleport(_previousTile.coords[0],_previousTile.coords[1]+1);
        }
    }

    //actions taken to perform cursor and camera movements
    void positionUpdate()
    {
        //performing cursor movement, with adjustments based on the current camera position
        _moveVec.Normalize();

        if (!_teleporting)
        {
            _body.velocity=moveVecTransform(_moveVec);
        }

        else
        {
            t_moveVec.x=_moveVec.x*c_cursorspeed;
            t_moveVec.z=_moveVec.z*c_cursorspeed;
            _body.velocity=t_moveVec;
        }

        //performing camera rotations, based on the target cam angle
        _camPositionsCurrent[0]=Mathf.Lerp(_camPositionsCurrent[0],5*c_camPositions[_currentcamPosition,0],.8f);
        _camPositionsCurrent[1]=Mathf.Lerp(_camPositionsCurrent[1],5*c_camPositions[_currentcamPosition,1],.8f);
        _camPositionsCurrent[2]=Mathf.Lerp(_camPositionsCurrent[2],_targetcamYAngle,.1f);

        //performing camera position movement
        _posvec=transform.position;
        _posvec.x+=_camPositionsCurrent[0];
        _posvec.y+=2;
        _posvec.z+=_camPositionsCurrent[1];

        _camAngle.y=_camPositionsCurrent[2];

        _globals.cam.transform.eulerAngles=_camAngle;
        _globals.cam.transform.position=Vector3.Lerp(_globals.cam.transform.position,_posvec,.2f);
    }

    //do the camera transform to make a vector which represents velocity movement relative to the camera
    Vector3 moveVecTransform(Vector3 moveVec)
    {
        Vector3 newVec=moveVec;
        newVec.x=moveVec[c_camPositions[_currentcamPosition,3]]*c_cursorspeed*c_camPositions[_currentcamPosition,5];
        newVec.z=moveVec[c_camPositions[_currentcamPosition,4]]*c_cursorspeed*c_camPositions[_currentcamPosition,6];
        newVec=Quaternion.Euler(0,-45,0)*newVec;

        return newVec;
    }

    //teleport update function, called every frame to move cursor towards destination
    void cursorTeleport()
    {
        if (!_teleporting)
        {
            return;
        }

        _moveVec.x=_destination.x-transform.position.x;
        _moveVec.z=_destination.z-transform.position.z;

        if (transform.position.x<_destination.x+c_teleportCutoff && transform.position.x>_destination.x-c_teleportCutoff
            && transform.position.z<_destination.z+c_teleportCutoff && transform.position.z>_destination.z-c_teleportCutoff)
        {
            _teleporting=false;
        }
    }

    //set a grid coordinate as a destination
    void setTeleport(int xpos,int ypos)
    {
        _teleporting=true;

        _destination=_globals.grid.getTile(xpos,ypos).transform.position;
    }

    //do a teleport by incrementing x and z real world coordinates of the cursor
    //(so basically a translation)
    void incrementTeleport(float xinc,float zinc)
    {
        _teleporting=true;

        Vector3 destination=transform.position;

        // destination.x+=xinc*c_camPositions[_currentcamPosition,5];
        // destination.z+=zinc*c_camPositions[_currentcamPosition,6];
        // destination=Quaternion.Euler(0,-45,0)*destination;

        destination.x+=xinc;
        destination.z+=zinc;

        destination.x=destination[c_camPositions[_currentcamPosition,3]]*c_camPositions[_currentcamPosition,5];
        destination.z=destination[c_camPositions[_currentcamPosition,4]]*c_camPositions[_currentcamPosition,6];

        _destination=destination;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("tile"))
        {
            _cursorTile.position=collider.transform.position;
            _previousTile=collider.gameObject.GetComponent<tile>();
        }
    }

    //set the cursor to execute the given command function the next
    //time the cursor clicks on a selected tile. the command given
    //take in a tile as an arg
    public void commandQueue(Action<tile> command,Action cancelCommand=null)
    {
        _currentCommand=command;
        _cancelCommand=cancelCommand;
    }

    //default cancel function to provide to commandQueue
    public void defaultCancel()
    {
        _globals.grid.clearSelectedTiles();
    }
}
