using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ozze : Scene
{
    private void Start()
    {
        topLeftBound = new Vector2(-5.5f, 8f);
        bottomRightBound = new Vector2(5.5f, -0.5f);
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        player.SetActive(false);
        ObjectManager.Instance.RecallObject(boss);
    }

    void Initialize()
    {
        // �÷��̾� ����
        player = ObjectManager.Instance.Player;
        player.SetActive(true);
        player.transform.position = playerSpawnPos;

        // ���� ����
        boss=ObjectManager.Instance.NewObject(eObjectKey.GOOPY, bossSpawnPos);
    }



    public override eSceneKey SceneKey => eSceneKey.OZZE;

    GameObject player;
    GameObject boss;
    Vector3 playerSpawnPos= new Vector3(-3.5f, -0.5f);
    Vector3 bossSpawnPos= new Vector3(3.5f, -0.5f);
}