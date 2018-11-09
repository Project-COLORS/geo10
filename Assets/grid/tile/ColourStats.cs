using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourStats:MonoBehaviour
{
    public int[] colourHp=new int[]{0,0,0}; //r,g,b
    public int maxDamage; //max damage that can be taken

    public void dealDamage(int colourType,int damage)
    {
        colourHp[colourType]+=damage;

        if (colourHp[colourType]<0)
        {
            colourHp[colourType]=0;
        }

        else if (colourHp[colourType]>maxDamage)
        {
            colourHp[colourType]=maxDamage;
        }
    }
}
