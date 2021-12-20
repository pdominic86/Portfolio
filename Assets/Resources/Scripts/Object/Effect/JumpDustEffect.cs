using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDustEffect : Effect
{
    private void Awake()
    {
        endTime = 0.7f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.JUMP_DUST_EFFECT;

}
