using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{
    private void Awake()
    {
        direction = 1f;
        speed = 12f;
        damage = 2;
    }

    private void OnEnable()
    {
        SetBoundry();
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * forward;
        if (!boundary.Contains(transform.position))
            ObjectManager.Instance.RecallObject(gameObject);
    }




    // ** self-defined
    // Boundary ¼³Á¤
    private void SetBoundry()
    {
        boundary = SceneManager.Instance.CurrentScene.Boundary;
        boundary.xMin += -1f;
        boundary.xMax += 1f;
        boundary.yMin += -1f;
        boundary.yMax += 1f;
    }

    // ** Getter & Setter
    public override eObjectKey ObjectKey => eObjectKey.NORMAL_BULLET;
}
