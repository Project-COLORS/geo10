using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputcontrol:MonoBehaviour
{
    public menu _menu;
    public cursor _cursor;

    void Start()
    {
        setFocus("cursor");
    }

    public void setFocus(string focusItem)
    {
        _menu.keyFocus=false;
        _cursor.keyFocus=false;

        switch (focusItem)
        {
            case "cursor":
            _cursor.keyFocus=true;
            break;

            case "menu":
            _menu.keyFocus=true;
            break;
        }
    }
}
