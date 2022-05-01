using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerRetranslator : EmotionReaction
{
    private HumansList humans;

    private void Awake()
    {
        humans = FindObjectOfType<HumansList>();
    }

    protected override void React(Vector2 position)
    {
        humans.RetranslateAnger(position);
    }
}
