using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Prefab : MonoBehaviour
{
    // ** Unity ����

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
            if(direction*scale.x<-1f)
            {
                scale.x *= -1f;
                transform.localScale = scale;
            }
        }
    }

    public int Angle
    {
        get => angle;
        set
        {
            angle = value;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }


    public Vector2 BoundaryX
    {
        get => boundaryX;
        set => boundaryX = value;
    }

    // ���� (-1,1)���� ������ ����
    protected float direction;
    protected Vector2 boundaryX;

    protected int angle;

    // object ������ ���� key
    protected eObjectKey objectKey;
    protected eObjectKey groupKey;

}
