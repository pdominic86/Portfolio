using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : Prefab
{
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
        colliderSize = collider.size;
        colliderOffset = collider.offset;
    }

    private void OnEnable()
    {
        collider.isTrigger = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Prefab target = collision.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;

        eGroupKey targetKey = target.GroupKey;
        if(targetKey==eGroupKey.PLAYER)
        {
            Vector3 targetPosition = collision.transform.position;
            Vector3 position = transform.position;
            Rect box = new Rect(position.x + colliderOffset.x - colliderSize.x * 0.5f, position.y + colliderOffset.y - colliderSize.y * 0.5f, colliderSize.x, colliderSize.y);
            if(targetPosition.y>box.yMax-0.1f && targetPosition.x>box.xMin && targetPosition.x<box.xMax)
                collider.isTrigger = false;
        }
    }


    public override eObjectKey ObjectKey => eObjectKey.PLATFORM;
    public override eGroupKey GroupKey => eGroupKey.PLATFORM;


    BoxCollider2D collider;
    Vector2 colliderSize;
    Vector2 colliderOffset;
}
