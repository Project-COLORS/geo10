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

    public void processTileConfig(tileconfig config)
    {
        _spriteRenderer.color=config.colour;
        isObstruction=config.obstruction;
        tileHeight=config.tileHeight;

        Vector3 pos=transform.position;
        pos.y+=tileHeight;
        transform.position=pos;
    }

    public void setColour(Color colour)
    {
        _spriteRenderer.color=colour;
    }

    public void markSelected()
    {
        _spriteRenderer.color=Color.cyan;
    }
}
