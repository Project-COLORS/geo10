using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileconfig:MonoBehaviour
{
    [Tooltip("Tile height in gameplay units, not actual physical height")]
    public int _tileHeight;
    public Color _colour;
    public bool _obstruction;
}
