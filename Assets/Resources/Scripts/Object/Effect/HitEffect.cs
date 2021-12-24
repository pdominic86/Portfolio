using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : Effect
{
    private void Awake()
    {
        endTime = 0.7f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.HIT;
}
