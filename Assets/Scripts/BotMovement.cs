using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class BotMovement : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    [Min(0)]
    private float maxSpeed;
    [SerializeField]
    [Min(0)]
    private float maxBackSpeed;
    [SerializeField]
    [Min(0)]
    private float dashSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float angularSpeed;
    [SerializeField]
    private float dashAngularSpeed;
    [SerializeField]
    private float dashAcceleration;

    public UnityEvent MoveEvent;
    public UnityEvent DashEvent;

    private float speed = 0;
    private Vector2 direction = Vector2.up;

    private float currentAcceleration;
    private float currentAngularSpeed;
    private float speedAmount => Mathf.Abs(speed) / maxSpeed;

    public bool isInPush { get; private set; }
    public bool isInDash { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (isInPush)
            return;
        if (speed <= maxSpeed)
        {
            currentAcceleration = acceleration;
            currentAngularSpeed = angularSpeed;
            isInDash = false;
        }
        AdjustSpeed();
        UpdateDirection();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }
    }

    private void Dash()
    {
        speed = dashSpeed;
        DashEvent?.Invoke();
        currentAcceleration = dashAcceleration;
        currentAngularSpeed = dashAngularSpeed;
        isInDash = true;
    }

    public void TerminateDash()
    {
        speed = 0;
    }

    public void Push(Vector2 distance)
    {
        rb.velocity = Vector2.zero;
        speed = 0;
        rb.AddForce(distance, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-100f, 100f), ForceMode2D.Impulse);
        isInPush = true;
        var s = DOTween.Sequence().AppendInterval(1).AppendCallback(() => { 
            isInPush = false;
            direction = transform.up;
            speed = 0;
        });
    }

    private void FixedUpdate()
    {
        if (isInPush)
            return;
        ApplyMovement(direction, speed);
    }

    private void UpdateDirection()
    {
        var targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (Mathf.Approximately(targetDir.magnitude, 0))
            return;
        Debug.DrawRay(transform.position, targetDir);
        var mult = 1;
        var dir = direction;
        //if (Vector2.Dot(targetDir, direction) < -0.9f)
        //{
        //    mult = -1;
        //    dir = -direction;
        //}
        var rotation = Quaternion.RotateTowards(
            Quaternion.LookRotation(Vector3.forward, dir),
            Quaternion.LookRotation(Vector3.forward, targetDir), currentAngularSpeed * speedAmount * Time.deltaTime);
        direction =  rotation * (mult * Vector2.up);
    }

    private void AdjustSpeed()
    {
        var targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        var ms = maxSpeed;

        //if (Vector2.Dot(targetDir, direction) < -0.9f)
        //{
        //    ms = -maxBackSpeed;
        //}
        var target = targetDir.normalized.magnitude * ms;
        var acc = currentAcceleration * Time.deltaTime;
        var delta = target - speed;
        var change = Mathf.Sign(delta) * Mathf.Min(acc, Mathf.Abs(delta));
        speed += change;
    }

    private void ApplyMovement(Vector2 direction, float speed)
    {
        var vel = (Vector3)direction * speed;
        rb.velocity = vel;
        var angleVel = Vector2.SignedAngle(transform.up, direction);
        rb.angularVelocity = angleVel / Time.fixedDeltaTime;
        if (!Mathf.Approximately(vel.magnitude, 0))
            MoveEvent?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction);
    }
}
