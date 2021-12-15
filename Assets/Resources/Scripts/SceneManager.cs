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

    }
    private void Start()
    {
        ObjectManager.Instance.NewObject(eObjectKey.GOOPY);
    }



    // ** Getter & Setter
    public Vector2 BoundaryX => boundaryX;



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
    private Vector2 boundaryX = new Vector2(-6f, 6f);


    private static SceneManager instance;
}
