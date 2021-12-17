using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : Prefab
{
    // ** unity Á¦°ø
    protected void Update()
    {
        Vector3 forward = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        transform.position+=speed * Time.deltaTime * forward;
        if (transform.position.x < boundaryX.x || transform.position.x > boundaryX.y)
        {
            ObjectManager.Instance.RecallObject(gameObject);
        }
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
