using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ozze : Scene
{
    private void Awake()
    {
        camera = FindObjectOfType<CameraController>();
        activeStart = true;
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if(activeStart)
        {
            activeStart = false;
            return;
        }
        Initialize();
    }

    private void OnDisable()
    {
        if(player)
            player.SetActive(false);
        ObjectManager.Instance.RecallAll();
    }

    void Initialize()
    {
        // �÷��̾� ����
        player = ObjectManager.Instance.Player;
        player.SetActive(true);
        player.transform.position = playerSpawnPos;

        // ���� ����
        ObjectManager.Instance.NewObject(eObjectKey.GOOPY, bossSpawnPos);

        // ī�޶� ����
        camera.SetPositionOffset(positionOffset);
        camera.SetTarget(player.transform);
        camera.SetSceneKey(eSceneKey.OZZE);

        ObjectManager.Instance.NewObject(eObjectKey.SCENE_CHANGE_OPEN, transitionOffset);
    }



    public override eSceneKey SceneKey => eSceneKey.OZZE;
    public override Rect Boundary => new Rect(-5.5f, -0.3f, 11f, 9f);

    GameObject player;
    GameObject boss;
    Vector3 playerSpawnPos= new Vector3(-3.5f, -0.3f);
    Vector3 bossSpawnPos= new Vector3(3.5f, -0.3f);
    Vector3 positionOffset = new Vector3(0f, 2.2f, -2f);
    Vector3 transitionOffset = new Vector3(-0.3f, 2.2f, 0f);


    bool activeStart;
}
