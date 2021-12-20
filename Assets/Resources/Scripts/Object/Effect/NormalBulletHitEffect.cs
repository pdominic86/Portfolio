using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletHitEffect : Effect
{
    private void Awake()
    {
        endTime = 0.5f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.NORMAL_BULLET_HIT;
}
