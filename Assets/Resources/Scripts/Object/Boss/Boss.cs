using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Prefab
{
    // ** Getter & Setter
    public override eGroupKey GroupKey { get => eGroupKey.BOSS; }
    public virtual eObjectKey ObjectKey { get; }

    public int HP => hp;
    public int MaxHP => MaxHP;

    // ** Field
    protected int maxHp;
    protected int hp;
}
