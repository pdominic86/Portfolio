using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys
{
    public static KeyCode KEY_UP = KeyCode.UpArrow;
    public static KeyCode KEY_DOWN = KeyCode.DownArrow;
    public static KeyCode KEY_RIGHT = KeyCode.RightArrow;
    public static KeyCode KEY_LEFT = KeyCode.LeftArrow;
    public static KeyCode KEY_JUMP = KeyCode.Z;
    public static KeyCode KEY_SHOOT = KeyCode.X;
    public static KeyCode KEY_DASH = KeyCode.LeftShift;
    public static KeyCode KEY_LOCK = KeyCode.A;
    public static KeyCode KEY_EXMOVE = KeyCode.S;

    public static ulong right = 0x00000001;
    public static ulong left = 0x00000010;
    public static ulong up = 0x00000100;
    public static ulong down = 0x00001000;
    public static ulong jump = 0x00010000;
    public static ulong shoot = 0x00100000;
    public static ulong dash = 0x01000000;
    public static ulong locked = 0x10000000;
    public static ulong exmove = 0x100000000;

    public static ulong InputCheck()
    {
        ulong input = 0;
        if (Input.GetKey(KEY_UP))
            input |= up;
        if (Input.GetKey(KEY_DOWN))
            input |= down;
        if (Input.GetKey(KEY_RIGHT))
            input |= right;
        if (Input.GetKey(KEY_LEFT))
            input |= left;
        if (Input.GetKey(KEY_JUMP))
            input |= jump;
        if (Input.GetKey(KEY_SHOOT))
            input |= shoot;
        if (Input.GetKey(KEY_DASH))
            input |= dash;
        if (Input.GetKey(KEY_LOCK))
            input |= locked;
        if (Input.GetKey(KEY_EXMOVE))
            input |= exmove;

        return input;
    }
}
