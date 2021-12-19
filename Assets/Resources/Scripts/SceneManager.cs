using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private void Awake()
    {
        if(instance!=this)
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


        var prefabs = Resources.LoadAll<GameObject>("Scene");
        foreach(var prefab in prefabs)
        {
            eSceneKey sceneKey = prefab.GetComponent<Scene>().SceneKey;
            if (!sceneList.ContainsKey(sceneKey))
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                sceneList.Add(sceneKey, obj);
            }
        }
    }

    private void Start()
    {
        GameObject scene = sceneList[eSceneKey.OZZE];
        scene.SetActive(true);
        currentScene = scene.GetComponent<Scene>();
    }



    // ** Getter & Setter
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




    // ** »ý¼ºÀÚ
    private SceneManager() { }

    // ** Field
    private Dictionary<eSceneKey,GameObject> sceneList;
    private Scene currentScene;

    private static SceneManager instance;
}
