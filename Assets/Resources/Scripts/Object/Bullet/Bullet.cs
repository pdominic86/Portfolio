using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : Prefab
{
    protected void Awake()
    {
        base.Awake();
    }

    protected void OnDisable()
    {
        base.OnDisable();
    }


    protected void SetBoundry()
    {
        boundary = SceneManager.Instance.CurrentScene.Boundary;
        boundary.xMin += -1f;
        boundary.xMax += 1f;
        boundary.yMin += -1f;
        boundary.yMax += 1f;
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
