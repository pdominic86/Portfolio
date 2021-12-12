using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eObjectKey
{
    NORMAL_BULLET,GOOPY,PLAYER
};

public enum eGroupKey
{
    BULLET,ENEMY,BOSS, PLAYER
}

public delegate void DelayAction();


class CoroutineFunc
{
    public static IEnumerator DelayCoroutine(DelayAction _func, float time)
    {
        yield return new WaitForSeconds(time);
        _func();
    }
}