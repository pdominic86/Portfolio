using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorld : Prefab
{
    private void Awake()
    {
        base.Awake();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (bLoad)
            Initialize();
    }

    private void OnDisable()
    {
        base.OnDisable();
    }

    private void Update()
    {
        if (!bCanInput)
            return;

        ulong input = Keys.InputCheck();
        bool bMove = false;
        Vector3 moveFactor = Vector3.zero;

        if ((input & Keys.left) != 0)
        {
            direction = -1f;
            directionFactor = 0.5f;
            bMove = true;
            moveFactor.x = -1f;
        }
        if ((input & Keys.right) != 0)
        {
            direction = 1f;
            directionFactor = 0.5f;
            bMove = true;
            moveFactor.x = 1f;
        }
        if((input & Keys.down) != 0)
        {
            if ((input & Keys.left) != 0 || (input & Keys.right) != 0)
                directionFactor -= 0.25f;
            else
                directionFactor = 0f;
            bMove = true;
            direction = 1f;
            moveFactor.y = -1f;
        }
        if ((input & Keys.up) != 0)
        {
            if ((input & Keys.left) != 0 || (input & Keys.right) != 0)
                directionFactor += 0.25f;
            else
                directionFactor = 1f;
            bMove = true;
            direction = 1f;
            moveFactor.y = 1f;
        }

        Vector3 scale = transform.localScale;
        if(scale.x*direction<0f)
        {
            scale.x *= -1f;
            transform.localScale = scale;
        }
        animator.SetFloat("direction", directionFactor);
        animator.SetBool("move", bMove);

        if (bMove)
            transform.position += speed * Time.deltaTime * moveFactor;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Prefab target = collision.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;

        eGroupKey targetKey = target.GroupKey;
        if(targetKey==eGroupKey.TRIGGER)
        {
            if(bCanInput && Input.GetKey(Keys.KEY_SHOOT))
            {
                Trigger trigger = target as Trigger;
                trigger.ToNextScene();
                bCanInput = false;
            }
        }
    }

    void Initialize()
    {
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody.gravityScale = 0f;

        direction = 1f;
        directionFactor = 0f;

        bCanInput = true;
    }




    public override eObjectKey ObjectKey { get => eObjectKey.PLAYER_WORLD; }
    public override eGroupKey GroupKey { get => eGroupKey.PLAYER; }



    Rigidbody2D rigidbody;
    float directionFactor;
    float speed = 3f;

    Animator animator;

    bool bCanInput;
}
