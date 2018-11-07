using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile:MonoBehaviour
{
    public SpriteRenderer _spriteRenderer; //set to self
    public GameObject currentCharacter;

    public bool isObstruction;

    public void setColour(Color colour)
    {
        _spriteRenderer.color=colour;
    }

    public void markSelected()
    {
        _spriteRenderer.color=Color.cyan;
    }
}
