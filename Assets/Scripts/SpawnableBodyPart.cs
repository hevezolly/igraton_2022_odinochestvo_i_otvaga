using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableBodyPart : MonoBehaviour
{
    [SerializeField]
    private BasicFloor floor;
    [SerializeField]
    private VipedChecker vipe;
    [SerializeField]
    private float speedLimit;

    [SerializeField]
    private Rigidbody2D rb;

    private bool inited = false;
    public void Init()
    {
        inited = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inited && rb.velocity.magnitude < speedLimit)
        {
            rb.velocity = Vector2.zero;
            floor.SetWipable();
            vipe.Init();
            inited = false;
        }
    }
}
