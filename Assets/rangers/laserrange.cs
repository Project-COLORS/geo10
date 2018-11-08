using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserrange:MonoBehaviour
{
    public globalscontrol _globals;

    void Update()
    {
        transform.LookAt(_globals.cursor.transform);
        Vector3 angles=transform.eulerAngles;
        angles.x=0;
        transform.eulerAngles=angles;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("tile"))
        {
            collider.gameObject.GetComponent<tile>().setColour(Color.blue);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("tile"))
        {
            collider.gameObject.GetComponent<tile>().setColour(Color.clear);
        }
    }
}
