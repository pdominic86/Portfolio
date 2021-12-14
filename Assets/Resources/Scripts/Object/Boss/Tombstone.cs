using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : Boss
{
    private void Awake()
    {
        // Rigidbody 설정
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = gravityScale;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Collider 설정
        collider = gameObject.AddComponent<CapsuleCollider2D>();
        collider.offset = new Vector2(0f, 0.86f);
        collider.size = new Vector2(1.2f, 1.58f);
        collider.isTrigger = true;

        // Animator 설정
        animator = GetComponent<Animator>();

        //체력
        maxHp = 200;
    }

    private void Start()
    {
        Initialize();   
    }

    private void OnEnable()
    {
        Initialize();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Prefab target = collision.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;

        if (target.GroupKey == eGroupKey.BULLET)
        {
            Bullet bullet = target as Bullet;
            hp -= bullet.Damage;
            ObjectManager.Instance.RecallObject(collision.gameObject);
        }
        else if (target.GroupKey==eGroupKey.BOSS)
        {
            Boss boss = target as Boss;
            hp = boss.HP;

            ObjectManager.Instance.RecallObject(collision.gameObject);
        }
    }

    // ** self_definede
    private void Initialize()
    {
        hp = maxHp;
    }

    // ** Getter & Setter
    public override eObjectKey ObjectKey => eObjectKey.TOMBSTONE;


    Rigidbody2D rigidbody;
    CapsuleCollider2D collider;
    Animator animator;
    float speed;
    float gravityScale = 5f;
}
