using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTextEffect : Effect
{
    private void Awake()
    {
        endTime = 3f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
    }
    public override eObjectKey ObjectKey => eObjectKey.FIGHT_TEXT;
}
