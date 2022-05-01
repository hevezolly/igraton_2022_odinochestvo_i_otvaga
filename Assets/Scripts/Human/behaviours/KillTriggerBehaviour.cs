using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class KillTriggerBehaviour : EmotionReaction, IDestinationProvider
{
    public DestinationType DestinationType => DestinationType.Sight;

    [SerializeField]
    private HumanAttention attention;
    [SerializeField]
    private HumanDestinationSetter destination;
    [SerializeField]
    private AIPath pathfinding;
    [SerializeField]
    private HumanInterest interest;
    [SerializeField]
    private BotMovement bot;
    [SerializeField]
    private float speed;
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private float shotTime;
    [SerializeField]
    private float reloadTime;

    public UnityEvent GameOverEvent;

    private bool isTriggered = false;

    [SerializeField]
    private Transform gunPoint;

    private bool canShoot = true;

    public Vector2 GetDestination()
    {
        return bot.transform.position;
    }

    public bool OnWayPointReached()
    {
        return true;
    }

    private void Update()
    {
        if (!isTriggered)
            return;
        if (!attention.CanSee(bot.gameObject))
        {
            return;
        }
        if (canShoot)
        {
            Shoot();
            GameObject.FindObjectOfType<GameFinish>().OnLoose();
            GameOverEvent?.Invoke();
        }
    }

    private void Shoot()
    {
        canShoot = false;
        line.positionCount = 2;
        line.gameObject.SetActive(true);
        line.SetPosition(0, transform.InverseTransformPoint(gunPoint.position));
        line.SetPosition(1, transform.InverseTransformPoint(bot.transform.position));
        DOTween.Sequence()
            .AppendInterval(shotTime).AppendCallback(() => line.gameObject.SetActive(false))
            .AppendInterval(reloadTime).AppendCallback(() => canShoot = true);
    }

    protected override void React(Vector2 position)
    {
        isTriggered = true;
        interest.RemovePointOfInterest();
        pathfinding.maxSpeed = speed;
        destination.SetDestinationProvider(this);
    }
}
