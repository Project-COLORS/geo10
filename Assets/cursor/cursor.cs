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

    /*-- camera and movement related --*/
    Vector3 _moveVec=new Vector3();
    Vector3 t_moveVec=new Vector3();
    Vector3 _posvec=new Vector3();

    Vector3 _camAngle=new Vector3(30,0,0);
    readonly float c_cursorspeed=7.9f;
    readonly int [,] c_camPositions=new int[4,7]{{1,1,225,0,2,-1,1},{-1,1,135,2,0,-1,-1},{-1,-1,45,0,2,1,-1},{1,-1,-45,2,0,1,1}};

    float _targetcamYAngle=225f;
    int _currentcamPosition=0;
    float[] _camPositionsCurrent=new float[3]{0,0,0};
    /*-- camera/movement end --*/

    tile _previousTile;
    public bool keyFocus=false;

    Action<tile> _currentCommand;
    Action _cancelCommand;

    void Update()
    {
        //set cursor always point to camera
        _cursorSprite.forward=-_globals.cam.transform.forward;

        keyControl();
        positionUpdate();
    }

    void keyControl()
    {
        if (!keyFocus)
        {
            _moveVec.x=0;
            _moveVec.z=0;
            return;
        }

        _moveVec.x=Input.GetAxisRaw("Vertical");
        _moveVec.z=Input.GetAxisRaw("Horizontal");

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
            else if (_previousTile.currentCharacter!=null)
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

    void positionUpdate()
    {
        //performing cursor movement, with adjustments based on the current camera position
        _moveVec.Normalize();
        t_moveVec.x=_moveVec[c_camPositions[_currentcamPosition,3]]*c_cursorspeed*c_camPositions[_currentcamPosition,5];
        t_moveVec.z=_moveVec[c_camPositions[_currentcamPosition,4]]*c_cursorspeed*c_camPositions[_currentcamPosition,6];
        t_moveVec=Quaternion.Euler(0,-45,0)*t_moveVec;
        _body.velocity=t_moveVec;

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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("tile"))
        {
            if (_previousTile)
            {
                _previousTile.setColour(_previousTile.currentColour);
            }

            _previousTile=collider.gameObject.GetComponent<tile>();
            _previousTile.setColour(Color.blue); //temporary float over tile effect
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
