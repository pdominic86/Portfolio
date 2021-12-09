using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goopy : Prefab
{
    private void Awake()
    {
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 3f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody.simulated = false;
        animator = GetComponent<Animator>();
        animator.SetTrigger("spawn");
        StartCoroutine(DelayCoroutine(() => { ePhase = Phase.PHASE_1; }, 2f));
        targetPosition = FindObjectOfType<PlayerController>().transform;
    }
    void Update()
    {
        switch (ePhase)
        {
            case Phase.PHASE_1:
                if (eState==State.IDLE)
                {
                    if(jumpCount> maxCount)
                    {
                        if ((direction < 0f && targetPosition.position.x < transform.position.x) || (direction > 0f && targetPosition.position.x > transform.position.x))
                        {
                            jumpCount = 0;
                            eState = State.ATTACK;
                            animator.SetTrigger("punch");
                            StartCoroutine(DelayCoroutine(() => { eState = State.IDLE; }, 1.5f));
                            return;
                        }
                    }
                    ++jumpCount;
                    eState = State.JUMP_READY;
                    StartCoroutine(DelayCoroutine(() => { eState = State.JUMP; }, 1f));
                }
                else if(eState == State.JUMP)
                {
                    rigidbody.simulated = true;
                    rigidbody.AddForce(new Vector2(direction, 3f) * Random.Range(minJumpForce, maxJumpForce), ForceMode2D.Impulse);
                    eState = State.IN_AIR;
                }
                else if (eState == State.ATTACK)
                {

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
        if(transform.position.x<-5.7f || transform.position.x > 5.7f)
        {
            Vector3 position = transform.position;
            position.x = (transform.position.x < 0f ? -5.7f : 5.7f);
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
            rigidbody.simulated = false;
            rigidbody.velocity = Vector2.zero;
            Vector3 position = transform.position;
            position.y = 0f;
            transform.position = position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.bounds.size);
    }


    IEnumerator DelayCoroutine(DelayAction _func,float time)
    {
        yield return new WaitForSeconds(time);
        _func();
    }




    // Getter && Setter
    public override eObjectKey ObjectKey
    { get; set; } = eObjectKey.GOOPY;




    // 점프 관련
    Rigidbody2D rigidbody;
    float minJumpForce = 4.1f;
    float maxJumpForce = 5.1f;
    float direction = -1f;

    // phase 공격패턴 관련
    Transform targetPosition;
    int jumpCount;
    int maxCount =4;
    
    // 상태 관련
    [SerializeField] Phase ePhase=Phase.SPAWN;
    [SerializeField] State eState = State.IDLE;


    // 애니메이션 관련
    Animator animator;


    // coroutine 사용 함수
    delegate void DelayAction();

    enum Phase
    { SPAWN,PHASE_1, PHASE_2, PHASE_3 };
    enum State
    {
        IDLE,JUMP_READY,JUMP,IN_AIR, ATTACK_READY, ATTACK
    }

}
