using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumansList : MonoBehaviour
{

    [SerializeField]
    private GameObject robot;
    public IEnumerable<HumanAttention> humans 
    { 
        get
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                if (!transform.GetChild(i).gameObject.activeSelf)
                    continue;
                var h = transform.GetChild(i).GetComponentInChildren<HumanAttention>();
                if (h != null)
                {
                    yield return h;
                }
            }
        } 
    }

    public void RetranslateAnger(Vector2 pos)
    {
        foreach (var h in humans)
        {
            h.GetComponent<HumanReaction>().OnTrigger(pos);
        }
    }

    public void CheckWin()
    {
        if (humans.Count() == 1)
        {
            GameObject.FindObjectOfType<GameFinish>().OnWin();
        }
    }

    public void AddPointOfInterest(IAttentionPoint point)
    {
        foreach (var h in humans)
        {
            h.AddAttentionPoint(point);
        }
    }

    public void RemovePointOfInterest(IAttentionPoint point)
    {
        foreach (var h in humans)
        {
            h.RemoveAttentionPoint(point);
        }
    }

    public void OnDash()
    {
        foreach (var h in humans)
        {
            h.TriggerDash(robot.transform.position);
        }
    }
}
