﻿using System.Collections;
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
    readonly int [,] c_camPositions=new int[4,7]{{1,1,225,0,2,-1,1},{-1,1,135,2,0,-1,-1},{-1,-1,45,0,2,1,-1},{1,-1,-45,2,0,1,1}};

    float _targetcamYAngle=225f;
    int _currentcamPosition=0;
    float[] _camPositionsCurrent=new float[3]{0,0,0};
    /*-- camera/movement end --*/

    tile _previousTile; //the tile the cursor is over or was last over

    [NonSerialized]
    public bool keyFocus=false; //for keycontrol system

    //command queue system
    Action<tile> _currentCommand;
    Action _cancelCommand;

    //teleport system
    bool _teleporting=false;
    Vector3 _destination;
    readonly float c_teleportCutoff=.3f;

    void Start()
    {
        setTeleport(0,0);
    }

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

    //actions taken to perform cursor and camera movements
    void positionUpdate()
    {
        //performing cursor movement, with adjustments based on the current camera position
        _moveVec.Normalize();

        if (!_teleporting)
        {
            t_moveVec.x=_moveVec[c_camPositions[_currentcamPosition,3]]*c_cursorspeed*c_camPositions[_currentcamPosition,5];
            t_moveVec.z=_moveVec[c_camPositions[_currentcamPosition,4]]*c_cursorspeed*c_camPositions[_currentcamPosition,6];
            t_moveVec=Quaternion.Euler(0,-45,0)*t_moveVec;
            _body.velocity=t_moveVec;
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

    void setTeleport(int xpos,int ypos)
    {
        _teleporting=true;
        _destination=_globals.grid.getTile(xpos,ypos).transform.position;
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
