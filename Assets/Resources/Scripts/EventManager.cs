using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        playerPosition = FindObjectOfType<PlayerController>().transform;
    }

    private void LateUpdate()
    {
        if(!bOccured && playerPosition.position.x>point)
        {
            mapManager.StartEvent(eventRange);
            bOccured = true;
        }
    }




    MapManager mapManager;
    Transform playerPosition;
    float point = 5f;
    Vector2Int eventRange = new Vector2Int(7, 12);
    bool bOccured;
}
