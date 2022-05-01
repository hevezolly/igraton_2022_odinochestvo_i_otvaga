using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Events
{
    [System.Serializable]
    public class AttentionPointEvent: UnityEvent<IAttentionPoint> { }

    [System.Serializable]
    public class PositionEvent: UnityEvent<Vector2> { }

    [System.Serializable]
    public class EmotionEvent: UnityEvent<EmotionChangeTrigger> { }

}

public class EmotionChangeTrigger
{
    public EmotionType type;
    public Vector2 position;
}
