using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu:MonoBehaviour
{
    menuitem[] _menuItems;
    int _currentMaxMenuItems;
    int _currentMenuItem=0;
    public RectTransform _menuCursor; //set to the menu cursor child

    //for cursor calculations
    float c_menuItemHeight=30f;
    float c_menuItemOffset=13f;

    public bool keyFocus=false;

    void Start()
    {
        _menuItems=transform.GetComponentsInChildren<menuitem>();

        setActionMenu(new string[]{"a","b"},new System.Action[2]{
            ()=>{
                Debug.Log("hey");
            },
            ()=>{
                Debug.Log("hey2");
            }
        });
    }

    void Update()
    {
        keyControl();
    }

    void keyControl()
    {
        if (!keyFocus)
        {
            return;
        }

        if (Input.GetButtonDown("confirm"))
        {
            _menuItems[_currentMenuItem].itemAction();
        }

        else if (Input.GetButtonDown("down"))
        {
            moveMenuCursor(1);
        }

        else if (Input.GetButtonDown("up"))
        {
            moveMenuCursor(-1);
        }
    }

    void setActionMenu(string[] menuTexts,System.Action[] menuActions)
    {
        _currentMaxMenuItems=menuTexts.Length;

        for (int x=0;x<_menuItems.Length;x++)
        {
            if (x>=menuTexts.Length)
            {
                _menuItems[x].setActive(false);
            }

            else
            {
                _menuItems[x].setActive(true);
                _menuItems[x].setItem(menuTexts[x],menuActions[x]);
            }
        }
    }

    void moveMenuCursor(int changePos)
    {
        _currentMenuItem+=changePos;

        if (_currentMenuItem>=_currentMaxMenuItems)
        {
            _currentMenuItem=0;
        }

        else if (_currentMenuItem<0)
        {
            _currentMenuItem=_currentMaxMenuItems-1;
        }

        Vector2 cursorpos=_menuCursor.anchoredPosition;
        cursorpos.y=(-c_menuItemHeight*_currentMenuItem)-c_menuItemOffset;
        _menuCursor.anchoredPosition=cursorpos;
    }
}
