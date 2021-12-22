using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Popup : Prefab
{
    public virtual void Recall() { }

    // ** Getter & Setter
    public override eGroupKey GroupKey => eGroupKey.BULLET;
    public override eObjectKey ObjectKey { get; }
}
