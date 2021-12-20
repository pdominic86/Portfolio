using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        camera.rect = screenRect;
        float verticalSize = camera.orthographicSize;
        horizontalRatio = screenRect.height / screenRect.width/ verticalSize;
    }

    private void Start()
    {
        targetPos = ObjectManager.Instance.Player.transform;
    }

    private void LateUpdate()
    {
        if (targetPos)
        {
            float offsetX = targetPos.position.x * horizontalRatio * moveRange;
            transform.position = new Vector3(offsetX, positionOffset.y, positionOffset.z);
        }
    }

    public void SetTarget(Transform _transform)
    {
        targetPos = _transform;
    }

    


    Transform targetPos;
    Rect screenRect = new Rect(0f, 0f, 16f, 9f);
    Vector3 positionOffset=new Vector3(0f,2.2f,-10f);
    float moveRange = 0.5f;
    float horizontalRatio;
}
