using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Zenject;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float _damage = 0;
    
    [Inject] protected PlayerMovement playerMovementScript;

    protected PlayerAttack _playerAttack;

    virtual public void Awake()
    {
        _playerAttack = playerMovementScript.gameObject.GetComponent<PlayerAttack>();
    }

    virtual public void OnEnable()
    {
        Debug.Log(gameObject.name + " weapon was activated");
        Debug.Log(_damage);
        _playerAttack.SetAttackDamage(_damage);
    }

    virtual public void OnDisable()
    {
        //Debug.Log(gameObject.name + " weapon was deactivated");
    }

    public float GetDamage()
    {
        return _damage;
    }
}