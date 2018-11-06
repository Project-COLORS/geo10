using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile:MonoBehaviour
{
    public SpriteRenderer _spriteRenderer; //set to self

    public void setColour(Color colour)
    {
        _spriteRenderer.color=colour;
    }
}
