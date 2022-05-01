using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EmotionReaction : MonoBehaviour
{
    [SerializeField]
    private EmotionType type;
    

    public void ReactToEmotionChange(EmotionChangeTrigger trigger)
    {
        if (trigger.type == type)
            React(trigger.position);
    }

    protected abstract void React(Vector2 position);
}
