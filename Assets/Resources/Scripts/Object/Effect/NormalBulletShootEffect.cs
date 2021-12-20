using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletShootEffect : Effect
{
    private void Awake()
    {
        endTime = 0.3f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.NORMAL_BULLET_SHOOT;
}
