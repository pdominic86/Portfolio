using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goopy : Boss
{
    private void Awake()
    {
        // Rigidbody 설정
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Collider 설정
        collider = gameObject.AddComponent<CapsuleCollider2D>();
        collider.offset = new Vector2(0f, 0.86f);
        collider.size = new Vector2(1.2f, 1.58f);
        collider.isTrigger = true;

        // Animator 설정
        animator = GetComponent<Animator>();
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
        rigidbody.gravityScale = 0f;
        rigidbody.velocity = Vector2.zero;

        // 방향 초기화
        Vector3 scale = transform.localScale;
        if (scale.x < 0f)
        {
            scale.x = -scale.x;
            transform.localScale = scale;
        }
    }

    void Update()
    {
        switch (phase)
        {

            case ePhase.PHASE_1:
                if (action == eAction.INTRO)
                {
                    if (actionState == eActionState.START)
                    {
                        animator.SetTrigger("intro_phase1");
                        actionState = eActionState.ACT;
                        StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.FINISH; }, introDelay));
                    }
                    else if (actionState == eActionState.FINISH)
                    {
                        action = eAction.IDLE;
                        actionState = eActionState.START;
                    }
                }
                else if (action == eAction.IDLE)
                {
                    targetCount = Random.Range(minCount, maxCount);
                    action = eAction.JUMP;
                    actionState = eActionState.SELECT;
                }
                else if (action == eAction.JUMP)
                {
                    if (actionState == eActionState.SELECT)
                    {
                        if (hp < phase2Start)
                        {
                            phase = ePhase.PHASE_2;
                            action = eAction.INTRO;
                        }
                        else if (jumpCount >= targetCount)
                        {
                            if (targetPosition.position.x < transform.position.x && direction > 0f)
                                Direction = -1f;
                            else if (targetPosition.position.x > transform.position.x && direction < 0f)
                                Direction = 1f;

                            jumpCount = 0;
                            targetCount = Random.Range(minCount, maxCount);
                            StartCoroutine(CoroutineFunc.DelayCoroutine(() => { action = eAction.ATTACK; actionState = eActionState.START; }, jumpDelay));
                        }
                        else
                        {
                            ++jumpCount;
                            StartCoroutine(CoroutineFunc.DelayCoroutine(Jump, jumpDelay));
                        }

                        animator.SetTrigger("ready");
                        actionState = eActionState.READY;
                    }
                    else if (actionState == eActionState.ACT)
                    {
                        Vector3 position = transform.position;
                        if (position.x < boundaryX.x || position.x > boundaryX.y)
                        {
                            position.x = (position.x < 0f ? boundaryX.x : boundaryX.y);
                            transform.position = position;
                            Vector3 force = rigidbody.velocity;
                            force.x *= -1f;
                            rigidbody.velocity = force;
                            direction *= -1f;
                            Vector3 scale = transform.localScale;
                            scale.x *= -1f;
                            transform.localScale = scale;
                        }
                        if (!bDownForce && rigidbody.velocity.y < 0f)
                        {
                            bDownForce = true;
                            animator.SetTrigger("down_force");
                        }
                        if (transform.position.y < 0f)
                        {
                            bDownForce = false;
                            actionState = eActionState.FINISH;
                            rigidbody.gravityScale = 0f;
                            rigidbody.velocity = Vector2.zero;
                            position.y = 0f;
                            transform.position = position;
                        }
                    }
                    else if (actionState == eActionState.FINISH)
                    {
                        animator.SetBool("jump", false);
                        actionState = eActionState.SELECT;
                    }
                }
                else if (action == eAction.ATTACK)
                {
                    if(actionState== eActionState.START)
                    {
                        animator.SetTrigger("punch");
                        actionState = eActionState.ACT;
                        StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.FINISH; }, attackDelay));
                    }
                    else if(actionState==eActionState.FINISH)
                    {
                        action = eAction.JUMP; 
                        actionState = eActionState.SELECT;
                    }
                }


                break;
            case ePhase.PHASE_2:

                break;
            case ePhase.PHASE_3:
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {

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
            Debug.Log(hp);
        }
    }





    // ** Self-defind
    // 초기화 (OnEnable, Start에서 쓰기 위함)
    private void Initialize()
    {
        // Target 설정
        targetPosition = FindObjectOfType<PlayerController>().transform;

        // Boundary 설정
        boundaryX = SceneManager.Instance.BoundaryX;
        boundaryX.x += collider.offset.x + collider.size.x * 0.5f;
        boundaryX.y += collider.offset.x - collider.size.x * 0.5f;

        // 방향 설정
        direction = -1f;

        // State 설정
        phase = ePhase.PHASE_1;
        action = eAction.INTRO;
        actionState = eActionState.START;
    }

    // 점프 함수
    private void Jump()
    {
        rigidbody.gravityScale = gravity;
        Vector2 force = new Vector2(direction, 3f) * Random.Range(minJumpForce, maxJumpForce);
        rigidbody.AddForce(force, ForceMode2D.Impulse);
        actionState = eActionState.ACT;
        animator.SetBool("jump", true);
    }


    // **  Getter && Setter
    public override eObjectKey ObjectKey { get => eObjectKey.GOOPY; }




    // 점프 관련
    Rigidbody2D rigidbody;
    float minJumpForce = 5.6f;
    float maxJumpForce = 6.5f;
    float gravity = 5f;
    bool bDownForce;

    // phase1 공격패턴 관련
    Transform targetPosition;
    int jumpCount;
    int targetCount;
    int minCount = 4;
    int maxCount = 8;

    // 상태 관련
    [SerializeField] ePhase phase;
    [SerializeField] eAction action;
    [SerializeField] eActionState actionState;


    // 애니메이션 관련
    Animator animator;
    float introDelay = 2.8f;
    float jumpDelay = 0.75f;
    float attackDelay = 1.4f;

    // 충돌 관련
    CapsuleCollider2D collider;
    int hp = 200;
    const int phase2Start = 130;
    const int phase3Start = 60;

    // coroutine 사용 함수
    delegate void DelayAction();

    enum ePhase { PHASE_1, PHASE_2, PHASE_3 }
    enum eAction { INTRO, IDLE, JUMP, ATTACK }
    enum eActionState { READY, SELECT, START, ACT, FINISH }
}
