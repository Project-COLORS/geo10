using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile:MonoBehaviour
{
    [Tooltip("possible tile sprites")]
    public Sprite[] _sprites; //user set to possible sprites
    public SpriteRenderer _spriteRenderer; //set to self
}
