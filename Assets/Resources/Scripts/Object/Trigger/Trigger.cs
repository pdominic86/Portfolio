using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : Prefab
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Prefab target = collision.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;

        eGroupKey targetKey = target.GroupKey;
        if(targetKey==eGroupKey.PLAYER)
        {
            float y = SceneManager.Instance.CurrentScene.Boundary.yMin;
            y += popupY;
            GameObject obj = ObjectManager.Instance.NewObject(eObjectKey.BOTTON, new Vector3(transform.position.x, y));
            instruct = obj.GetComponent<Popup>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Prefab target = collision.gameObject.GetComponent<Prefab>();
        if (target == null)
            return;

        Debug.Log("call");
        eGroupKey targetKey = target.GroupKey;
        if (targetKey == eGroupKey.PLAYER)
        {
            instruct.Recall();
        }
    }


    public void ToNextScene()
    {
        SceneManager.Instance.SetScene(sceneKey);
    }

    public eSceneKey SceneKey
    {
        get => sceneKey;
        set => sceneKey = value;
    }

    public override eGroupKey GroupKey => eGroupKey.TRIGGER;
    public override eObjectKey ObjectKey => eObjectKey.TRIGGER;


    protected eSceneKey sceneKey;

    float popupY = 2f;
    Popup instruct;
}
