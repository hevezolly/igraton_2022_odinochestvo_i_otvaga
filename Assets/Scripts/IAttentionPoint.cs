using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttentionPoint
{
    public GameObject gameObject { get; }

    public bool IsVitalPoint { get; }
}

public static class AttentionExtention
{
    public static Vector2 GetPosition(this IAttentionPoint p) => p.gameObject.transform.position;
}
