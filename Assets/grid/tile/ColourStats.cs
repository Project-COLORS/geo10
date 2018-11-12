using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourStats:MonoBehaviour
{
    public SpriteRenderer _spriteRenderer; //set to self

    public float[] colourHp; //r,g,b
    public float maxDamage; //max damage that can be taken

    //array of main element colours. move this out into
    //a global for each map eventually for variance
    static Color[] c_colours=new Color[]{
        new Color32(199,30,61,1), //r
        new Color32(152,155,21,1), //g
        new Color32(51,169,193,1) //b
    };

    public Color currentHpColour;

    //currently, the system is, dealing colour damage will
    //first another colour before increasing the damage of
    //the dealt damage. we could also put type calculations in
    //here eventually
    public void dealDamage(int colourType,int damage)
    {
        int currentColour=0;
        for (int x=0;x<colourHp.Length;x++)
        {
            if (colourHp[x]>0)
            {
                currentColour=x;
                break;
            }
        }

        if (colourType==currentColour)
        {
            colourHp[currentColour]+=damage;
        }

        else
        {
            colourHp[currentColour]-=damage;

            if (colourHp[currentColour]<0)
            {
                colourHp[colourType]=colourHp[currentColour]*-1;
                colourHp[currentColour]=0;
            }
        }

        if (colourHp[currentColour]>maxDamage)
        {
            colourHp[currentColour]=maxDamage;
        }

        else if (colourHp[currentColour]<0)
        {
            colourHp[currentColour]=0;
        }

        setHpColour();
    }

    //calculate the colour hp. change this function later
    //when the system is more thought out
    public void setHpColour()
    {
        for (int x=0;x<colourHp.Length;x++)
        {
            if (colourHp[x]>0)
            {
                Color setColour=new Color32(0,0,0,0);
                setColour+=c_colours[x];
                setColour.a=colourHp[x]/maxDamage;

                _spriteRenderer.color=setColour;
                currentHpColour=setColour;

                return;
            }
        }
    }
}
