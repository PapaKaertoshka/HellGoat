using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class Vasiliy : MonoBehaviour
{
    [Header("Base logic setup")]
    [SerializeField, Range(0f, 10f)] protected float speed;
    [SerializeField, Range(0f, 50f)] protected float reactionRadius;
    [SerializeField, Range(0f, 10f)] protected float attackRadius;
    [Header("Editor UI")]
    [SerializeField] private Color gizmoColor;
    [SerializeField] private int healtPonts;
    [SerializeField] public Animator anim;

    [SerializeField] protected PlayerMovement playerInstance;
    [SerializeField] protected int damage;
    public bool isAttacking = true;
    [SerializeField] private SphereCollider attackCollider;

    private bool animIsPlaying = false;

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, reactionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }


    private void Attack() {
        if (isAttacking == true)
        {
            StartCoroutine(Attacking());
            Debug.Log("Attacking!");
        }
    }

    private void Chase(float distance)
    {
        Vector3 direction = playerInstance.transform.position - transform.position;
        if (animIsPlaying) return;
        if (distance < attackRadius)
        {
            transform.LookAt(playerInstance.transform);
            anim.SetBool("Walking", false);
            Attack();
        }
        else
        {
            anim.SetBool("Walking", true);
            //anim.SetBool("Attack", false);
            transform.LookAt(playerInstance.transform.position);
            transform.position += direction.normalized * speed * Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        healtPonts -= damage;
        if (healtPonts <= 0)
        {
            StartCoroutine(Death());
        }
    }

    protected void Update()
    {
        InputAction.CallbackContext input;

        float distance = Vector3.Distance(transform.position, playerInstance.transform.position);
        if (distance < reactionRadius)
            Chase(distance);

    }
    IEnumerator Attacking()
    {
        isAttacking = false;
        attackCollider.enabled = true;
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(2.5f);
       // anim.SetBool("Attack", false);
        attackCollider.enabled = false;
        isAttacking = true;
    }
    IEnumerator Death()
    {
        anim.SetBool("Dying",true);
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(this);
    }
}
