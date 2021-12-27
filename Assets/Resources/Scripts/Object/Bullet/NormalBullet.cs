using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{
    private void Awake()
    {
        direction = 1f;
        speed = 12f;
        damage = 2;
    }

    private void OnEnable()
    {
        SetBoundry();
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * forward;
        if (!boundary.Contains(transform.position))
            ObjectManager.Instance.RecallObject(gameObject);
    }




    // ** self-defined
    public override void Hit()
    {
        ObjectManager.Instance.RecallObject(gameObject);
        ObjectManager.Instance.NewObject(eObjectKey.NORMAL_BULLET_HIT, transform.position);
    }

    // ** Getter & Setter
    public override eObjectKey ObjectKey => eObjectKey.NORMAL_BULLET;
}
