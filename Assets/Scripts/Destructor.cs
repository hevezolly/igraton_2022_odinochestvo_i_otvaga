using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Destructor : MonoBehaviour
{
    [SerializeField]
    private GameObject toDestroy;
    [SerializeField]
    private float duration;

    private Sequence s;

    public void DestoryObject()
    {
        if (s != null)
            return;
        s = DOTween.Sequence();
        if (TryGetComponent<SpriteRenderer>(out var renderer))
        {
            s.Append(DOTween.To(() => renderer.color.a,
                (a) => renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, a), 0, duration));
        }
        else
        {
            s.AppendInterval(duration);
        }
        s.AppendCallback(() => Destroy(toDestroy));
    }

    private void OnDestroy()
    {
        s?.Kill();
    }
}
