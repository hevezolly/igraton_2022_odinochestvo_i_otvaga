using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoToExitTrigger : EmotionReaction, IDestinationProvider
{
    public DestinationType DestinationType => DestinationType.Distance;

    public UnityEvent GameFinishEvent;

    [SerializeField]
    private List<Transform> Exits;
    [SerializeField]
    private HumanInterest interest;
    [SerializeField]
    private HumanDestinationSetter destinationSetter;
    [SerializeField]
    private AIPath pathfinder;
    [SerializeField]
    private float newSpeed;

    public Vector2 GetDestination()
    {
        var min = float.MaxValue;
        Transform exit = transform;
        foreach (var e in Exits)
        {
            if (Vector2.Distance(e.position, transform.position) < min)
            {
                exit = e;
                min = Vector2.Distance(e.position, transform.position);
            }
        }
        return exit.position;
    }

    public bool OnWayPointReached()
    {
        GameFinishEvent?.Invoke();
        FindObjectOfType<GameFinish>().OnLoose();
        return true;
    }

    protected override void React(Vector2 position)
    {
        interest.RemovePointOfInterest();
        destinationSetter.SetDestinationProvider(this);
        pathfinder.maxSpeed = newSpeed;
    }

}
