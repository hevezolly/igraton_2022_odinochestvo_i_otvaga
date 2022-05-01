using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDestinationSetter : MonoBehaviour
{
    [SerializeField]
    private GameObject initialProvider;
    [SerializeField]
    private float destinationReachRadius;
    [SerializeField]
    private HumanAttention attention;
    [SerializeField]
    private AIPath ai;

    public IDestinationProvider destination { get; private set; }
    private IDestinationProvider initialDestination;
    private void Awake()
    {
        if (initialProvider != null)
            destination = initialProvider.GetComponent<IDestinationProvider>();
        initialDestination = destination;
    }

    public void SetDestinationProvider(IDestinationProvider provider)
    {
        ai.canMove = provider != null;
        destination = provider;
    }

    public void SetIdleProvider()
    {
        destination = initialDestination;
        ai.canMove = true;
    }

    private void Update()
    {
        if (destination == null)
        {
            return;
        }
        ai.destination = destination.GetDestination();
        var reached = false;
        switch (destination.DestinationType)
        {
            case DestinationType.Distance:
                reached = IsDestinationReachedApproach();
                break;
            case DestinationType.Sight:
                reached = IsDestinationReachedSight();
                break;
            default:
                throw new System.NotImplementedException();
        }
        if (!reached)
            return;
        if (!destination.OnWayPointReached())
        {
            destination = initialDestination;
        }
    }

    private bool IsDestinationReachedSight()
    {
        return attention.CanSee(ai.destination);
    }

    private bool IsDestinationReachedApproach()
    {
        return Vector2.Distance(transform.position, ai.destination) < destinationReachRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, destinationReachRadius);
    }
}
