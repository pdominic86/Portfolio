using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAuraEffect : Effect
{
    private void Awake()
    {
        endTime = 0.2f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.PARRY_AURA;
}
