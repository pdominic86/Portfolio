using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopyPhase2Dust : Effect
{
    private void Awake()
    {
        endTime = 1f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.GOOPY_PHASE2_DUST;
}
