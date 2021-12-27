using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadDustEffect : Effect
{
    private void Awake()
    {
        endTime = 4f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.DEAD_DUST;
}
