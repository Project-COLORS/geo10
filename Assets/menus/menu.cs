using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu:MonoBehaviour
{
    menuitem[] _menuItems;

    void Start()
    {
        _menuItems=transform.GetComponentsInChildren<menuitem>();

        setActionMenu(new string[]{"a"},new System.Action[2]);
    }

    void setActionMenu(string[] menuTexts,System.Action[] menuActions)
    {
        for (int x=0;x<_menuItems.Length;x++)
        {
            if (x>=menuTexts.Length)
            {
                _menuItems[x].setActive(false);
            }

            else
            {
                _menuItems[x].setActive(true);
                _menuItems[x].setItem(menuTexts[x]);
            }
        }
    }
}
