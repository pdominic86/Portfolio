using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletHitEffect : Effect
{
    private void Start()
    {
        endTime = 0.5f;
        Initialize(endTime);
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.NORMAL_BULLET_HIT;
}
