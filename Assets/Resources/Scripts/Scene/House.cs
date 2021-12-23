using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Scene
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
        player.SetActive(false);
        ObjectManager.Instance.RecallAll();
    }

    void Initialize()
    {
        Debug.Log(bLoad);

        // 플레이어 설정
        player = ObjectManager.Instance.Player;
        player.SetActive(true);
        player.transform.position = playerSpawnPos;

        // trigger 설정
        GameObject obj = ObjectManager.Instance.NewObject(eObjectKey.TRIGGER, worldPosition);
        obj.GetComponent<Trigger>().SceneKey = eSceneKey.WORLD;
        obj = ObjectManager.Instance.NewObject(eObjectKey.TRIGGER, tutorialPosition);
        obj.GetComponent<Trigger>().SceneKey = eSceneKey.TUTORIAL;

        // 카메라 설정
        camera.SetPositionOffset(cameraOffset);
        camera.CameraState = CameraController.eCameraState.STAGE;
        camera.SetTarget(player.transform);

        ObjectManager.Instance.NewObject(eObjectKey.SCENE_CHANGE_OPEN);
    }



    public override eSceneKey SceneKey => eSceneKey.HOUSE;
    public override Rect Boundary => new Rect(-5.8f, -2.5f, 11.2f, 6f);

    GameObject player;
    GameObject boss;

    Vector3 playerSpawnPos = new Vector3(-3.5f, -2.5f);
    Vector3 cameraOffset = new Vector3(-0.3f, 0f, -2f);

    Vector3 worldPosition = new Vector3(-6f, -1.5f);
    Vector3 tutorialPosition = new Vector3(-0.2f, -1f);
}
