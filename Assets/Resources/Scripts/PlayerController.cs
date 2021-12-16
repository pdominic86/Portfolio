using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Prefab
{
    private void Awake()
    {
        // Rigidbody 설정
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Collider 설정
        collider = gameObject.AddComponent<CapsuleCollider2D>();
        collider.offset = new Vector2(0f, 0.64f);
        collider.size = new Vector2(0.43f, 0.95f);
        collider.isTrigger = true;

        animator = GetComponent<Animator>();

        // 기본 방향 설정
        direction = 1f;
    }

    private void Start()
    {
        // Boundary 설정
        boundaryX = SceneManager.Instance.BoundaryX;
        boundaryX.x += collider.offset.x + collider.size.x * 0.5f;
        boundaryX.y += collider.offset.x - collider.size.x * 0.5f;

        action = eAction.IDLE;
        actionState = eActionState.START;
    }


    void Update()
    {
        // input check
        ulong input = Keys.InputCheck();
        SetNextAction(input);
        if ((input & Keys.left)!=0 && direction > 0f)
        {
        }
        if (horizontalInput != 0f)
        {
            // 이동 관련
            transform.position += speed * horizontalInput * Time.deltaTime * Vector3.right;

            // 방향 관련
            direction = (horizontalInput < 0f ? -1f : 1f);
            Vector3 scale = transform.localScale;
            if (direction < 0f && scale.x > 0f || direction > 0f && scale.x < 0f)
            {
                scale.x *= -1f;
                transform.localScale = scale;
            }

            if(action==eAction.IDLE)
            {
                action = eAction.MOVE;
                animator.SetBool("run", true);
            }
        }
        else
        {

        }



        if (Input.GetKey(KeyCode.C))
        {

        }

        // state
        switch (action)
        {
            case eAction.IDLE:
                {
                    if(horizontalInput!=0f)
                    {
                        action = eAction.MOVE;
                        actionState = eActionState.START;


                    }
                }
                break;
            case eAction.MOVE:
                {
                    if (actionState == eActionState.START)
                    {
                        animator.SetBool("run", true);
                        actionState = eActionState.ACT;
                    }
                    else if (actionState == eActionState.ACT)
                    {
                        if(horizontalInput==0f)
                        {
                            action = eAction.IDLE;
                            animator.SetBool("run", false);
                        }
                        else
                        {
                            transform.position += speed * horizontalInput * Time.deltaTime * Vector3.right;
                        }
                    }
                }
                break;
            case eAction.JUMP:
                break;
            case eAction.ATTACK:
                break;
            case eAction.DEATH:
                break;
            default:
                break;
        }



        // boundary 관련
        Vector3 position = transform.position;
        if (position.x < boundaryX.x || transform.position.x > boundaryX.y)
        {
            position.x = (position.x < 0f ? boundaryX.x : boundaryX.y);
            transform.position = position;
        }

        // jump 관련
        if (transform.position.y < 0f)
        {
            bJump = false;
            rigidbody.gravityScale = 0f;
            rigidbody.velocity = Vector2.zero;
            position.y = 0f;
            transform.position = position;
        }

        // 공격
        if (!bFire && Input.GetKey(KeyCode.C))
        {
            Vector3 firePosition = transform.position;
            firePosition += (direction < 0f ? leftOffset : rightOffset);
            ObjectManager.Instance.NewObject(eObjectKey.NORMAL_BULLET, firePosition, direction);
            bFire = true;
            StartCoroutine(CoroutineFunc.DelayCoroutine(() => { bFire = false; }, fireDelay));
        }
        // 점프
        if (!bJump && Input.GetKey(KeyCode.X))
        {
            bJump = true;
            rigidbody.gravityScale = gravity;
            rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.bounds.size);
    }


    void SetNextAction(ulong _input)
    {
        if((_input&Keys.shoot)!=0)
        {
            if((_input&Keys.left)!=0 || (_input & Keys.right) != 0)
            {
                nextAction = eAction.MOVE_AND_ATTACK;
            }
            if ((_input & Keys.left) != 0)
            {

            }
        }
        else
        {

        }
    }

    // ** Getter && Setter
    // key
    public override eObjectKey ObjectKey { get => eObjectKey.PLAYER; }
    public override eGroupKey GroupKey { get => eGroupKey.PLAYER; }

    // 이동 관련
    float speed = 3f;

    // 점프 관련
    Rigidbody2D rigidbody;
    Vector2 jumpForce = new Vector2(0f, 12f);
    float gravity = 2.5f;
    bool bJump;

    // 충돌 관련
    CapsuleCollider2D collider;

    // 공격 관련
    Vector3 rightOffset = new Vector3(0.45f, 0.55f, 0f);
    Vector3 leftOffset = new Vector3(-0.45f, 0.55f, 0f);
    bool bFire;
    float fireDelay = 0.2f;

    // 애니메이션 관련
    Animator animator;


    // 상태 관련
    eAction action;
    eAction nextAction;
    eActionState actionState;

    enum eAction { NONE, INTRO, IDLE, MOVE, JUMP, ATTACK, MOVE_AND_ATTACK,HIT, DEATH }
    enum eActionState { READY, SELECT, START, ACT, FINISH }
}
