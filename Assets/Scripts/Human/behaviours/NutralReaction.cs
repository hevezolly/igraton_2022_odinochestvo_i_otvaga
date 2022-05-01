using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutralReaction : EmotionReaction
{
    [SerializeField]
    private HumanInterest interest;
    [SerializeField]
    private HumanDestinationSetter destination;
    [SerializeField]
    private AIPath pathfinder;

    private float maxSpeed;

    private void Awake()
    {
        maxSpeed = pathfinder.maxSpeed;
    }
    protected override void React(Vector2 position)
    {
        interest.RemovePointOfInterest();
        destination.SetIdleProvider();
        pathfinder.maxSpeed = maxSpeed;
    }
}
