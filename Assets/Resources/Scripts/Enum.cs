using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eObjectKey
{
    // player
    PLAYER,
    PLAYER_WORLD,
    // bullet
    NORMAL_BULLET,
    // enemy
    TARGET_BORAD,
    // boss
    GOOPY,
    TOMBSTONE,
    // parry
    SPHERE,

    //effect
    PARRY_AURA,
    PARRY_HIT,
    EXPLOSION,
    EXPLOSION_SIDE,
    GOOPY_EXPLODE,
    JUMP_DUST_EFFECT,
    DASH_DUST_EFFECT,
    HIT,
    NORMAL_BULLET_HIT,
    NORMAL_BULLET_SHOOT,
    GOOPY_PHASE1_DUST,
    GOOPY_PHASE2_DUST,
    TOMBSTONE_INTRO_DUST,
    FIGHT_TEXT,

    // scene transition
    SCENE_CHANGE_OPEN,
    SCENE_CHANGE_CLOSE,

    // trigger
    TRIGGER,

    // popup
    BOTTON,

    // plarform
    PLATFORM
};

public enum eGroupKey
{
    PLAYER,
    BULLET,
    BOSS,
    ENEMY,
    PARRY,
    EFFECT,
    TRIGGER,
    POPUP,
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