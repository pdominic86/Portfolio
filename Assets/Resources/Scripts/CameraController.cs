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
        switch (currentScene)
        {
            case eSceneKey.HOUSE:
            case eSceneKey.OZZE:
                if (targetPos)
                {
                    float offsetX = targetPos.position.x * horizontalRatio * moveRange;
                    transform.position = new Vector3(offsetX, positionOffset.y, positionOffset.z);
                }
                break;
            case eSceneKey.WORLD:
                break;
                break;
            default:
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

    public void SetSceneKey(eSceneKey _key)
    {
        currentScene = _key;
    }



    Rect screenRect = new Rect(0f, 0f, 16f, 9f);
    Vector3 positionOffset = new Vector3(0f, 0f,-10f);

    eSceneKey currentScene;

    Transform targetPos;
    float moveRange = 0.5f;
    float horizontalRatio;

}
