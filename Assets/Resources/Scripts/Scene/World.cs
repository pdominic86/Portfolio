using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : Scene
{
    private void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        base.OnDisable();
    }
    public override eSceneKey SceneKey => eSceneKey.HOUSE;
}
