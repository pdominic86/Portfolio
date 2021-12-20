using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionClose : Effect
{
    private void Awake()
    {
        endTime = 1f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.SCENE_CHANGE_CLOSE;
}
