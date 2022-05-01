using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanFollow : MonoBehaviour
{
    private Transform target;
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            transform.position = target.position;
        else
            Destroy(gameObject);
    }
}
