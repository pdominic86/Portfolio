using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Parry : Prefab
{
    // ** Getter & Setter
    public override eGroupKey GroupKey => eGroupKey.PARRY;
    public override eObjectKey ObjectKey { get; }
}
