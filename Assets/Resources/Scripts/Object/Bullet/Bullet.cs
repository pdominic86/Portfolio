using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : Prefab
{
    protected void Awake()
    {
        base.Awake();
    }

    protected void OnEnable()
    {
        base.OnEnable();
    }

    protected void OnDisable()
    {
        base.OnDisable();
    }

    // ** Getter & Setter
    public override eGroupKey GroupKey => eGroupKey.BULLET; 
    public override eObjectKey ObjectKey { get; }

    public int Damage { get => damage; }


    // Field
    protected CapsuleCollider2D collider;
    protected float speed;
    protected int damage;
}
