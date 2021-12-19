using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopyExplodeEffect : Effect
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

    // ** Getter & Setter
    public override eObjectKey ObjectKey => eObjectKey.GOOPY_EXPLODE;
}
