using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scene : MonoBehaviour
{
    protected void Awake()
    {
        camera = FindObjectOfType<CameraController>();
        bLoad = false;
    }

    protected void OnDisable()
    {
        bLoad = true;
    }

    // Getter & Setter
    public virtual eSceneKey SceneKey { get; }
    public virtual Rect Boundary { get; }




    // ** Field
    protected eSceneKey sceneKey;
    protected Rect boundary;

    protected CameraController camera;
    protected bool bLoad;
}
