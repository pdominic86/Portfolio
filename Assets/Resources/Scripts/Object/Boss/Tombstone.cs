using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : Boss
{
    private void Awake()
    {
        base.Awake();
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        //체력
        maxHp = 300;
    }

    private void OnEnable()
    {
        if(bLoad)
            Initialize();
    }

    private void OnDisable()
    {
        base.OnDisable();
        // 물리 영향 없앰
        rigidbody.gravityScale = gravityScale;
        rigidbody.velocity = Vector2.zero;

        ObjectManager.Instance.RecallObject(dustEffect);
    }

    private void Update()
    {
        if (!target.activeSelf)
            return;
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
                    ObjectManager.Instance.NewObject(eObjectKey.TOMBSTONE_INTRO_DUST, transform.position);
                }
            }
            else if(actionState == eActionState.START)
            {
                animator.SetBool("intro", true);
                actionState = eActionState.ACT;
                StartCoroutine(CoroutineFunc.DelayOnce(() => { actionState = eActionState.FINISH; }, introDelay));
            }
            else if (actionState == eActionState.FINISH)
            {
                animator.SetBool("intro", false);
                action = eAction.MOVE;
                actionState = eActionState.SELECT;
                StartCoroutine(CoroutineFunc.DelayOnce(() => { action = eAction.MOVE; actionState = eActionState.SELECT; }, turnDelay));
                if (target.transform.position.x < transform.position.x)
                    direction = -1f;
                else
                    direction = 1f;

                targetCount = Random.Range(minCount, maxCount);
                dustEffect.SetActive(true);
            }
        }
        else if (action == eAction.MOVE)
        {
            if(hp<0)
            {
                action = eAction.DEATH;
                actionState = eActionState.START;
            }
            if(actionState==eActionState.SELECT)
            {
                if(direction<0f)
                    animator.SetBool("left", true);
                else
                    animator.SetBool("right", true);

                StartCoroutine(CoroutineFunc.DelayOnce(() => { actionState = eActionState.ACT; }, turnDelay));
                Vector3 dustScale = dustEffect.transform.localScale;
                dustScale.x = direction;
                dustEffect.transform.localScale = dustScale;
                dustAnimator.SetTrigger("start");
                
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
                    StartCoroutine(CoroutineFunc.DelayOnce(() => { actionState = eActionState.SELECT; }, turnDelay));
                    dustAnimator.SetTrigger("end");
                }
                if (moveCount >=targetCount)
                {
                    Vector2 targetBound = new Vector2(position.x - collider.size.x * 0.8f, position.x + collider.size.x * 0.8f);
                    if(target.transform.position.x>targetBound.x && target.transform.position.x < targetBound.y)
                    {
                        action = eAction.ATTACK;
                        actionState = eActionState.START;
                        targetCount = Random.Range(minCount, maxCount);
                        moveCount = 0;
                    }
                }

            }
        }
        else if (action == eAction.ATTACK)
        {
            Debug.Log(actionState);
            if (actionState==eActionState.START)
            {
                animator.SetBool("attack", true);
                actionState = eActionState.ACT;
                dustEffect.SetActive(false);
                StartCoroutine(CoroutineFunc.DelayOnce(() => { ObjectManager.Instance.NewObject(eObjectKey.TOMBSTONE_ATTACK_DUST,transform.position); SoundManager.Instance.PlaySound(eSoundKey.TOMBSTONE_ATTACK, audioSource); }, attackDelay*0.5f));
                StartCoroutine(CoroutineFunc.DelayOnce(() => { actionState = eActionState.FINISH; }, attackDelay));
                
            }
            else if (actionState == eActionState.FINISH)
            {
                animator.SetBool("attack", false);
                action = eAction.MOVE;
                actionState = eActionState.SELECT;
                dustEffect.SetActive(true);
            }

        }
        else if(action==eAction.DEATH)
        {
            if(actionState==eActionState.START)
            {
                animator.SetBool("death", true);
                target.GetComponent<Player>().SetCanInput(false);
                actionState = eActionState.ACT;
                Vector3 position = Camera.main.transform.position;
                position.z = 0f;
                ObjectManager.Instance.NewObject(eObjectKey.KNOCKDOWN_TEXT, position);
                StartCoroutine(DeathEffect());
                StartCoroutine(CoroutineFunc.DelayOnce(() => { SceneManager.Instance.SetScene(eSceneKey.WORLD); }, 4f));
                
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
            StartCoroutine(Blink());
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
        target = ObjectManager.Instance.Player;

        // effect
        dustEffect = ObjectManager.Instance.NewObject(eObjectKey.TOMBSTONE_MOVE_DUST, transform.position);
        dustEffect.transform.parent = gameObject.transform;
        dustAnimator = dustEffect.GetComponent<Animator>();
        dustEffect.SetActive(false);
        targetCount = 0;
        moveCount = 0;

        SoundManager.Instance.PlaySound(eSoundKey.TOMBSTONE_DROP, audioSource);
    }
    IEnumerator Blink()
    {
        float timeSpend = 0f;
        bFade = true;
        while (timeSpend < blinkTime)
        {
            timeSpend += blinkDelay;
            if (!bFade)
                spriteRenderer.color = full;
            else
                spriteRenderer.color = fade;
            bFade ^= true;
            yield return new WaitForSeconds(blinkDelay);
        }

        bFade = false;
        spriteRenderer.color = full;
    }

    IEnumerator DeathEffect()
    {
        while(true)
        {
            Vector3 positionOffset=new Vector3(Random.Range(-3f,3f), Random.Range(1f, 5f));
            ObjectManager.Instance.NewObject(eObjectKey.EXPLOSION, transform.position+ positionOffset);
            ObjectManager.Instance.NewObject(eObjectKey.EXPLOSION_SIDE, transform.position+ positionOffset);
            yield return new WaitForSeconds(0.5f);
        }
    }
    // ** Getter & Setter
    public override eObjectKey ObjectKey => eObjectKey.TOMBSTONE;


    Rigidbody2D rigidbody;
    CapsuleCollider2D collider;
    Animator animator;
    float speed=10f;
    float gravityScale = 5f;

    float introDelay = 2f;
    float turnDelay = 0.05f;
    float attackDelay = 1f;

    int minCount = 5;
    int maxCount = 9;
    int targetCount;
    int moveCount;

    eAction action;
    eActionState actionState;

    GameObject target;

    // 움직임 효과 관련
    GameObject dustEffect;
    Animator dustAnimator;

    // hit effect 관련
    SpriteRenderer spriteRenderer;
    bool bFade;
    float blinkDelay = 0.05f;
    float blinkTime = 0.3f;
    Color full = Color.white;
    Color fade = new Color(1f, 1f, 1f, 0.5f);

    AudioSource audioSource;

    enum eAction { INTRO, IDLE, MOVE, ATTACK, DEATH }
    enum eActionState { READY, SELECT, START, ACT, FINISH }
}
