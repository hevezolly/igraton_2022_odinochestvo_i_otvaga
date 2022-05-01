using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPartPointOfInterest : MonoBehaviour, IAttentionPoint
{
    [SerializeField]
    private float DistanceToSus;

    [SerializeField]
    private VipedChecker cleanProgress;

    private HumansList humans;
    public bool IsVitalPoint => Vector2.Distance(transform.position, bot.transform.position) <= DistanceToSus && cleanProgress.FillPercent > 0.2f;

    private GameObject bot;

    public void Init()
    {
        bot = FindObjectOfType<BotMovement>().gameObject;
        humans = FindObjectOfType<HumansList>();
        humans.AddPointOfInterest(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DistanceToSus);
    }

    private void OnDestroy()
    {
        if (humans != null)
            humans.RemovePointOfInterest(this);
    }
}
