using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Prefab : MonoBehaviour
{
    // ** Unity 제공

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

    // 방향 (-1,1)만을 값으로 가짐
    protected float direction;
    protected Vector2 boundaryX;

    // object 구분을 위한 key
    protected eObjectKey objectKey;
    protected eObjectKey groupKey;

}
