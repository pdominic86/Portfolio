using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryHitEffect : Effect
{
    private void Awake()
    {
        endTime = 0.4f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.PARRY_HIT;
}
