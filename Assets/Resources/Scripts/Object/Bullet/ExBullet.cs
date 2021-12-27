using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExBullet : Bullet
{
    private void Awake()
    {
        direction = 1f;
        speed = 14f;
        damage = 3;
    }

    private void OnEnable()
    {
        SetBoundry();
        hp = maxHp;
        bHitable = true;
    }
    private void OnDisable()
    {
        Vector3 scale = transform.localScale;
        if(scale.x<0f)
        {
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime * Vector3.right;
        if (!boundary.Contains(transform.position))
            ObjectManager.Instance.RecallObject(gameObject);
    }




    // ** self-defined
    public override void Hit()
    {
        --hp;
        bHitable = false;
        if(hp<1)
        {
            ObjectManager.Instance.NewObject(eObjectKey.EX_BULLET_HIT, transform.position);
            ObjectManager.Instance.RecallObject(gameObject);
        }
        else
            StartCoroutine(CoroutineFunc.DelayOnce(() => { bHitable = true; }, hitDelay));
    }

    // ** Getter & Setter
    public override eObjectKey ObjectKey => eObjectKey.EX_BULLET;



    // ** Field
    int maxHp = 3;
    int hp;
    bool bHitable;
    float hitDelay = 0.1f;
}
