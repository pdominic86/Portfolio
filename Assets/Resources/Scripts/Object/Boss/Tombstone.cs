using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : Boss
{
    private void Awake()
    {
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();

        //체력
        maxHp = 300;
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
                if (rigidbody.gravityScale > 1f && position.y < boundary.yMin)
                {
                    rigidbody.gravityScale = 0f;
                    rigidbody.velocity = Vector3.zero;
                    position.y = boundary.yMin;
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

                targetCount = Random.Range(minCount, maxCount);
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
                if(position.x<boundary.xMin || position.x > boundary.xMax)
                {
                    ++moveCount;
                    actionState = eActionState.FINISH;
                    if (position.x < boundary.xMin)
                    {
                        position.x = boundary.xMin;
                        animator.SetBool("left", false);
                    }
                    else
                    {
                        position.x = boundary.xMax;
                        animator.SetBool("right", false);
                    }
                    transform.position = position;
                    direction *= -1f;
                    StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.SELECT; }, turnDelay));
                }
                if (moveCount >=targetCount)
                {
                    Vector2 targetBound = new Vector2(position.x - collider.size.x * 0.5f, position.x + collider.size.x * 0.5f);
                    if(targetPosition.position.x>targetBound.x && targetPosition.position.x < targetBound.y)
                    {
                        action = eAction.ATTACK;
                        actionState = eActionState.START;
                        targetCount = Random.Range(minCount, maxCount);
                        moveCount = 0;
                        Debug.Log("ready");
                    }
                }

            }
        }
        else if (action == eAction.ATTACK)
        {
            if(actionState==eActionState.START)
            {
                Debug.Log("call");
                animator.SetBool("attack", true);
                actionState = eActionState.ACT;
                StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.FINISH; }, attackDelay));
            }
            else if (actionState == eActionState.FINISH)
            {
                animator.SetBool("attack", false);
                action = eAction.MOVE;
                actionState = eActionState.SELECT;
            }

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Prefab target = collision.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;

        eGroupKey targetKey = target.GroupKey;
        if (targetKey == eGroupKey.BULLET)
        {
            Bullet bullet = target as Bullet;
            hp -= bullet.Damage;
            ObjectManager.Instance.RecallObject(collision.gameObject);
        }
        else if (targetKey == eGroupKey.BOSS)
        {
            Boss boss = target as Boss;
            hp = boss.HP;

            ObjectManager.Instance.RecallObject(collision.gameObject);
            ObjectManager.Instance.NewObject(eObjectKey.GOOPY_EXPLODE, transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (action != eAction.ATTACK)
            return;
        Prefab target = collision.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;

        eGroupKey targetKey = target.GroupKey;
        if (targetKey == eGroupKey.PLAYER)
        {
            PlayerController player = target as PlayerController;
            player.Hit();
            //zObjectManager.Instance.NewObject(eObjectKey.)
        }
    }

    // ** self_definede
    private void Initialize()
    {
        // Rigidbody 설정
        rigidbody.gravityScale = gravityScale;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 상태
        action = eAction.INTRO;
        actionState = eActionState.READY;

        // Boundary 설정
        boundary = SceneManager.Instance.CurrentScene.Boundary;
        boundary.xMin += collider.offset.x + collider.size.x * 0.5f;
        boundary.xMax += collider.offset.x - collider.size.x * 0.5f;

        // 방향
        direction = -1f;

        // 체력
        hp = maxHp;

        // target
        targetPosition = ObjectManager.Instance.Player.transform;

        targetCount = 0;
        moveCount = 0;
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
    float attackDelay = 1f;

    int minCount = 5;
    int maxCount = 9;
    int targetCount;
    int moveCount;

    eAction action;
    eActionState actionState;

    Transform targetPosition;

    enum eAction { INTRO, IDLE, MOVE, ATTACK, DEATH }
    enum eActionState { READY, SELECT, START, ACT, FINISH }
}
