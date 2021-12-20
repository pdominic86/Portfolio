using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scene : MonoBehaviour
{

    // Getter & Setter
    public virtual eSceneKey SceneKey { get; }
    public virtual Rect Boundary { get; }




    // ** Field
    protected eSceneKey sceneKey;
    protected Rect boundary;

    protected CameraController camera;
}
