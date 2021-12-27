using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExBulletHitEffect : Effect
{
    // Start is called before the first frame update
    private void Awake()
    {
        endTime = 0.5f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.EX_BULLET_HIT;
}
