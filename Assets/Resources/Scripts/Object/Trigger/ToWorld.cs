using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToWorld : Trigger
{
    public override void ToNextScene() 
    {
        SceneManager.Instance.SetScene(eSceneKey.WORLD);
    }

    public override eObjectKey ObjectKey => eObjectKey.TO_WORLD;

}
