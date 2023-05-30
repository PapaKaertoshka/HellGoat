using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private int value = 0;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth ph = other.gameObject.GetComponent<PlayerHealth>();
        if (ph)
        {
            ph.TakeDamage(value);
        }
    }
}
