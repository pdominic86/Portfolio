using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eObjectKey
{
    // player
    PLAYER,
    // bullet
    NORMAL_BULLET,
    // enemy
    TARGET_BORAD,
    // boss
    GOOPY,
    TOMBSTONE,
    //effect
    TARGET_BREAK,
    GOOPY_EXPLODE,
    JUMP_DUST_EFFECT,
    DASH_DUST_EFFECT,
    NORMAL_BULLET_HIT,
    NORMAL_BULLET_SHOOT,
    GOOPY_PHASE2_DUST,

    // scene transition
    SCENE_CHANGE_OPEN,
    SCENE_CHANGE_CLOSE,

    // trigger
    TO_TUTORIAL,
    TO_WORLD,

    // plarform
    PLATFORM
};

public enum eGroupKey
{
    PLAYER,
    BULLET,
    BOSS,
    ENEMY,
    EFFECT,
    TRIGGER,
    PLATFORM
}

public enum eSceneKey
{
    TITLE,
    HOUSE,
    TUTORIAL,
    WORLD,
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