using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private AudioEffect effect;

    [SerializeField]
    private List<AudioEffect> otherEffects;
    public void Play()
    {
        if (effect == null)
            return;
        effect.PlayAt(transform.position);
        foreach (var e in otherEffects)
        {
            e.PlayAt(transform.position);
        }
    }
}
