using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopyExplodeEffect : Effect
{
    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    void Initialize()
    {
        StartCoroutine(CoroutineFunc.DelayCoroutine(() => { ObjectManager.Instance.RecallObject(gameObject); }, 0.3f));
    }
    // ** Getter & Setter
    public override eObjectKey ObjectKey => eObjectKey.GOOPY_EXPLODE;
}
