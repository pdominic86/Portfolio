using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletShootEffect : Effect
{
    private void Start()
    {
        endTime = 0.3f;
        Initialize(endTime);
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.NORMAL_BULLET_SHOOT;
}
