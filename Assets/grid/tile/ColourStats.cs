using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourStats:MonoBehaviour
{
    public SpriteRenderer _spriteRenderer; //set to self

    public int[] colourHp=new int[]{0,0,0}; //r,g,b
    public int maxDamage; //max damage that can be taken

    //array of main element colours. move this out into
    //a global for each map eventually for variance
    static Color[] c_colours=new Color[3]{
        new Color(199,30,61,1), //r
        new Color(152,155,21,1), //g
        new Color(51,169,193,1) //b
    };

    //currently, the system is, dealing colour damage will
    //first another colour before increasing the damage of
    //the dealt damage. we could also put type calculations in
    //here eventually
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

    //calculate the colour hp. change this function later
    //when the system is more thought out
    void setHpColour()
    {
        for (int x=0;x<colourHp.Length;x++)
        {
            if (colourHp[x]>0)
            {
                Color setColour=new Color(0,0,0,0);
                setColour+=c_colours[x];
                setColour.a=colourHp[x]/maxDamage;

                _spriteRenderer.color=setColour;
                return;
            }
        }
    }
}
