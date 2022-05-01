using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonderAroundTriggeredBehaviour : EmotionReaction, IDestinationProvider
{
    public DestinationType DestinationType => DestinationType.Distance;

    private Vector2 point;

    [SerializeField]
    private float newSpeed;
    [SerializeField]
    private AIPath pathfinder;
    [SerializeField]
    private HumanInterest interest;
    [SerializeField]
    private HumanDestinationSetter destinationSetter;

    public Vector2 GetDestination()
    {
        return point;
    }

    private void RecalculateDestination()
    {
        var grid = AstarPath.active.data.gridGraph;

        var randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];

        point = (Vector3)randomNode.position;
    }

    public bool OnWayPointReached()
    {
        RecalculateDestination();
        return true;
    }

    protected override void React(Vector2 position)
    {
        interest.RemovePointOfInterest();
        RecalculateDestination();
        destinationSetter.SetDestinationProvider(this);
        pathfinder.maxSpeed = newSpeed;
    }
}
