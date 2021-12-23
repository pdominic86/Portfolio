using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSideEffect : Effect
{
    private void Awake()
    {
        endTime = 0.5f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.EXPLOSION_SIDE;
}
