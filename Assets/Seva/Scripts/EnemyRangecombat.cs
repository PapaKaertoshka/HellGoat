using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangecombat : Enemy
{
    [Header("Range combat settings")]
    [SerializeField] private GameObject orb;
    [SerializeField, Range(0f, 10f)] private float attackRateSec;
    [SerializeField, Range(0f, 10f)] private float attackForce;
    private float timer = 0f;

    protected override void Attack()
    {
        timer += Time.deltaTime;
        if (timer >= attackRateSec)
        {
            timer = 0f;

            Rigidbody rb = Instantiate(orb, transform.position, transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(rb.transform.forward * attackForce, ForceMode.Impulse);
        }
    }
}
