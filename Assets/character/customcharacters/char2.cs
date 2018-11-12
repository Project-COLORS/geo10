using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char2:character
{
    void Start()
    {
        c_charMenuStrings=new string[]{"MOVE","ATTACK"};
        c_charMenuActions=new System.Action[]{move,attack};
    }

    void attack()
    {

    }
}
