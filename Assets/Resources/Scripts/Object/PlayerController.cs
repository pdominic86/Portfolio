using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Prefab
{
    private void Awake()
    {
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Initialize();
    }


    void Update()
    {
        if (!bCanInput)
            return;

        // input check
        ulong input = Keys.InputCheck();

        // 방향전환 관련
        if (!bDash)
        {
            if ((input & Keys.left) != 0)
            {
                direction = -1f;
                bMove = true;
            }
            else if ((input & Keys.right) != 0)
            {
                direction = 1f;
                bMove = true;
            }
            else
                bMove = false;

            Vector3 scale = transform.localScale;
            if (direction * scale.x < 0f)
            {
                scale.x *= -1f;
                transform.localScale = scale;
            }
        }

        // 애니메이션을 재생할 변수들을 키입력에 따라 변경
        bLock = ((input & Keys.locked) != 0 ? true : false);
        bDuck = (bJump || bDash ? false : (input & Keys.down) != 0);
        bShoot = ((input & Keys.shoot) != 0 ? true : false);
        if(bLock || bDuck || bDash)
            bMove =false;

        if (!bDuck && !bLock)
        {
            if (!bCanDash && (input & Keys.dash) != 0)
            {
                bDash = true;
                bCanDash = true;
                bHitable = false;
                rigidbody.gravityScale = 0f;
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(dashForce * direction, ForceMode2D.Impulse);
                StartCoroutine(CoroutineFunc.DelayCoroutine(DashEnd, 0.45f));
                StartCoroutine(CoroutineFunc.DelayCoroutine(() =>{ bCanDash = false; }, 0.8f));
                ObjectManager.Instance.NewObject(eObjectKey.DASH_DUST_EFFECT, transform.position+ dashOffset);
            }

            if (!bJump && !bDash && (input & Keys.jump) != 0)
            {
                bJump = true;
                rigidbody.gravityScale = gravity;
                rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
            }
        }

        if (bShoot)
        {
            int offsetIndex = 0;
            float poseFactor = 0f;

            if ((input & Keys.up) != 0)
            {
                if ((input & Keys.left) != 0 || (input & Keys.right) != 0)
                {
                    poseFactor = 0.5f;
                    offsetIndex = (direction < 0f ? 4 : 5);
                }
                else
                {
                    poseFactor = 1f;
                    offsetIndex = (direction < 0f ? 6 : 7);
                }
            }
            else
                offsetIndex = (direction < 0f ? 2 : 3);

            if (bJump || bDash)
                offsetIndex = (direction < 0f ? 2 : 3);
            else if(bDuck)
                offsetIndex = (direction < 0f ? 0 : 1);
            else
                animator.SetFloat("shoot_pose", poseFactor);

            if (!bFire)
            {

                int angle = 0;
                if ((offsetIndex & 1) == 0)
                    angle = 180;

                if (offsetIndex == 4)
                    angle -= 60;
                else if (offsetIndex == 5)
                    angle = 60;
                else if (offsetIndex == 6 || offsetIndex == 7)
                    angle = 90;

                Vector3 firePosition = transform.position + fireOffsets[offsetIndex];
                ObjectManager.Instance.NewObject(eObjectKey.NORMAL_BULLET_SHOOT, firePosition);
                ObjectManager.Instance.NewObject(eObjectKey.NORMAL_BULLET, firePosition, angle);
                bFire = true;
                StartCoroutine(CoroutineFunc.DelayCoroutine(() => { bFire = false; }, fireDelay));
            }
        }

        // 이동 관련
        if (bMove)
            transform.position += direction * speed * Time.deltaTime * Vector3.right;

        Vector3 position = transform.position;
        if (position.x < boundary.xMin || transform.position.x > boundary.xMax)
        {
            position.x = (position.x < boundary.xMin?boundary.xMin: boundary.xMax);
            transform.position = position;
        }

        // jump 종료 관련
        if (transform.position.y < boundary.yMin)
        {
            if(bJump)
            {
                ObjectManager.Instance.NewObject(eObjectKey.JUMP_DUST_EFFECT, transform.position);
                bJump = false;
            }

            rigidbody.gravityScale = 0f;
            rigidbody.velocity = Vector2.zero;
            position.y = boundary.yMin;
            transform.position = position;
            animator.SetBool("jump", false);
        }

        // animation
        animator.SetBool("dash", bDash);
        animator.SetBool("shoot", bFire);
        animator.SetBool("duck", bDuck);
        if(bJump)
            animator.SetBool("run", false);
        else
            animator.SetBool("run", bMove);
        if (bDash)
            animator.SetBool("jump", false);
        else
            animator.SetBool("jump", bJump);


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Prefab target = collider.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;

        eGroupKey targetKey = target.GroupKey;
        if(targetKey==eGroupKey.TRIGGER)
        {
            if(Input.GetKey(Keys.KEY_SHOOT))
            {
                Trigger trigger = target as Trigger;
                trigger.ToNextScene();
                bCanInput = false;
            }
        }

    }

    // ** self
    void Initialize()
    {
        // Rigidbody 설정
        rigidbody.gravityScale = 0f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 기본 방향 설정
        direction = 1f;

        // Boundary 설정
        boundary = SceneManager.Instance.CurrentScene.Boundary;
        boundary.xMin += collider.offset.x + collider.size.x * 0.5f;
        boundary.xMax += collider.offset.x - collider.size.x * 0.5f;

        // intro
        bCanInput = false;
        animator.SetBool("intro", true);
        StartCoroutine(CoroutineFunc.DelayCoroutine(() => { bCanInput = true; animator.SetBool("intro", false); }, introDelay));

        hp = maxHP;
        bHitable = true;
    }

    public void Hit()
    {
        if (!bHitable)
            return;

        --hp;
        /*
        if (hp < 0)
        {
            SceneManager.Instance.CurrentScene.gameObject.SetActive(false);
            return;
        }
        */
        animator.SetBool("shoot", false);
        animator.SetBool("duck", false);
        animator.SetBool("run", false);
        animator.SetBool("jump", false);
        animator.SetTrigger("hit");
        bHitable = false;
        bCanInput = false;
        StartCoroutine(CoroutineFunc.DelayCoroutine(() => { bCanInput = true; }, 0.6f));
        StartCoroutine(CoroutineFunc.DelayCoroutine(()=> { bHitable = true; }, hitDelay));
        StartCoroutine(Blink());

        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = gravity;
        rigidbody.AddForce(hitForce, ForceMode2D.Impulse);
    }

    void DashEnd()
    {
        bDash = false;
        bHitable = true;
        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = gravity;
    }

    IEnumerator Blink()
    {
        bFade = false;
        while (!bHitable)
        {
            yield return new WaitForSeconds(blinkDelay);
            if (!bFade)
                spriteRenderer.color = full;
            else
                spriteRenderer.color = fade;
            bFade ^= true;
        }

        bFade = false;
        spriteRenderer.color = full;
    }



    // ** Getter && Setter
    // key
    public override eObjectKey ObjectKey { get => eObjectKey.PLAYER; }
    public override eGroupKey GroupKey { get => eGroupKey.PLAYER; }

    // intro animation variable
    bool bCanInput;
    float introDelay = 2f;

    // 이동 관련
    float speed = 3f;
    bool bMove;

    // 점프 관련
    Rigidbody2D rigidbody;
    Vector2 jumpForce = new Vector2(0f, 12f);
    float gravity = 2.5f;
    bool bJump;

    // 대쉬 관련
    bool bDash;
    bool bCanDash;
    Vector2 dashForce = new Vector2(10f, 0f);
    Vector3 dashOffset = new Vector3(0f, 0.3f);

    // 충돌 관련
    CapsuleCollider2D collider;
    bool bHitable;
    float hitDelay = 1.5f;
    Vector2 hitForce= new Vector2(0f, 8f);

    // 체력
    int maxHP = 3;
    int hp;

    // 입력 상태
    bool bLock;
    bool bShoot;
    bool bDuck;

    // 공격 관련
    Vector3[] fireOffsets = 
       { new Vector3(-0.8f, 0.3f, 0f), new Vector3(0.8f, 0.3f, 0f), new Vector3(-0.8f, 0.7f, 0f), new Vector3(0.8f, 0.7f, 0f),
    new Vector3(-0.8f, 1.2f, 0f),new Vector3(0.8f, 1.2f, 0f),new Vector3(-0.3f, 1.6f, 0f),new Vector3(0.3f, 1.6f, 0f)};
    bool bFire;
    float fireDelay = 0.2f;

    // 애니메이션 관련
    Animator animator;
    SpriteRenderer spriteRenderer;
    float blinkDelay = 0.1f;
    bool bFade;
    Color full = Color.white;
    Color fade = new Color(1f, 1f, 1f, 0.5f);
}
