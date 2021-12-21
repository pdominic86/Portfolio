using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTutorial : Trigger
{
    public override void ToNextScene()
    {
        SceneManager.Instance.SetScene(eSceneKey.TUTORIAL);
    }
    public override eObjectKey ObjectKey => eObjectKey.TO_TUTORIAL;

}
