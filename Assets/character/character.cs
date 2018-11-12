using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character:MonoBehaviour
{
    public characterStat stats; //set to own stats which should be a child
    public globalscontrol _globals;

    [System.NonSerialized]
    public tile currentTile;

    protected string[] c_charMenuStrings;

    protected System.Action[] c_charMenuActions;

    void Start()
    {
        c_charMenuStrings=new string[]{"MOVE"};
        c_charMenuActions=new System.Action[]{move};
    }

    void Update()
    {
        transform.forward=-_globals.cam.transform.forward;
    }

    public void move()
    {
        _globals.grid.moveCalc(transform.position,stats.moveSpaces);
        _globals.cursor.commandQueue(move2,_globals.cursor.defaultCancel);
    }

    void move2(tile tile)
    {
        _globals.grid.clearSelectedTiles();
        _globals.grid.relocateChar(this,tile);
    }

    public void openCharMenu()
    {
        _globals.inputcontrol.setFocus("menu");
        _globals.menu.setActionMenu(c_charMenuStrings,c_charMenuActions);
    }
}
