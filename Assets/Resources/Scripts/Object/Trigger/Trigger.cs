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
        if(targetKey==eGroupKey.PLAYER && instruct==null)
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

        eGroupKey targetKey = target.GroupKey;
        if (targetKey == eGroupKey.PLAYER)
        {
            instruct.Recall();
            instruct = null;
        }
    }

    private void OnEnable()
    {
        instruct = null;
    }


    public void ShowScene(Prefab _target)
    {
        if(SceneManager.Instance.CurrentScene.SceneKey==eSceneKey.WORLD)
        {
            eObjectKey cardKey=0;
            switch(sceneKey)
            {
                case eSceneKey.HOUSE:
                    cardKey = eObjectKey.STAGE_CARD_HOUSE;
                    break;
                case eSceneKey.OZZE:
                    cardKey = eObjectKey.STAGE_CARD_OZZE;
                    break;
            }
            GameObject obj = ObjectManager.Instance.NewObject(cardKey, _target.transform.position);
            Popup popup = obj.GetComponent<Popup>();
            popup.SetPlayer(_target as Player);
        }
        else
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
