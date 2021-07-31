using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyMaps
{
    public static readonly KeyCode TILE_SELECTOR = KeyCode.Mouse0;

    public static readonly KeyCode[] UP_MOVE_KEYS = { KeyCode.W, KeyCode.UpArrow };
    public static readonly KeyCode[] LEFT_MOVE_KEYS = { KeyCode.A, KeyCode.LeftArrow };
    public static readonly KeyCode[] DOWN_MOVE_KEYS = { KeyCode.S, KeyCode.DownArrow };
    public static readonly KeyCode[] RIGHT_MOVE_KEYS = { KeyCode.D, KeyCode.RightArrow };

    public static bool IsAnyKeyPressed(KeyCode[] validKeys)
    {
        foreach(KeyCode key in validKeys)
        {
            if(Input.GetKey(key))
                return true;
        }
        return false;
    }
}
