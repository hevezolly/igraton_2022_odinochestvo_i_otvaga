using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestinationProvider
{
    // Start is called before the first frame update
    Vector2 GetDestination();
    bool OnWayPointReached();

    DestinationType DestinationType { get; }
}

public enum DestinationType
{
    Distance,
    Sight
}


