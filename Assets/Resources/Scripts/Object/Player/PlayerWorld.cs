using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorld : Prefab
{
    private void OnEnable()
    {
        
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
    }





    public override eObjectKey ObjectKey { get => eObjectKey.PLAYER_WORLD; }
    public override eGroupKey GroupKey { get => eGroupKey.PLAYER; }
}
