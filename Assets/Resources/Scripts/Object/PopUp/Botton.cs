using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botton : Popup
{
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(Expand());
    }

    public override void Recall()
    {
        StartCoroutine(Reduce());
    }

    IEnumerator Expand()
    {
        Vector3 scale = Vector3.zero;
        while(scale.x<1f)
        {
            float delay = Time.deltaTime;
            yield return new WaitForSeconds(delay);
            delay *= 10;
            scale.x += delay;
            scale.y += delay;
            scale.z += delay;
            transform.localScale = scale;
        }
        transform.localScale = Vector3.one;
    }

    IEnumerator Reduce()
    {
        Vector3 scale = Vector3.one;
        while (scale.x > 0f)
        {
            float delay = Time.deltaTime;
            yield return new WaitForSeconds(delay);
            delay *= 10;
            scale.x -= delay;
            scale.y -= delay;
            scale.z -= delay;
            transform.localScale = scale;
        }
        transform.localScale = Vector3.zero;
        ObjectManager.Instance.RecallObject(gameObject);
    }
    public override eObjectKey ObjectKey => eObjectKey.BOTTON;
}
