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
        collider.direction = CapsuleDirection2D.Horizontal;
        collider.offset = new Vector2(0.05f, 3.1f);
        collider.size = new Vector2(1.95f, 1.43f);
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

    private void OnDisable()
    {
        // 물리 영향 없앰
        rigidbody.gravityScale = gravityScale;
        rigidbody.velocity = Vector2.zero;
    }

    private void Update()
    {
        if (action == eAction.INTRO)
        {
            if (actionState == eActionState.READY)
            {
                Vector3 position = transform.position;
                if (rigidbody.gravityScale > 1f && position.y < 0f)
                {
                    rigidbody.gravityScale = 0f;
                    rigidbody.velocity = Vector3.zero;
                    position.y = 0f;
                    transform.position = position;

                    actionState = eActionState.START;
                }
            }
            else if(actionState == eActionState.START)
            {
                animator.SetBool("intro", true);
                actionState = eActionState.ACT;
                StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.FINISH; }, introDelay));
            }
            else if (actionState == eActionState.FINISH)
            {
                animator.SetBool("intro", false);
                action = eAction.MOVE;
                actionState = eActionState.SELECT;
                StartCoroutine(CoroutineFunc.DelayCoroutine(() => { action = eAction.MOVE; actionState = eActionState.SELECT; }, turnDelay));
                if (targetPosition.position.x < transform.position.x)
                    direction = -1f;
                else
                    direction = 1f;
            }
        }
        else if (action == eAction.MOVE)
        {
            if(actionState==eActionState.SELECT)
            {
                if(direction<0f)
                    animator.SetBool("left", true);
                else
                    animator.SetBool("right", true);

                StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.ACT; }, turnDelay));
            }
            else if(actionState==eActionState.ACT)
            {
                transform.position += direction * speed * Time.deltaTime * Vector3.right;
                Vector3 position = transform.position;
                if(position.x<boundaryX.x)
                {
                    actionState = eActionState.FINISH;
                    position.x = boundaryX.x;
                    transform.position = position;
                    animator.SetBool("left", false);
                    direction *= -1f;
                    StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.SELECT; }, turnDelay));
                }
                else if(position.x > boundaryX.y)
                {
                    actionState = eActionState.FINISH;
                    position.x = boundaryX.y;
                    transform.position = position;
                    animator.SetBool("right", false);
                    direction *= -1f;
                    StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.SELECT; }, turnDelay));
                }


            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("call");
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
            ObjectManager.Instance.NewObject(eObjectKey.GOOPY_EXPLODE, transform.position);
        }
    }

    // ** self_definede
    private void Initialize()
    {
        // 상태
        action = eAction.INTRO;
        actionState = eActionState.READY;

        // Boundary 설정
        boundaryX = SceneManager.Instance.BoundaryX;
        boundaryX.x += collider.offset.x + collider.size.x * 0.5f;
        boundaryX.y += collider.offset.x - collider.size.x * 0.5f;
        Debug.Log(boundaryX);

        // 방향
        direction = -1f;

        // 체력
        hp = maxHp;

        // target
        targetPosition = FindObjectOfType<PlayerController>().transform;
    }

    // ** Getter & Setter
    public override eObjectKey ObjectKey => eObjectKey.TOMBSTONE;


    Rigidbody2D rigidbody;
    CapsuleCollider2D collider;
    Animator animator;
    float speed=10f;
    float gravityScale = 5f;

    float introDelay = 2f;
    float turnDelay = 0.25f;

    eAction action;
    eActionState actionState;

    Transform targetPosition;

    enum eAction { INTRO, IDLE, MOVE, ATTACK, DEATH }
    enum eActionState { READY, SELECT, START, ACT, FINISH }
}
