using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBorad : Enemy
{
    private void Awake()
    {
        MaxHp = 10;
    }

    private void OnEnable()
    {
        hp = MaxHp;
    }

    private void LateUpdate()
    {
        if(hp<=0)
        {
            ObjectManager.Instance.RecallObject(gameObject);
            ObjectManager.Instance.NewObject(eObjectKey.EXPLOSION, transform.position);
            ObjectManager.Instance.NewObject(eObjectKey.EXPLOSION_SIDE, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Prefab target = collision.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;

        eGroupKey targetKey = target.GroupKey;
        if(targetKey==eGroupKey.BULLET)
        {
            Bullet bullet = target as Bullet;
            hp -= bullet.Damage;
        }
    }
    public override eObjectKey ObjectKey => eObjectKey.TARGET_BORAD;
}
