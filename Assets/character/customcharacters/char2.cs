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
        _globals.grid.moveCalc(transform.position,stats.attackRange,true);
        _globals.cursor.commandQueue(attack2,_globals.cursor.defaultCancel);
    }

    void attack2(tile tile)
    {
        _globals.grid.clearSelectedTiles();
        tile.colourStats.dealDamage(stats.colourType,stats.attack*2);

        if (tile.currentCharacter)
        {
            tile.currentCharacter.GetComponent<character>().dealDamage(stats.attack);
        }
    }
}
