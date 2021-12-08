using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Awake()
    {
        playerPosition = FindObjectOfType<PlayerController>().transform;
        offsetX = transform.position.x - playerPosition.position.x;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(offsetX + playerPosition.position.x, transform.position.y, transform.position.z);
    }

    Transform playerPosition;
    float offsetX;
}
