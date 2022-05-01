using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorCleaner : MonoBehaviour
{
    private const string Kernel = "CSMain";
    private const string BrushStrat = "start";
    private const string BrushEnd = "end";
    private const string BrushWidth = "width";
    private const string Texture = "Result";

    [SerializeField]
    private Vector2 start;
    [SerializeField]
    private Vector2 end;
    [SerializeField]
    private float widht;
    [Space]
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private Transform[] samplePoints;
    [SerializeField]
    private ComputeShader shader;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        var providers = GetFloors();
        foreach (var p in providers)
        {
            if (!p.CanBeWiped)
                continue;
            UpdateProvider(p);
        }
    }

    void UpdateProvider(IFloorProvider provider)
    {
        var texture = provider.GetTexture();
        var k = shader.FindKernel(Kernel);

        var rect = provider.GetLocalRect();
        var floorTransform = provider.GetTransform();
        var floorStart = floorTransform.InverseTransformPoint(transform.TransformPoint(start));
        var floorEnd = floorTransform.InverseTransformPoint(transform.TransformPoint(end));
        var tStart = TransformPointByRect(floorStart, rect);
        var tEnd = TransformPointByRect(floorEnd, rect);
        //var clamped = ClampPoint(tStart, tEnd);
        //tStart = clamped.Item1;
        //tEnd = clamped.Item2;
        var tWidht = widht / Mathf.Max(floorTransform.lossyScale.x, floorTransform.lossyScale.y);

        shader.SetVector(BrushStrat, tStart);
        shader.SetVector(BrushEnd, tEnd);
        shader.SetFloat(BrushWidth, tWidht);
        shader.SetTexture(k, Texture, texture);
        shader.Dispatch(k, Mathf.CeilToInt(texture.width / 8f), Mathf.CeilToInt(texture.height / 8f), 1);
        provider.UpdateTexture();
    }

    System.Tuple<Vector2, Vector2> ClampPoint(Vector2 p1, Vector2 p2)
    {
        if (p1.x < 0)
        {
            var t = Mathf.InverseLerp(p1.x, p2.x, 0);
            p1 = new Vector2(0, Mathf.Lerp(p1.y, p2.y, t));
            Debug.Log(1);
        }
        if (p1.x > 1)
        {
            var t = Mathf.InverseLerp(p1.x, p2.x, 1);
            p1 = new Vector2(1, Mathf.Lerp(p2.y, p1.y, t));
        }
        return new System.Tuple<Vector2, Vector2>(p1, p2);
    }
    float invLerp(float from, float to, float value)
    {
        return (value - from) / (to - from);
    }

    private Vector2 TransformPointByRect(Vector2 point, Rect rect)
    {
        return new Vector2(
            invLerp(rect.xMin, rect.xMax, point.x),
            invLerp(rect.yMin, rect.yMax, point.y));
    }

    IEnumerable<IFloorProvider> GetFloors()
    {
        var result = new HashSet<IFloorProvider>();
        foreach (var p in samplePoints.Select(t => t.position))
        {
            var casts = Physics2D.RaycastAll(p, Vector2.zero, 1, mask, -10000, 100000)
                .Select(c => c.collider.GetComponent<IFloorProvider>())
                .Where(p => p != null);
            result.UnionWith(casts);
        }
        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(start, widht);
        Gizmos.DrawWireSphere(end, widht);
        Gizmos.DrawLine(start, end);
    }
}
