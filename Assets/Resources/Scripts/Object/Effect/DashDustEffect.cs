using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDustEffect : Effect
{
    private void Awake()
    {
        endTime = 0.5f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.DASH_DUST;
}
