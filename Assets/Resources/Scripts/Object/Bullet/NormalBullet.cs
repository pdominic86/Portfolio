using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{
    private void Awake()
    {
        collider = gameObject.AddComponent<CapsuleCollider2D>();
        collider.direction = CapsuleDirection2D.Horizontal;
        collider.size = new Vector2(0.15f, 0.07f);
        collider.isTrigger = true;

        direction = 1f;
        speed = 12f;
        damage = 2;
    }

    private void Start()
    {
        SetBoundry();
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * forward;
        if (transform.position.x < boundaryX.x || transform.position.x > boundaryX.y)
            ObjectManager.Instance.RecallObject(gameObject);
    }

    private void OnEnable()
    {
        SetBoundry();
    }



    // ** self-defined
    // Boundary ¼³Á¤
    private void SetBoundry()
    {
        boundaryX = SceneManager.Instance.BoundaryX;
        boundaryX.x += collider.offset.x + collider.size.x * 0.5f;
        boundaryX.y += collider.offset.x - collider.size.x * 0.5f;
    }

    // ** Getter & Setter
    public override eObjectKey ObjectKey => eObjectKey.NORMAL_BULLET;
}
