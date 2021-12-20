using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopyExplodeEffect : Effect
{
    private void Awake()
    {
        endTime = 0.5f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }

    // ** Getter & Setter
    public override eObjectKey ObjectKey => eObjectKey.GOOPY_EXPLODE;
}
