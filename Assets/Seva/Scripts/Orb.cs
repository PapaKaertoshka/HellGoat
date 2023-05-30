using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class Orb : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float damage;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player)
        {
            //player.Damage(damage);

            Destroy(this.gameObject);
        }
    }
}
