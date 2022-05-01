using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class VipedChecker : MonoBehaviour
{
    [SerializeField]
    private LayerMask brushMask;


    [SerializeField]
    private Vector2Int resolution;
    [SerializeField]
    private Vector2 gridDimentions;
    [SerializeField]
    private Vector2 gridOffset;

    public UnityEvent VipedEvent;

    private HashSet<Vector2> activePoints;

    private int totalAmount;


    // Start is called before the first frame update
    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        activePoints = new HashSet<Vector2>(GetPoints());
        totalAmount = activePoints.Count;
    }

    public float FillPercent => activePoints.Count / (float)totalAmount;

    private bool IsUnderBrush(Vector2 position)
    {
        return Physics2D.RaycastAll(position, Vector2.zero, 1, brushMask).Length > 0;
    }

    public void TryWipe()
    {
        var removeCandidates = new List<Vector2>();
        foreach (var activePoint in activePoints)
        {
            if (IsUnderBrush(activePoint))
                removeCandidates.Add(activePoint);
        }
        foreach (var candidate in removeCandidates)
        {
            activePoints.Remove(candidate);
        }
        if (activePoints.Count == 0)
            RemoveStain();
    }

    private void RemoveStain()
    {
        VipedEvent?.Invoke();
    }

    private IEnumerable<Vector2> GetPoints()
    {
        var step = new Vector2(gridDimentions.x / (resolution.x - 1),
            gridDimentions.y / (resolution.y - 1));
        var start = gridOffset - gridDimentions / 2;
        for (var x = 0; x < resolution.x; x++)
        {
            for (var y = 0; y < resolution.y; y++)
            {
                var point = start + new Vector2(x * step.x, y * step.y);
                yield return transform.TransformPoint(point);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var p in GetPoints())
        {
            if (Application.isPlaying)
            {
                Gizmos.color = (activePoints.Contains(p) ? Color.yellow : Color.red);
            }
            Gizmos.DrawSphere(p, 0.05f);
        }
    }

    private void OnValidate()
    {
        resolution.x = Mathf.Max(0, resolution.x);
        resolution.y = Mathf.Max(0, resolution.y);
    }
}
