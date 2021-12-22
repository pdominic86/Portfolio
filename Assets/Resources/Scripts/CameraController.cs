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

    private void LateUpdate()
    {
        switch (cameraState)
        {
            case eCameraState.STAY:
                break;

            case eCameraState.STAGE:
                if (targetPos)
                {
                    float offsetX = targetPos.position.x * horizontalRatio * moveRange;
                    transform.position = new Vector3(offsetX, positionOffset.y, positionOffset.z);
                }
                break;

            case eCameraState.FOLLOW:
                break;
        }

    }

    public void SetTarget(Transform _transform)
    {
        targetPos = _transform;
    }

    public void SetPositionOffset(Vector3 _offset)
    {
        positionOffset = _offset;
        transform.position = _offset;
    }

    public eCameraState CameraState
    {
        get => cameraState;
        set => cameraState = value;
    }


    Rect screenRect = new Rect(0f, 0f, 16f, 9f);
    Vector3 positionOffset = new Vector3(0f, 0f,-10f);

    eCameraState cameraState;

    Transform targetPos;
    float moveRange = 0.5f;
    float horizontalRatio;



    public enum eCameraState { STAY,FOLLOW,STAGE}
}
