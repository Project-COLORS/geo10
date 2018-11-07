using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character:MonoBehaviour
{
    public GameObject _cam; //set to the main camera
    public characterStat _stats; //set to own stats which should be a child
    public grid _grid; //set to the main grid object

    void Start()
    {
        move();
    }

    void Update()
    {
        transform.forward=-_cam.transform.forward;
    }

    public void move()
    {
        _grid.moveCalc(transform.position,_stats.moveSpaces);
    }
}
