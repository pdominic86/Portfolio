using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goopy : Boss
{
    private void Awake()
    {
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        var colliders = gameObject.GetComponents<CapsuleCollider2D>();
        mainCollider = colliders[0];
        subCollider = colliders[1];
        subCollider.enabled = false;

        // ü��
        maxHp = 300;
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        // ���� ���� ����
        rigidbody.gravityScale = 0f;
        rigidbody.velocity = Vector2.zero;

        // ���� �ʱ�ȭ
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
                        StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.FINISH; }, introDelay_Phase1));
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
                    animator.SetBool("ready", true);
                }
                else if (action == eAction.JUMP)
                {
                    if (actionState == eActionState.SELECT)
                    {
                        if (hp < phase2Start)
                        {
                            --minCount;
                            --maxCount;
                            StartCoroutine(CoroutineFunc.DelayCoroutine(ToPhase2, jumpDelay));
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
                        actionState = eActionState.READY;
                    }
                    else if (actionState == eActionState.ACT)
                    {
                        Vector3 position = transform.position;
                        if (position.x < boundary.xMin || position.x > boundary.xMax)
                        {
                            position.x = (position.x < boundary.xMin ? boundary.xMin : boundary.xMax);
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
                        if (transform.position.y < boundary.yMin)
                        {
                            bDownForce = false;
                            actionState = eActionState.FINISH;
                            rigidbody.gravityScale = 0f;
                            rigidbody.velocity = Vector2.zero;
                            position.y = boundary.yMin;
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
                        StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.FINISH; }, attackDelay_Phase1));
                        subCollider.enabled = true;
                    }
                    else if(actionState==eActionState.FINISH)
                    {
                        action = eAction.JUMP; 
                        actionState = eActionState.SELECT;
                        subCollider.enabled = false;
                    }
                }
                break;
            case ePhase.PHASE_2:
                if (action == eAction.INTRO)
                {
                    if (actionState == eActionState.START)
                    {
                        animator.SetTrigger("intro_phase2");
                        actionState = eActionState.ACT;
                        StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.FINISH; }, introDelay_Phase2));
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
                    animator.SetBool("ready",true);
                }
                else if (action == eAction.JUMP)
                {
                    if (actionState == eActionState.SELECT)
                    {
                        if (hp < phase3Start)
                            action = eAction.DEATH;
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
                        actionState = eActionState.READY;
                    }
                    else if (actionState == eActionState.ACT)
                    {
                        Vector3 position = transform.position;
                        if (position.x < boundary.xMin || position.x > boundary.xMax)
                        {
                            position.x = (position.x < boundary.xMin ? boundary.xMin : boundary.xMax);
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
                        if (transform.position.y < boundary.yMin)
                        {
                            bDownForce = false;
                            actionState = eActionState.FINISH;
                            rigidbody.gravityScale = 0f;
                            rigidbody.velocity = Vector2.zero;
                            position.y = boundary.yMin;
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
                    if (actionState == eActionState.START)
                    {
                        animator.SetTrigger("punch");
                        actionState = eActionState.ACT;
                        StartCoroutine(CoroutineFunc.DelayCoroutine(() => { actionState = eActionState.FINISH; }, attackDelay_Phase2));
                        subCollider.enabled = true;
                    }
                    else if (actionState == eActionState.FINISH)
                    {
                        action = eAction.JUMP;
                        actionState = eActionState.SELECT;
                        subCollider.enabled = false;
                    }
                }
                else if (action == eAction.DEATH)
                {
                    if(actionState==eActionState.READY)
                    {
                        actionState = eActionState.ACT;
                        animator.SetBool("death", true);
                        StartCoroutine(CoroutineFunc.DelayCoroutine(ToPhase3, introDelay_Phase3));
                    }
                }
                    break;

            default:
                break;
        }
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
            ObjectManager.Instance.NewObject(eObjectKey.NORMAL_BULLET_HIT,collision.transform.position);
        }
    }





    // ** Self-defind
    // �ʱ�ȭ (OnEnable, Start���� ���� ����)
    private void Initialize()
    {
        // Rigidbody ����
        rigidbody.gravityScale = 0f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Target ����
        targetPosition = ObjectManager.Instance.Player.transform;

        mainCollider.direction = CapsuleDirection2D.Horizontal;
        mainCollider.offset = new Vector2(0f, 0.88f);
        mainCollider.size = new Vector2(1.49f, 1.49f);
        subCollider.offset = new Vector2(0f, 0.88f);
        subCollider.size = new Vector2(1.49f, 1.49f);

        // Boundary ����
        boundary = SceneManager.Instance.CurrentScene.Boundary;
        boundary.xMin += mainCollider.offset.x + mainCollider.size.x * 0.5f;
        boundary.xMax += mainCollider.offset.x - mainCollider.size.x * 0.5f;

        // ���� ����
        direction = -1f;

        // State ����
        phase = ePhase.PHASE_1;
        action = eAction.INTRO;
        actionState = eActionState.START;

        // jump count
        jumpCount = 0;
        targetCount = 0;

        //ü��
        hp = maxHp;
    }

    // ���� �Լ�
    private void Jump()
    {
        rigidbody.gravityScale = gravityScale;
        Vector2 force = new Vector2(direction, 3f) * Random.Range(minJumpForce, maxJumpForce);
        rigidbody.AddForce(force, ForceMode2D.Impulse);
        actionState = eActionState.ACT;
        animator.SetBool("jump", true);
    }

    private void ToPhase2()
    {
        phase = ePhase.PHASE_2;
        action = eAction.INTRO;
        actionState = eActionState.START;
        jumpCount = 0;
        animator.SetBool("ready", false);
        mainCollider.offset = new Vector2(0f, 1.6f);
        mainCollider.size = new Vector2(2.8f, 2.8f);
        subCollider.offset = new Vector2(0f, 1.6f);
        subCollider.size = new Vector2(2.8f, 2.8f);
        boundary = SceneManager.Instance.CurrentScene.Boundary;
        boundary.xMin += mainCollider.offset.x + mainCollider.size.x * 0.5f;
        boundary.xMax += mainCollider.offset.x - mainCollider.size.x * 0.5f;
    }

    private void ToPhase3()
    {
        ObjectManager.Instance.NewObject(eObjectKey.TOMBSTONE, transform.position + new Vector3(0f, height));
    }


    // **  Getter && Setter
    public override eObjectKey ObjectKey  => eObjectKey.GOOPY; 



    // ���� ����
    Rigidbody2D rigidbody;
    float minJumpForce = 5.6f;
    float maxJumpForce = 7.8f;
    float gravityScale = 5f;
    bool bDownForce;

    // ���� ����
    Transform targetPosition;
    int jumpCount;
    int targetCount;
    int minCount = 4;
    int maxCount = 8;

    // ���� ����
    [SerializeField] ePhase phase;
    [SerializeField] eAction action;
    [SerializeField] eActionState actionState;


    // �ִϸ��̼� ����
    Animator animator;
    float introDelay_Phase1 = 2.8f;
    float introDelay_Phase2 = 5f;
    float introDelay_Phase3 = 3f;
    float jumpDelay = 0.7f;
    float attackDelay_Phase1 = 1.4f;
    float attackDelay_Phase2 = 2.5f;

    // Phase3 ����
    float height = 10f;

    // �浹 ����
    CapsuleCollider2D mainCollider;
    CapsuleCollider2D subCollider;
    const int phase2Start = 280;
    const int phase3Start = 260;

    // coroutine ��� �Լ�
    delegate void DelayAction();

    enum ePhase { PHASE_1, PHASE_2 }
    enum eAction { INTRO, IDLE, JUMP, ATTACK, DEATH }
    enum eActionState { READY, SELECT, START, ACT, FINISH }
}
