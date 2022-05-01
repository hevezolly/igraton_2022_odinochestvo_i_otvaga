using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckConfusionBehaviour : EmotionReaction, IDestinationProvider
{
    public DestinationType DestinationType => DestinationType.Sight;

    private Vector2 confusionPlace;


    [SerializeField]
    private HumanDestinationSetter destination;
    [SerializeField]
    private HumanInterest interest;

    public Vector2 GetDestination()
    {
        return confusionPlace;
    }

    public bool OnWayPointReached()
    {
        interest.SetPointOfInterest(confusionPlace);
        return true;
    }

    protected override void React(Vector2 position)
    {
        Debug.DrawLine(transform.position, position, Color.red, 10);
        interest.RemovePointOfInterest();
        confusionPlace = position;
        destination.SetDestinationProvider(this);
    }
}
