using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Scene
{
    public override eSceneKey SceneKey => eSceneKey.HOUSE;
    Vector3 positionOffset = new Vector3(0f, 0f, -2f);
}
