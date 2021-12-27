using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scene : MonoBehaviour
{
    protected void Awake()
    {
        camera = FindObjectOfType<CameraController>();
        audioSource = GetComponent<AudioSource>();
        bLoad = false;
    }

    protected void OnDisable()
    {
        bLoad = true;
    }

    // Getter & Setter
    public virtual eSceneKey SceneKey { get; }
    public virtual Rect Boundary { get; }
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }



    // ** Field
    protected eSceneKey sceneKey;
    protected Rect boundary;

    protected CameraController camera;
    protected AudioSource audioSource;
    protected bool bLoad;
}
