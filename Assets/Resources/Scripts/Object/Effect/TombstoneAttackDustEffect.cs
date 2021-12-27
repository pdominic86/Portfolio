using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombstoneAttackDustEffect : Effect
{
    private void Awake()
    {
        endTime = 0.8f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.TOMBSTONE_ATTACK_DUST;
}
