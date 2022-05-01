using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class HumanCollisionProcessor : MonoBehaviour
{
    [SerializeField]
    private int patience;
    [SerializeField]
    private float restorePatienceTime;
    [SerializeField]
    private float accumulateAngerTime;

    [SerializeField]
    private float pushPower;
    [SerializeField]
    private float resetDistance;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private HumanInterest interest;
    [SerializeField]
    private float interestTime;
    [SerializeField]
    private HumanDeath death;

    public UnityEvent BotPushEvent;

    private int currentPatience;

    private Coroutine touch;
    private BotMovement bot = null;

    public UnityEvent ExplosionEvent;

    private void Awake()
    {
        currentPatience = patience;
        StartCoroutine(RestorePatience());
    }

    private IEnumerator TouchProcess()
    {
        while (true)
        {
            currentPatience = Mathf.Max(currentPatience - 1, 0);
            if (currentPatience == 0)
            {
                Push();
            }
            yield return new WaitForSeconds(accumulateAngerTime);
        }
    }

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent<BotMovement>(out bot))
            return;
        if (bot.isInDash)
        {
            bot.TerminateDash();
            death.Die(bot.gameObject);
            ExplosionEvent?.Invoke();
            Kill();
        }
        if (touch == null)
            touch = StartCoroutine(TouchProcess());
    }

    private void Push()
    {
        var dir = (bot.transform.position - transform.position).normalized;
        bot.Push(dir * pushPower);
        interest.SetPointOfInterest(bot.transform);
        animator.SetTrigger("kik");
        BotPushEvent?.Invoke();
        DOTween.Sequence().AppendInterval(interestTime).AppendCallback(() => interest.RemovePointOfInterest());
    }

    private void Kill()
    {
        Destroy(transform.parent.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent<BotMovement>(out var bot))
            return;
        if (touch != null) 
        {
            StopCoroutine(touch);
            touch = null;
        }
    }

    private void Update()
    {
        if (bot == null || touch == null)
            return;
        if (Vector2.Distance(bot.transform.position, transform.position) > resetDistance)
        {
            StopCoroutine(touch);
            touch = null;
        }
    }

    private IEnumerator RestorePatience()
    {
        while (true)
        {
            currentPatience = Mathf.Min(currentPatience + 1, patience);
            yield return new WaitForSeconds(restorePatienceTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, resetDistance);
    }
}
