using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Awake()
    {
        playerPos = FindObjectOfType<PlayerController>().transform;
        Camera camera = GetComponent<Camera>();
        camera.rect = screenRect;
        float verticalSize = camera.orthographicSize;
        horizontalRatio = screenRect.height / screenRect.width/ verticalSize;
    }

    private void LateUpdate()
    {
        float offsetX = playerPos.position.x * horizontalRatio * moveRange;
        transform.position = new Vector3(offsetX, positionOffset.y, positionOffset.z);
    }


    Transform playerPos;
    Rect screenRect = new Rect(0f, 0f, 16f, 9f);
    Vector3 positionOffset=new Vector3(0f,2.2f,-10f);
    float moveRange = 0.5f;
    float horizontalRatio;
}
