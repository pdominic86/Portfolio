using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Prefab : MonoBehaviour
{
    // ** Unity ����
    protected void Awake()
    {
        bLoad = false;
    }

    protected void OnDisable()
    {
        bLoad = true;
    }



    // ** self
    public virtual void Hit() { }


    // ** Getter & Setter
    public virtual eObjectKey ObjectKey { get; }
    public virtual eGroupKey GroupKey { get; }




    public float Direction
    {
        get => direction;
        set
        {
            direction = (value < 0f ? -1f : 1f);
            Vector3 scale = transform.localScale;
            if(direction*scale.x<0f)
            {
                scale.x *= -1f;
                transform.localScale = scale;
            }
        }
    }

    public float Angle
    {
        get => angle;
        set
        {
            angle = value;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            float radian = angle * Mathf.Deg2Rad;
            forward = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    }



    // ���� (-1,1)���� ������ ����
    protected float direction;
    protected Rect boundary;

    protected float angle;
    protected Vector3 forward;

    protected bool bLoad;
}
