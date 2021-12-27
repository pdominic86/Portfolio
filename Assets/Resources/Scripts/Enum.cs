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
    EX_BULLET,
    // enemy
    TARGET_BORAD,
    // boss
    GOOPY,
    TOMBSTONE,
    // parry
    PARRY_SPHERE,

    //effect
    PARRY_AURA,
    PARRY_HIT,
    JUMP_DUST,
    DASH_DUST,
    HIT,
    DEAD_DUST,
    EXPLOSION,
    EXPLOSION_SIDE,
    GOOPY_EXPLODE,
    GOOPY_PHASE1_DUST,
    GOOPY_PHASE2_DUST,
    TOMBSTONE_INTRO_DUST,
    TOMBSTONE_MOVE_DUST,
    TOMBSTONE_ATTACK_DUST,
    NORMAL_BULLET_HIT,
    NORMAL_BULLET_SHOOT,
    EXMOVE_DUST,
    EX_BULLET_HIT,

    FIGHT_TEXT,
    DEAD_TEXT,
    KNOCKDOWN_TEXT,

    // scene transition
    SCENE_CHANGE_OPEN,
    SCENE_CHANGE_CLOSE,
    STAGE_CARD_OZZE,
    STAGE_CARD_HOUSE,
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

public enum eSoundKey
{
    // player
    PLAYER_INTRO,
    PLAYER_JUMP,
    PLAYER_SHOOT,
    PLAYER_EXMOVE,

    // goopy
    GOOPY_PHASE1_INTRO,
    GOOPY_PHASE1_JUMP,
    GOOPY_PHASE1_ATTACK,
    GOOPY_PHASE2_INTRO,
    GOOPY_PHASE2_JUMP,
    GOOPY_PHASE2_ATTACK,
    GOOPY_DEATH,

    // tombstone
    TOMBSTONE_DROP,
    TOMBSTONE_MOVE,
    TOMBSTONE_ATTACK,
    TOMBSTONE_DEATH

}

public delegate void DelayAction();

class CoroutineFunc
{
    public static IEnumerator DelayOnce(DelayAction _func, float time)
    {
        yield return new WaitForSeconds(time);
        _func();
    }
}