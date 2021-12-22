using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Title : Scene
{
    private void Awake()
    {
        promptText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        ObjectManager.Instance.NewObject(eObjectKey.SCENE_CHANGE_OPEN);
        camera = FindObjectOfType<CameraController>();
        camera.SetPositionOffset(positionOffset);
        camera.CameraState = CameraController.eCameraState.STAY;
        bInput = true;
        promptText.enabled = false;
        StartCoroutine(Blink());
    }

    private void Update()
    {
        if (!bInput)
            return;

        if (Input.anyKey)
        {
            bInput = false;
            SceneManager.Instance.SetScene(eSceneKey.HOUSE);
        }
    }

    IEnumerator Blink()
    {
        while(true)
        {
            yield return new WaitForSeconds(blinkDelay);
            promptText.enabled ^= true;
        }
    }


    public override eSceneKey SceneKey => eSceneKey.TITLE;
    Vector3 positionOffset = new Vector3(0f, 0f, -2f);
    bool bInput;

    // prompt text ฐüทร
    TextMeshProUGUI promptText;
    float blinkDelay = 0.5f;
    float startDelay = 1f;
}
