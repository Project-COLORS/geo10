using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileconfig:MonoBehaviour
{
    [Tooltip("Tile height in gameplay units, not actual physical height")]
    public float tileHeight;
    public bool obstruction;
    public bool cannotBeSelected;
    public float[] colourHp=new float[]{0,0,0};
    public float maxDamage;
}
