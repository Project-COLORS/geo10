using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor:MonoBehaviour
{
    /*-- externals --*/
    public GameObject _cam; //set to maincam
    public Rigidbody _body; //set to SELF
    public Transform _cursorSprite; //set to the cursor child sprite

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

    //camera end

    tile _previousTile;

    void Update()
    {
        //set cursor always point to camera
        _cursorSprite.forward=-_cam.transform.forward;

        inputProcessing();
        positionUpdate();
    }

    void inputProcessing()
    {
        _moveVec.x=Input.GetAxisRaw("Vertical");
        _moveVec.z=Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("rotateleft"))
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

        _cam.transform.eulerAngles=_camAngle;
        _cam.transform.position=Vector3.Lerp(_cam.transform.position,_posvec,.2f);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (_previousTile)
        {
            _previousTile.setColour(Color.clear);
        }

        _previousTile=collider.gameObject.GetComponent<tile>();
        _previousTile.setColour(Color.cyan);
    }
}
