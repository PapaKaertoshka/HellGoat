using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClosecombat : Enemy
{
    private Animator enemyAnim;
    private bool isAttacking = true;
    [SerializeField] private SphereCollider attackCollider;
 

    private void Start()
    {
        enemyAnim = GetAnimator();
        attackCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Player.TakeDamage(damage)
    }
    protected override void Attack()
    {
        if (isAttacking)
        {
            StartCoroutine(Attacking());
            Debug.Log("Attacking!");
        }
    }
    IEnumerator Attacking()
    {
        isAttacking = false;
        attackCollider.enabled = true;
        enemyAnim.SetTrigger("Attack");
        yield return new WaitForSeconds(1.6f);
        //enemyAnim.SetBool("Attack", false);
        attackCollider.enabled = false;
        isAttacking = true;
    }
}
