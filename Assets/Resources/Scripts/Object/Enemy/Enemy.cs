using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Prefab
{
    public override eGroupKey GroupKey => eGroupKey.ENEMY;
    public override eObjectKey ObjectKey { get; }


    protected int MaxHp;
    protected int hp;
}
