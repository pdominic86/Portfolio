using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCardOzze : Popup
{
    private void OnEnable()
    {
        bCanInput = false;
        StartCoroutine(CoroutineFunc.DelayOnce(() => { bCanInput = true; }, 0.2f));
    }
    void Update()
    {
        if (!bCanInput)
            return;
        if (Input.GetKeyDown(Keys.KEY_SHOOT))
            SceneManager.Instance.SetScene(eSceneKey.OZZE);
        else if (Input.GetKeyDown(Keys.KEY_JUMP))
        {
            player.SetCanInput(true);
            ObjectManager.Instance.RecallObject(gameObject);
        }
    }

    public override eObjectKey ObjectKey => eObjectKey.STAGE_CARD_OZZE;
    bool bCanInput;
}
