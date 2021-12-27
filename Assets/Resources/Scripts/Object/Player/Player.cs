using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Prefab
{

    public void SetCanInput(bool _bCanInput)
    {
        bCanInput = _bCanInput;
    }

    public override eGroupKey GroupKey => eGroupKey.PLAYER;
    public override eObjectKey ObjectKey { get; }


    protected bool bCanInput;
}
