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
        enemyAnim.SetBool("Attack", true);
        yield return new WaitForSeconds(0.8f);
        enemyAnim.SetBool("Attack", false);
        attackCollider.enabled = false;
        isAttacking = true;
    }
}
