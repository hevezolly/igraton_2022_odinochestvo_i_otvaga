using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimation : MonoBehaviour
{
    [SerializeField]
    private AIPath pathfinder;
    [SerializeField]
    private Animator animator;

    private float maxSpeed;
    private void Awake()
    {
        maxSpeed = pathfinder.maxSpeed;
    }


    // Update is called once per frame
    void Update()   
    {
        animator.SetBool("isWalking", !Mathf.Approximately(pathfinder.velocity.magnitude, 0) && pathfinder.canMove);
        animator.SetFloat("speed", Vector2.Dot(pathfinder.velocity, transform.up) / maxSpeed);
    }
}
