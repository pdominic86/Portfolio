using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Player
{
    private void Awake()
    {
        base.Awake();
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetAnimator();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (bLoad)
            Initialize();
    }

    private void OnDisable()
    {
        base.OnDisable();
        rigidbody.velocity = Vector2.zero;
        Direction = 1f;
    }

    void Update()
    {
        if (!bCanInput || bExmove)
            return;

        // input check
        ulong input = Keys.InputCheck();

        // 방향전환 관련
        // dash 상태가 아닐때,
        if (!bDash)
        {
            // 좌우방향키를 입력하면
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
        }

        // 이미지 방향 전환
        Vector3 scale = transform.localScale;
        if (direction * scale.x < 0f)
        {
            scale.x *= -1f;
            transform.localScale = scale;
        }

        // 애니메이션을 재생할 변수들을 키입력에 따라 변경
        // lock키
        bLock = ((input & Keys.locked) != 0 ? true : false);
        // duck 키 - dash 나 jump 상태일때 입력 불가
        bDuck = (bJump || bDash ? false : (input & Keys.down) != 0);
        // shoot 키
        bShoot = ((input & Keys.shoot) != 0 ? true : false);
        // 이동키는 lock,duck,dash 상태 일때 애니메이션에 영향을 주지 못함
        if(bLock || bDuck || bDash)
            bMove =false;

        if ((input & Keys.down) != 0 && (input & Keys.jump) != 0)
        {
            bDropDown = true;
            if (platformCollider != null)
            {
                platformCollider.isTrigger = true;
                platformCollider = null;
                bJump = true;
                animator.SetBool(parameterList[eState.JUMP], true);
                return;
            }
        }
        else
            bDropDown = false;

        if (!bDuck && !bLock)
        {
            // dash 관련
            if (bCanDash && (input & Keys.dash) != 0)
            {
                bDash = true;
                bCanDash = false;
                bHitable = false;
                
                rigidbody.gravityScale = 0f;
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(dashForce * direction, ForceMode2D.Impulse);
                
                animator.SetTrigger(parameterList[eState.DASH]);
                StartCoroutine(CoroutineFunc.DelayOnce(DashEnd, dashDelay * 0.5f));
                StartCoroutine(CoroutineFunc.DelayOnce(() =>{ bCanDash = true; }, dashDelay));
                ObjectManager.Instance.NewObject(eObjectKey.DASH_DUST, transform.position+ dashOffset);
            }

            // 패링 관련
            if (bJump && bCanParry && Input.GetKeyDown(Keys.KEY_JUMP))
            {
                bCanParry = false;
                bParry = true;
                animator.SetBool(parameterList[eState.PARRY], true);
                StartCoroutine(CoroutineFunc.DelayOnce(() => { bParry = false; }, parryInterval));
            }

            // 점프 관련
            if (!bJump && !bDash && (input & Keys.jump) != 0)
            {
                bJump = true;
                rigidbody.gravityScale = gravity;
                rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
                SoundManager.Instance.PlaySound(eSoundKey.PLAYER_JUMP, audioSource);
            }
        }

        // 공격 관련
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
                animator.SetFloat(parameterList[eState.SHOOT_POS], poseFactor);

            if (!bFire && SceneManager.Instance.CurrentScene.SceneKey!=eSceneKey.HOUSE)
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
                StartCoroutine(CoroutineFunc.DelayOnce(() => { bFire = false; }, fireDelay));
                SoundManager.Instance.PlaySound(eSoundKey.PLAYER_SHOOT, audioSource);
            }
        }

        // 특수 공격 관련
        if(!bExmove && (input & Keys.exmove)!=0)
        {
            bExmove = true;
            bHitable = false;

            // animation
            InitializeAnimator();
            animator.SetTrigger(parameterList[eState.EXMOVE]);
            StartCoroutine(CoroutineFunc.DelayOnce(ExmoveEnd, exmoveInterval));

            // 물리초기화
            rigidbody.gravityScale = 0f;
            rigidbody.velocity = Vector2.zero;

            // bullet
            int offsetIndex = (direction < 0f ? 2 : 3);
            StartCoroutine(CoroutineFunc.DelayOnce(()=> { ObjectManager.Instance.NewObject(eObjectKey.EX_BULLET, transform.position + fireOffsets[offsetIndex], direction); },exbulletDelay));
            
            // effect
            Vector3 effectPosition = transform.position;
            effectPosition.y += exmoveOffset;
            ObjectManager.Instance.NewObject(eObjectKey.EXMOVE_DUST, effectPosition);
            SoundManager.Instance.PlaySound(eSoundKey.PLAYER_EXMOVE, audioSource);
            return;
        }

        // 이동 관련
        if (bMove)
            transform.position += direction * speed * Time.deltaTime * Vector3.right;

        // 패링 종료 관련
        if(!bCanParry&& (input & Keys.jump) == 0)
        {
            animator.SetBool(parameterList[eState.PARRY], false);
            bParry = false;
        }

        // animation 최종
        bool runState = (bJump ? false : bMove);
        bool jumpState = (bDash ? false : bJump);
        animator.SetBool(parameterList[eState.RUN], runState);
        animator.SetBool(parameterList[eState.JUMP], jumpState);
        animator.SetBool(parameterList[eState.SHOOT], bFire);
        animator.SetBool(parameterList[eState.DUCK], bDuck);
    }

    private void LateUpdate()
    {
        // boundary 관련
        Vector3 position = transform.position;
        if (position.x < boundary.xMin || transform.position.x > boundary.xMax)
        {
            position.x = (position.x < boundary.xMin ? boundary.xMin : boundary.xMax);
            transform.position = position;
        }

        // jump 종료 관련
        if (transform.position.y < boundary.yMin)
        {
            if (bJump)
            {
                ObjectManager.Instance.NewObject(eObjectKey.JUMP_DUST, transform.position);
                bJump = false;
            }
            bParry = false;
            bCanParry = true;
            rigidbody.gravityScale = 0f;
            rigidbody.velocity = Vector2.zero;
            position.y = boundary.yMin;
            transform.position = position;
            animator.SetBool(parameterList[eState.JUMP], false);
            animator.SetBool(parameterList[eState.PARRY], false);
        }
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
                trigger.ShowScene(this);
                bCanInput = false;
            }
        }
        else if(targetKey == eGroupKey.PLATFORM)
        {
            if(transform.position.y>collision.transform.position.y && !bDropDown)
            {
                collision.isTrigger = false;
                platformCollider = collision;

                Vector3 position = transform.position;
                if (bJump)
                {
                    ObjectManager.Instance.NewObject(eObjectKey.JUMP_DUST, transform.position);
                    bJump = false;
                }

                rigidbody.gravityScale = 0f;
                rigidbody.velocity = Vector2.zero;
                animator.SetBool(parameterList[eState.JUMP], false);
            }
        }
        else if(targetKey==eGroupKey.PARRY)
        {
            if(bParry)
            {
                bCanParry = true;
                // 점프
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(hitForce, ForceMode2D.Impulse);
                // effect
                Vector3 targetPos = collision.transform.position;
                ObjectManager.Instance.NewObject(eObjectKey.PARRY_AURA, targetPos);
                ObjectManager.Instance.NewObject(eObjectKey.PARRY_HIT, targetPos);
                ObjectManager.Instance.RecallObject(collision.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {     
        Vector3 position = transform.position;
        if (position.y > collision.transform.position.y)
        {
            if (bJump)
            {
                ObjectManager.Instance.NewObject(eObjectKey.JUMP_DUST, transform.position);
                bJump = false;
            }

            rigidbody.gravityScale = 0f;
            rigidbody.velocity = Vector2.zero;
            animator.SetBool(parameterList[eState.JUMP], false);
            animator.SetBool(parameterList[eState.PARRY], false);
        }
        
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        Vector3 position = transform.position;
        if (position.y > boundary.yMin)
        {
            rigidbody.gravityScale = gravity;
        }

        Prefab target = collider.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;
        eGroupKey targetKey = target.GroupKey;
        if (targetKey == eGroupKey.PLATFORM)
        {
            platformCollider = null;
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
        animator.SetTrigger(parameterList[eState.INTRO]);
        StartCoroutine(CoroutineFunc.DelayOnce(() => { bCanInput = true; }, introDelay));
        SoundManager.Instance.PlaySound(eSoundKey.PLAYER_INTRO, audioSource);

        hp = maxHP;

        // boolean 초기화

        // 입력 상태 관련
        bMove = false;
        bLock = false;
        bShoot = false;
        bDuck = false;
        bJump = false;
        // 패링 관련
        bCanParry = true;
        bParry = false;
        bCanDash=true;
        bDash = false;
        bFire = false;
        bExmove = false;
        bHitable = true;
        bDropDown = false;
    }

    public void Hit()
    {
        if (!bHitable)
            return;

        --hp;

        if (hp < 1)
        {
            ObjectManager.Instance.NewObject(eObjectKey.DEAD_DUST, transform.position);
            gameObject.SetActive(false);
            return;
        }
        InitializeAnimator();
        animator.SetTrigger("hit");
        bHitable = false;
        bCanInput = false;
        StartCoroutine(CoroutineFunc.DelayOnce(() => { bCanInput = true; }, 0.6f));
        StartCoroutine(CoroutineFunc.DelayOnce(()=> { bHitable = true; }, hitDelay));
        StartCoroutine(Blink());

        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = gravity;
        rigidbody.AddForce(hitForce, ForceMode2D.Impulse);

        ObjectManager.Instance.NewObject(eObjectKey.HIT, transform.position);
    }

    void InitializeAnimator()
    {
        for (eState state = 0; state <= eState.EXMOVE; ++state)
        {
            switch (state)
            {
                case eState.RUN:
                case eState.JUMP:
                case eState.PARRY:
                case eState.DUCK:
                case eState.SHOOT:
                    animator.SetBool(parameterList[state], false);
                    break;
            }
        }
    }

    void SetAnimator()
    {
        // 애니메이터를 불러옴
        animator = GetComponent<Animator>();

        // 파라메터 저장
        var parameters = animator.parameters;
        foreach (var parameter in parameters)
        {
            switch (parameter.name)
            {
                case "intro":
                    parameterList.Add(eState.INTRO, parameter.nameHash);
                    break;
                case "run":
                    parameterList.Add(eState.RUN, parameter.nameHash);
                    break;
                case "jump":
                    parameterList.Add(eState.JUMP, parameter.nameHash);
                    break;
                case "parry":
                    parameterList.Add(eState.PARRY, parameter.nameHash);
                    break;
                case "duck":
                    parameterList.Add(eState.DUCK, parameter.nameHash);
                    break;
                case "dash":
                    parameterList.Add(eState.DASH, parameter.nameHash);
                    break;
                case "hit":
                    parameterList.Add(eState.HIT, parameter.nameHash);
                    break;
                case "shoot":
                    parameterList.Add(eState.SHOOT, parameter.nameHash);
                    break;
                case "shoot_pose":
                    parameterList.Add(eState.SHOOT_POS, parameter.nameHash);
                    break;
                case "exmove":
                    parameterList.Add(eState.EXMOVE, parameter.nameHash);
                    break;
                default:
                    break;
            }
        }
    }

    void DashEnd()
    {
        bDash = false;
        bHitable = true;
        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = gravity;
    }

    void ExmoveEnd()
    {
        bExmove = false;
        bHitable = true;
        rigidbody.gravityScale = gravity;
        if (bJump)
            animator.SetBool(parameterList[eState.JUMP], true);
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
    public override eObjectKey ObjectKey => eObjectKey.PLAYER;
    public int HP => hp;




    // ** Field
    // intro animation variable
    float introDelay = 2f;

    // 이동 관련
    float speed = 3f;
    bool bMove;

    // 입력 상태 관련
    bool bLock;
    bool bShoot;
    bool bDuck;

    // 점프 관련
    Rigidbody2D rigidbody;
    Vector2 jumpForce = new Vector2(0f, 12f);
    float gravity = 2.5f;
    bool bJump;
    // 패링 관련
    bool bParry;
    bool bCanParry;
    float parryInterval = 0.2f;

    // 대쉬 관련
    bool bDash;
    bool bCanDash;
    Vector2 dashForce = new Vector2(10f, 0f);
    Vector3 dashOffset = new Vector3(0f, 0.3f);
    float dashDelay = 0.8f;

    // 충돌 관련
    CapsuleCollider2D collider;
    bool bHitable;
    float hitDelay = 1.5f;
    Vector2 hitForce= new Vector2(0f, 8f);

    // platform 충돌 관련
    bool bDropDown;
    Collider2D platformCollider;
    // 체력
    int maxHP = 5;
    int hp;


    // 공격 관련
    Vector3[] fireOffsets = 
       { new Vector3(-0.8f, 0.3f, 0f), new Vector3(0.8f, 0.3f, 0f), new Vector3(-0.8f, 0.7f, 0f), new Vector3(0.8f, 0.7f, 0f),
    new Vector3(-0.8f, 1.2f, 0f),new Vector3(0.8f, 1.2f, 0f),new Vector3(-0.3f, 1.6f, 0f),new Vector3(0.3f, 1.6f, 0f)};
    bool bFire;
    float fireDelay = 0.2f;

    // 특수 공격 관련
    bool bExmove;
    float exmoveInterval = 0.6f;
    float exbulletDelay = 0.3f;
    float exmoveOffset = 0.6f;

    // 애니메이션 관련
    Animator animator;
    Dictionary<eState, int> parameterList = new Dictionary<eState, int>();
    SpriteRenderer spriteRenderer;
    float blinkDelay = 0.1f;
    bool bFade;
    Color full = Color.white;
    Color fade = new Color(1f, 1f, 1f, 0.5f);

    AudioSource audioSource;
    enum eState { INTRO,RUN,JUMP,PARRY,DUCK,DASH,HIT,SHOOT,SHOOT_POS,EXMOVE}
}
