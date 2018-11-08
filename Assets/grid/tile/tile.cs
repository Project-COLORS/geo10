using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile:MonoBehaviour
{
    public SpriteRenderer _spriteRenderer; //set to self
    public GameObject currentCharacter;

    //config settings
    public float tileHeight;
    public bool isObstruction;
    public Color currentColour;
    public Color prevColour;

    public bool selected=false;

    //process a tile config
    public void processTileConfig(tileconfig config)
    {
        _spriteRenderer.color=config.colour;
        currentColour=config.colour;
        prevColour=config.colour;
        isObstruction=config.obstruction;
        tileHeight=config.tileHeight;

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
        currentColour=Color.cyan;
        selected=true;
    }

    //unmark this tile
    public void unselect()
    {
        _spriteRenderer.color=prevColour;
        currentColour=prevColour;
        selected=false;
    }
}
