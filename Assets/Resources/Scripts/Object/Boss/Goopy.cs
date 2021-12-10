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

        // 방향 설정
        direction = -1f;

        // State 설정
        phase = ePhase.SPAWN;
        action = eAction.INTRO;
        actionState = eActionState.READY;
    }

    private void Start()
    {
        // Target 설정
        targetPosition = FindObjectOfType<PlayerController>().transform;

        // Boundary 설정
        boundaryX = SceneManager.Instance.BoundaryX;
        boundaryX.x += collider.offset.x + collider.size.x * 0.5f;
        boundaryX.y += collider.offset.x - collider.size.x * 0.5f;
    }

    void Update()
    {
        switch (phase)
        {
            case ePhase.SPAWN:
                {
                    if (actionState == eActionState.READY)
                    {
                        animator.SetTrigger("spawn");
                        actionState = eActionState.ACT;
                        StartCoroutine(Func.DelayCoroutine(() => { actionState = eActionState.FINISH; }, spawnDelay));
                    }
                    else if(actionState == eActionState.FINISH)
                    {
                        phase = ePhase.PHASE_1;
                        action = eAction.IDLE;
                        actionState = eActionState.READY;
                    }
                }
                break;
            case ePhase.PHASE_1:
                if (action == eAction.IDLE)
                {
                    if(jumpCount> maxCount)
                    {
                        if ((direction < 0f && targetPosition.position.x < transform.position.x) || (direction > 0f && targetPosition.position.x > transform.position.x))
                        {
                            jumpCount = 0;
                            action = eAction.ATTACK;
                            actionState = eActionState.READY;
                        }
                            animator.SetTrigger("punch");
                            StartCoroutine(DelayCoroutine(() => { eState = State.IDLE; }, 1.5f));
                            return;
                    }
                    ++jumpCount;
                    eState = State.JUMP_READY;
                    StartCoroutine(DelayCoroutine(() => { eState = State.JUMP; }, 1f));
                }
                else if(eState == State.JUMP)
                {
                    rigidbody.gravityScale = gravity;
                    rigidbody.AddForce(new Vector2(direction, 3f) * Random.Range(minJumpForce, maxJumpForce), ForceMode2D.Impulse);
                    eState = State.IN_AIR;
                }

                if (hp < phase2Start)
                {
                    ePhase = Phase.PHASE_2;
                    eState = State.INTRO;
                }
                    break;
            case Phase.PHASE_2:

                    break;
            case Phase.PHASE_3:
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        Vector3 position = transform.position;
        if (position.x< boundaryX.x || position.x > boundaryX.y)
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

        if (transform.position.y < 0f)
        {
            eState = State.IDLE;
            rigidbody.gravityScale = 0f;
            rigidbody.velocity = Vector2.zero;
            position.y = 0f;
            transform.position = position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Prefab target=collision.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;

        if(target.GroupKey==eGroupKey.BULLET)
        {
            Bullet bullet = target as Bullet;
            hp -= bullet.Damage;
            ObjectManager.Instance.RecallObject(collision.gameObject);
            Debug.Log(hp);
        }
    }

    IEnumerator DelayCoroutine(DelayAction _func,float time)
    {
        yield return new WaitForSeconds(time);
        _func();
    }



    // Getter && Setter
    public override eObjectKey ObjectKey { get => eObjectKey.GOOPY; }




    // 점프 관련
    Rigidbody2D rigidbody;
    float minJumpForce = 4.1f;
    float maxJumpForce = 5.1f;
    float gravity = 3f;
    // phase 공격패턴 관련
    Transform targetPosition;
    int jumpCount;
    int maxCount =4;
    
    // 상태 관련
    [SerializeField] ePhase phase;
    [SerializeField] eAction action;
    [SerializeField] eActionState actionState;


    // 애니메이션 관련
    Animator animator;
    float spawnDelay = 2f;


    // 충돌 관련
    CapsuleCollider2D collider;
    int hp=200;
    const int phase2Start = 130;
    const int phase3Start = 60;

    // coroutine 사용 함수
    delegate void DelayAction();

    enum ePhase { SPAWN, PHASE_1, PHASE_2, PHASE_3 }
    enum eAction { INTRO, IDLE, JUMP,  ATTACK }
    enum eActionState { READY, START, ACT, FINISH }
}
