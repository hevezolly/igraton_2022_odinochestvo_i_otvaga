using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ConfusionStaticReaction : EmotionReaction
{
    [SerializeField]
    private HumanInterest interest;
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float waitTime;
    protected override void React(Vector2 position)
    {
        interest.SetPointOfInterest(position, turnSpeed);
        DOTween.Sequence().AppendInterval(waitTime).AppendCallback(() => interest.RemovePointOfInterest());
    }
}
