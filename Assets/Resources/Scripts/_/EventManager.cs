using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        playerPosition = FindObjectOfType<PlayerController>().transform;
        eventList = TextParse.To2DList(Resources.Load<TextAsset>("Texts/EventInfo"));
        bOccured = new bool[eventList.Count];
    }

    private void LateUpdate()
    {
        for(int i=0;i< bOccured.Length;++i)
        {
            if(!bOccured[i])
            {
                if ((playerPosition.position.x > eventList[i][0]))
                {
                    mapManager.StartEvent(eventList[i][1], eventList[i][2]);
                    bOccured[i] = true;
                }
                break;
            }

        }
    }




    MapManager mapManager;
    Transform playerPosition;
    List<List<int>> eventList;
    bool[] bOccured;
}
