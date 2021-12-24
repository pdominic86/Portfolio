using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopyPhase1Dust : Effect
{
    private void Awake()
    {
        endTime = 0.5f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.GOOPY_PHASE1_DUST;
}
