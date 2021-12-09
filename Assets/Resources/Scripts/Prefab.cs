using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Prefab : MonoBehaviour
{
    // Getter & Setter
    public virtual eObjectKey ObjectKey
    {
        get;set;
    }



    protected eObjectKey objectKey;
}
