using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDeath : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem blood;

    [SerializeField]
    private float spreadAngle;

    [SerializeField]
    private Vector2 throwForce;

    private HumansList humans;

    private void Awake()
    {
        humans = FindObjectOfType<HumansList>();   
    }

    [SerializeField]
    private List<GameObject> BodyParts;

    public void Die(GameObject bot)
    {
        var deathDir = (transform.position - bot.transform.position).normalized;
        humans.CheckWin();
        foreach (var p in BodyParts)
        {
            var dir = Quaternion.AngleAxis(Random.Range(-spreadAngle, spreadAngle) / 2, Vector3.forward) *
                deathDir * Random.Range(throwForce.x, throwForce.y);
            SpawnSinglePart(p, dir);
        }
        Instantiate(blood, transform.position, Quaternion.LookRotation(Vector3.forward, deathDir));
    }

    private void SpawnSinglePart(GameObject partObj, Vector2 direction)
    {
        var part = Instantiate(partObj, transform.position, Quaternion.identity);
        
        var rb = part.GetComponent<Rigidbody2D>();
        rb.AddForce(direction, ForceMode2D.Impulse);
        part.GetComponent<SpawnableBodyPart>().Init();
        part.GetComponent<HumanPartPointOfInterest>().Init();
    }
}
