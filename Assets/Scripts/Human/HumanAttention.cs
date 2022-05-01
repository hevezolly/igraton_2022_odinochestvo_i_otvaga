using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class HumanAttention : MonoBehaviour
{
    [SerializeField]
    private float attentionDistance;

    [SerializeField]
    private float sightDistance;

    [SerializeField]
    private float attentionAngle;

    [SerializeField]
    private LayerMask attentionPointsMask;
    [SerializeField]
    private LayerMask obstickleLayerMask;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Light2D fowLight;

    private HashSet<IAttentionPoint> attentionPoints;

    public PositionEvent ConfussionEvent;
    public PositionEvent CaptureEvent;

    private void AdjustLight()
    {
        if (fowLight == null)
            return;
        fowLight.pointLightOuterRadius = sightDistance;
        fowLight.pointLightOuterAngle = attentionAngle;
        fowLight.pointLightInnerAngle = attentionAngle;
    }

    private void OnValidate()
    {
        AdjustLight();
    }

    private void Awake()
    {
        AdjustLight();
        attentionPoints = new HashSet<IAttentionPoint>();
    }


    public void TriggerDash(Vector2 position)
    {
        if (CanHearAt(position))
        {
            ConfussionEvent?.Invoke(position);
        }
    }

    public bool CanHearAt(Vector2 position)
    {
        return Vector2.Distance(position, transform.position) < attentionDistance;
    }

    public bool CanSee(GameObject obj)
    {
        var position = obj.transform.position;
        var direction = position - transform.position;
        var result = Physics2D.Raycast(transform.position, direction, sightDistance, attentionPointsMask);
        return result.collider != null && result.collider.gameObject == obj && (Vector2.Angle(direction, transform.up) < attentionAngle / 2);
    }

    public bool CanSee(Vector2 position)
    {
        if (Vector2.Distance(position, transform.position) > sightDistance / 2)
            return false;
        var direction = (Vector3)position - transform.position;
        if (Vector2.Angle(direction, transform.up) > attentionAngle / 2)
            return false;
        var result = Physics2D.Raycast(transform.position, direction, direction.magnitude, obstickleLayerMask);
        return result.collider == null;
    }

    public void AddAttentionPoint(IAttentionPoint point)
    {
        attentionPoints.Add(point);
    }

    public void RemoveAttentionPoint(IAttentionPoint point)
    {
        if (attentionPoints.Contains(point))
            attentionPoints.Remove(point);
    }

    private void Update()
    {
        var toRemove = new List<IAttentionPoint>();
        foreach (var point in attentionPoints)
        {
            if (CanSee(point.gameObject))
            {
                if (point.IsVitalPoint)
                {
                    CaptureEvent?.Invoke(point.GetPosition());
                }
                else
                {
                    ConfussionEvent?.Invoke(point.GetPosition());
                }
                toRemove.Add(point);
            }
        }
        foreach (var p in toRemove)
        {
            attentionPoints.Remove(p);
        }
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attentionDistance);
    }
}
