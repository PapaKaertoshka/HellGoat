using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Base logic setup")]
    [SerializeField, Range(0f, 10f)] protected float speed;
    [SerializeField, Range(0f, 10f)] protected float reactionRadius;
    [SerializeField, Range(0f, 10f)] protected float attackRadius;
    [Header("Editor UI")]
    [SerializeField] private Color gizmoColor;
    
    protected static Player playerInstance;

    private void Start()
    {
        if (!playerInstance)
            playerInstance = GameObject.FindObjectOfType<Player>();
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
            Attack();
        }
        else
        {
            transform.LookAt(playerInstance.transform.position);
            transform.position += direction.normalized * speed * Time.deltaTime;
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
}
