using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Scene
{
    public void Awake()
    {
        activeStart = false;
    }

    private void OnEnable()
    {
        if(activeStart)
            Initialize();
    }

    private void OnDisable()
    {
        activeStart = true;
    }

    void Initialize()
    {
        // 플레이어 설정
        player = ObjectManager.Instance.Player;
        player.SetActive(true);
        player.transform.position = playerSpawnPos;

        // trigger 설정
        ObjectManager.Instance.NewObject(eObjectKey.TO_TUTORIAL);
        ObjectManager.Instance.NewObject(eObjectKey.TO_WORLD);

        // 카메라 설정
        camera.SetPositionOffset(positionOffset);
        camera.SetTarget(player.transform);
        camera.CameraState = CameraController.eCameraState.STAGE;

        ObjectManager.Instance.NewObject(eObjectKey.SCENE_CHANGE_OPEN, transitionOffset);
    }



    public override eSceneKey SceneKey => eSceneKey.HOUSE;
    public override Rect Boundary => new Rect(-5.5f, -0.3f, 11f, 9f);

    GameObject player;
    GameObject boss;
    Vector3 playerSpawnPos = new Vector3(-3.5f, 0f);
    Vector3 positionOffset = new Vector3(0f, 2.2f, -2f);
    Vector3 transitionOffset = new Vector3(-0.3f, 2.2f, 0f);


    bool activeStart;

}
