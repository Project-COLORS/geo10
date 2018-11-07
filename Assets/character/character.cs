using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character:MonoBehaviour
{
    public GameObject _cam;

    void Start()
    {

    }

    void Update()
    {
        transform.forward=-_cam.transform.forward;
    }
}
