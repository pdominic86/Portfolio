using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : Scene
{
    private void Awake()
    {
        base.Awake();
        instructor = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        if (active)
            Initialize();
    }

    private void OnDisable()
    {
        active = true;
    }

    void Initialize()
    {
        // instructor
        instructor.transform.position = Vector3.zero;

        // 플레이어 설정
        player = ObjectManager.Instance.Player;
        player.SetActive(true);
        player.transform.position = playerSpawnPos;

        // trigger 설정
        GameObject obj = ObjectManager.Instance.NewObject(eObjectKey.TRIGGER, triggerSpawnPos);
        obj.transform.localScale = triggerScale;
        obj.transform.parent = instructor.transform;
        Trigger trigger = obj.GetComponent<Trigger>();
        trigger.SceneKey = eSceneKey.HOUSE;

        // 카메라 설정
        camera.SetPositionOffset(cameraOffset);
        camera.CameraState = CameraController.eCameraState.STAY;
        camera.SetTarget(player.transform);

        ObjectManager.Instance.NewObject(eObjectKey.SCENE_CHANGE_OPEN);
    }

    private void Update()
    {
        Vector3 position = transform.position;
        if (transform.position.x > stageRange.x && transform.position.x < stageRange.y)
        {
            Vector3 playerPosition = player.transform.position;
            float diff = 0f;
            bool bChange = false;

            if (playerPosition.x < moveRange.x)
            {
                diff = moveRange.x - playerPosition.x;
                playerPosition.x = moveRange.x;
                bChange = true;
            }
            else if (playerPosition.x > moveRange.y)
            {
                diff = moveRange.y - playerPosition.x;
                playerPosition.x = moveRange.y;
                bChange = true;
            }

            if (bChange)
            {
                position.x += diff;
                transform.position = position;
                player.transform.position = playerPosition;
            }
        }
    }

    public override eSceneKey SceneKey => eSceneKey.TUTORIAL;
    public override Rect Boundary => new Rect(-4.5f, -2.5f, 9f, 7f);

    GameObject player;

    Vector3 playerSpawnPos = new Vector3(-3f, -2.5f);
    Vector3 triggerSpawnPos = new Vector3(55.95f, -1f);
    Vector3 triggerScale = new Vector3(1.7f, 2f, 1f);
    Vector3 cameraOffset = new Vector3(0f, 0f, -2f);

    Vector2 moveRange = new Vector2(-2f, 2f);
    Vector2 stageRange = new Vector2(0f, 52f);

    /// 튜토리얼 오브젝트 모음
    GameObject instructor;
}
