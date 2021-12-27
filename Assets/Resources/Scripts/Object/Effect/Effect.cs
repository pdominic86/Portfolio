using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : Prefab
{
    protected void Initialize(float _time)
    {
        StartCoroutine(CoroutineFunc.DelayOnce(() => { ObjectManager.Instance.RecallObject(gameObject); }, _time));
    }


    // ** Getter & Setter
    public override eGroupKey GroupKey => eGroupKey.EFFECT; 
    public override eObjectKey ObjectKey { get; }

    protected float endTime;
}
