using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HumanPath : MonoBehaviour, IDestinationProvider
{

    [SerializeField]
    private float pathCompleteWaitTime;

    private int updateValue = 1;
    private int currentPosition = 0;

    private bool reachedWayPoint;

    private Sequence seq;

    public DestinationType DestinationType => DestinationType.Distance;

    // Start is called before the first frame update
    public Vector2 GetDestination()
    {
        if (transform.childCount < 2)
            return transform.position;
        return transform.GetChild(currentPosition).position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (var i = 1; i < transform.childCount; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i - 1).position, transform.GetChild(i).position);
        }
    }

    bool IDestinationProvider.OnWayPointReached()
    {
        if (reachedWayPoint)
            return true;
        if (currentPosition == 0 && updateValue < 0 || currentPosition == transform.childCount - 1 && updateValue > 0)
        {
            updateValue = -updateValue;
            reachedWayPoint = true;
            seq = DOTween.Sequence()
                .AppendInterval(pathCompleteWaitTime)
                .AppendCallback(() =>
                {
                    currentPosition += updateValue;
                    reachedWayPoint = false;
                });
        }
        else
        {
            currentPosition += updateValue;
        }
        return true;
    }
}
