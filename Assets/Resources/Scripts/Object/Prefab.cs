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
            bool change = false;
            if (direction >= 0f && value < 0f)
            {
                direction = -1f;
                change = true;
            }
            else if (direction <= 0f && value > 0f)
            {
                direction = 1f;
                change = true;
            }
            if (change)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1f;
                transform.localScale = scale;
            }
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

    // object ������ ���� key
    protected eObjectKey objectKey;
    protected eObjectKey groupKey;

}
