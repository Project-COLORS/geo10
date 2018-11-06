using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor:MonoBehaviour
{
    public GameObject _cam;

    void Start()
    {

    }

    void Update()
    {
        //set cursor always point to camera
        transform.forward=-_cam.transform.forward;
    }
}
