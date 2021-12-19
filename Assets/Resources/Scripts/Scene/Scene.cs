using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scene : MonoBehaviour
{

    // Getter & Setter
    public virtual eSceneKey SceneKey { get; }
    public Vector2 TopLeftBound => topLeftBound;
    public Vector2 BottomRightBound => bottomRightBound;




    // ** Field
    protected eSceneKey sceneKey;
    protected Vector2 topLeftBound;
    protected Vector2 bottomRightBound;
}
