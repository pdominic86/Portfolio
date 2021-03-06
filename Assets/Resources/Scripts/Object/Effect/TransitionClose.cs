using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionClose : Effect
{
    private void Awake()
    {
        endTime = 1.4f;
    }

    private void OnEnable()
    {
        Initialize(endTime);
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition.z = 0;
        transform.position = cameraPosition;
    }
    public override eObjectKey ObjectKey => eObjectKey.SCENE_CHANGE_CLOSE;
}
