using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Prefab
{
    // Getter & Setter
    public override eGroupKey GroupKey { get => eGroupKey.BOSS; }
    public virtual eObjectKey ObjectKey { get; }
}
