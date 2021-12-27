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
        if (bLoad)
            Initialize();
    }

    private void OnDisable()
    {
        base.OnDisable();
        ObjectManager.Instance.RecallAll();
    }


    void Initialize()
    {
        player = ObjectManager.Instance.NewObject(eObjectKey.PLAYER_WORLD);

        // 카메라 설정
        camera.SetPositionOffset(cameraOffset);
        camera.SetTarget(player.transform);
        camera.CameraState = CameraController.eCameraState.FOLLOW;

        // trigger 설정
        GameObject obj = ObjectManager.Instance.NewObject(eObjectKey.TRIGGER, housePosition);
        obj.transform.localScale = houseScale;
        obj.GetComponent<Trigger>().SceneKey = eSceneKey.HOUSE;
        obj = ObjectManager.Instance.NewObject(eObjectKey.TRIGGER, ozzePosition);
        obj.transform.localScale = ozzeScale;
        obj.GetComponent<Trigger>().SceneKey = eSceneKey.OZZE;

        ObjectManager.Instance.NewObject(eObjectKey.SCENE_CHANGE_OPEN, player.transform.position);
    }

    public override eSceneKey SceneKey => eSceneKey.WORLD;


    GameObject player;
    Vector3 cameraOffset = new Vector3(0f, 0f, -2f);
    Vector3 ozzePosition = new Vector3(14.5f, 0.5f);
    Vector3 ozzeScale = new Vector3(1.5f, 2f);
    Vector3 housePosition = new Vector3(-0.4f, 1f);
    Vector3 houseScale = new Vector3(1.6f, 1f);

}
