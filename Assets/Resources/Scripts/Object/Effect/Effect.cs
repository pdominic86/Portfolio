using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : Prefab
{
    private void Awake()
    {
        StartCoroutine(CoroutineFunc.DelayCoroutine(() => { ObjectManager.Instance.RecallObject(gameObject); }, time));
    }
    // ** Getter & Setter
    public override eGroupKey GroupKey => eGroupKey.EFFECT; 
    public override eObjectKey ObjectKey { get; }



    float time=1f;
}
