using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class dummy : MonoBehaviour
{
    private Animation takeDamageAnim;
    void Awake()
    {
        takeDamageAnim = GetComponent<Animation>();
    }

    public void TakeDamage(float damage)
    {
        takeDamageAnim.Play();
    }

}
