using System;
using UnityEngine;

public class BookOfFlames : Weapon
{
    [SerializeField] private ParticleSystem _particles;
    
    void Awake()
    {
        _damage = 0;
    }
    private void OnEnable()
    {
        base.OnEnable();
        _particles.Play();
    }

    private void OnDisable()
    {
        base.OnDisable();
        _particles.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
 
    }
}
