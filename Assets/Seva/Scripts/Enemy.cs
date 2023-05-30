using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public abstract class Enemy : MonoBehaviour
{
    [Header("Base logic setup")]
    [SerializeField, Range(0f, 10f)] protected float speed;
    [SerializeField, Range(0f, 50f)] protected float reactionRadius;
    [SerializeField, Range(0f, 10f)] protected float attackRadius;
    [Header("Editor UI")]
    [SerializeField] private Color gizmoColor;
    [SerializeField] private int healtPonts;
    [SerializeField] private Animator anim;

    [SerializeField] protected PlayerMovement playerInstance;

    public Animator GetAnimator()
    {
        return anim;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, reactionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    protected abstract void Attack();   // Ovveride on child

    private void Chase(float distance)
    {
        Vector3 direction = playerInstance.transform.position - transform.position;

        if (distance < attackRadius)
        {
            transform.LookAt(playerInstance.transform);
            anim.SetBool("Run Forward", false);
            Attack();
        }
        else
        {
            anim.SetBool("Run Forward", true);
            transform.LookAt(playerInstance.transform.position);
            transform.position += direction.normalized * speed * Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        healtPonts -= damage;
        if(healtPonts <= 0)
        {
            StartCoroutine(Death());
        }
    }
    private void Wander()
    {
        // ToDo
    }

    protected void Update()
    {
        float distance = Vector3.Distance(transform.position, playerInstance.transform.position);
        if (distance < reactionRadius)
            Chase(distance);
        else Wander();
        
    }
    
    IEnumerator Death()
    {
        anim.SetBool("Die", true);
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(this);
    }
}
