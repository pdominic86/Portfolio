using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private void Awake()
    {
        if (instance != this)
        {
            var objs = FindObjectsOfType<SceneManager>();
            if (objs.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            else
                instance = this;
        }
        DontDestroyOnLoad(gameObject);

        var prefabs = Resources.LoadAll<GameObject>("Prefabs/Scene");
        foreach (var prefab in prefabs)
        {
            eSceneKey sceneKey = prefab.GetComponent<Scene>().SceneKey;
            if (!prefabList.ContainsKey(sceneKey))
            {
                prefabList.Add(sceneKey, prefab);
            }
        }

        UICanvas = Instantiate<Canvas>(Resources.Load<Canvas>("Prefabs/UICanvas"));
        UICanvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        GameObject title = Instantiate<GameObject>(prefabList[eSceneKey.TITLE]);
        sceneList.Add(eSceneKey.TITLE, title);
        currentScene = title.GetComponent<Scene>();
    }


    // ** self
    private void ToNextScene()
    {
        if (!sceneList.ContainsKey(nextSceneKey))
        {
            GameObject scene = Instantiate<GameObject>(prefabList[nextSceneKey]);
            scene.SetActive(false);
            sceneList.Add(nextSceneKey, scene);
        }
        currentScene.gameObject.SetActive(false);
        currentScene = sceneList[nextSceneKey].GetComponent<Scene>();
        currentScene.gameObject.SetActive(true);
        switch (nextSceneKey)
        {
            case eSceneKey.HOUSE:
            case eSceneKey.TUTORIAL:
            case eSceneKey.OZZE:
                StartCoroutine(CoroutineFunc.DelayOnce(() => { UICanvas.gameObject.SetActive(true); }, 1f));
                break;
        }
    }




    public bool SetScene(eSceneKey _key)
    {
        if(prefabList.ContainsKey(_key))
        {
            nextSceneKey = _key;
            ObjectManager.Instance.NewObject(eObjectKey.SCENE_CHANGE_CLOSE);
            StartCoroutine(CoroutineFunc.DelayOnce(ToNextScene, transitionDelay));
            currentScene.GetAudioSource().Stop();
            UICanvas.gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    // ** Getter & Setter
    public Scene CurrentScene => currentScene;

    public static SceneManager Instance
    {
        get
        {
            if (instance==null)
            {
                instance = FindObjectOfType<SceneManager>();
                if (instance == null)
                    instance = new GameObject("SceneManager").AddComponent<SceneManager>();
            }
            return instance;
        }
    }




    // ** ������
    private SceneManager() { }

    // ** Field
    private Dictionary<eSceneKey,GameObject> prefabList=new Dictionary<eSceneKey, GameObject>();
    private Dictionary<eSceneKey,GameObject> sceneList=new Dictionary<eSceneKey, GameObject>();
    private Scene currentScene;
    private eSceneKey nextSceneKey;
    private float transitionDelay = 1f;

    private static SceneManager instance;
    Canvas UICanvas;
    GameUI gameUI;
}
