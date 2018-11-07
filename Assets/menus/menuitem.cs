using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuitem:MonoBehaviour
{
    public Text thetext;
    public System.Action itemAction;

    public void setItem(string itemText,System.Action newItemAction=null)
    {
        thetext.text=itemText;
        itemAction=newItemAction;
    }

    public void setActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
