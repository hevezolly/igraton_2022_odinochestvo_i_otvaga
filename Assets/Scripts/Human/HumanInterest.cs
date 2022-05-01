using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInterest : MonoBehaviour
{
    [SerializeField]
    private AIPath ai;
    [SerializeField]
    private HumanDestinationSetter destinationProvider;
    

    private bool hasInterest = false;

    private Vector2 interestPosition;
    private Transform interestObject;

    private IDestinationProvider savedDestination;
    private bool savedRotationState;

    private float currentangularVelocity;

    private bool useAngularVel = false;
    public void SetPointOfInterest(Transform point, float speed = -1)
    {
        interestObject = point;
        hasInterest = true;
        useAngularVel = speed > 0;
        savedRotationState = ai.enableRotation;
        currentangularVelocity = speed;
        ai.enableRotation = false;
        savedDestination = destinationProvider.destination;
        destinationProvider.SetDestinationProvider(null);
    }

    public void SetPointOfInterest(Vector2 position, float speed = -1)
    {
        interestPosition = position;
        savedRotationState = ai.enableRotation;
        interestObject = null;
        useAngularVel = speed > 0;
        currentangularVelocity = speed;
        hasInterest = true;
        ai.enableRotation = false;
        savedDestination = destinationProvider.destination;
        destinationProvider.SetDestinationProvider(null);
    }

    public void RemovePointOfInterest()
    {
        if (!hasInterest)
            return;
        interestObject = null;
        hasInterest = false;
        ai.enableRotation = savedRotationState;
        destinationProvider.SetDestinationProvider(savedDestination);
    }

    private Vector3 focusPosition => (interestObject != null) ? interestObject.position : (Vector3)interestPosition;

    private void Update()
    {
        if (!hasInterest)
            return;
        var rot = Quaternion.LookRotation(Vector3.forward, focusPosition - transform.position);
        if (useAngularVel)
            rot = Quaternion.RotateTowards(transform.rotation, rot, currentangularVelocity * Time.deltaTime);
        transform.rotation = rot;
    }
}
