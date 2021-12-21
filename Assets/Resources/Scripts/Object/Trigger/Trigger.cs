using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : Prefab
{
    public virtual void ToNextScene() { }
    public override eGroupKey GroupKey => eGroupKey.TRIGGER;
    public override eObjectKey ObjectKey { get; }
}
