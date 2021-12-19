using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eObjectKey
{
    // player
    PLAYER,
    // bullet
    NORMAL_BULLET,
    // boss
    GOOPY,
    TOMBSTONE,
    //effect
    GOOPY_EXPLODE,
    JUMP_DUST_EFFECT,
    DASH_DUST_EFFECT,
    NORMAL_BULLET_HIT,
    NORMAL_BULLET_SHOOT
};

public enum eGroupKey
{
    PLAYER,
    BULLET,
    BOSS,
    ENEMY,
    EFFECT
}

public enum eSceneKey
{
    OZZE
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