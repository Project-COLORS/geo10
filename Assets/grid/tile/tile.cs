using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile:MonoBehaviour
{
    public SpriteRenderer _spriteRenderer; //set to self
    public ColourStats colourStats; //set to set, tile colour stats

    public GameObject currentCharacter; //what character is in this tile

    //config settings
    public float tileHeight;
    public bool isObstruction;
    public bool cannotBeSelected;

    public bool selected=false; //if this tile has been marked as selected

    //process a tile config
    public void processTileConfig(tileconfig config)
    {
        isObstruction=config.obstruction;
        tileHeight=config.tileHeight;
        cannotBeSelected=config.cannotBeSelected;

        colourStats.colourHp=config.colourHp;
        colourStats.maxDamage=config.maxDamage;
        colourStats.setHpColour();

        Vector3 pos=transform.position;
        pos.y+=tileHeight;
        transform.position=pos;
    }

    //set the colour of this tile
    public void setColour(Color colour)
    {
        _spriteRenderer.color=colour;
    }

    //mark this tile as selected and change the colour
    public void markSelected()
    {
        _spriteRenderer.color=Color.cyan;
        selected=true;
    }

    //unmark this tile
    public void unselect()
    {
        _spriteRenderer.color=colourStats.currentHpColour;
        selected=false;
    }
}
