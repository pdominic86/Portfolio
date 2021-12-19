using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDustEffect : Effect
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
    public override eObjectKey ObjectKey => eObjectKey.DASH_DUST_EFFECT;
}
