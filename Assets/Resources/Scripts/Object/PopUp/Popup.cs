using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Popup : Prefab
{
    public void SetPlayer(Player _player)
    {
        player = _player;
    }


    public virtual void Recall() { }

    // ** Getter & Setter
    public override eGroupKey GroupKey => eGroupKey.BULLET;
    public override eObjectKey ObjectKey { get; }



    // ** Field
    protected Player player;
}
