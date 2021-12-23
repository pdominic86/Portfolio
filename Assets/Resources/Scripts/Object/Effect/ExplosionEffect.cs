using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : Effect
{
    private void Awake()
    {
        endTime = 1f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.EXPLOSION;
}
