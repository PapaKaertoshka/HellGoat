using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private float _delay;
    
    [SerializeField] private List<GameObject> _healthUI= new List<GameObject>();

    private int _currentHealth = 20;
    
    public void SetHealth(int health)
    {
        if (_currentHealth < health)
        {
            StartCoroutine(AddHealth(_currentHealth + health));
        }
        else if (_currentHealth > health)
        {
            StartCoroutine(TakeDamage(_currentHealth - health));

        }
    }

    private IEnumerator TakeDamage(int damage)
    {
        for (int i = 0; i < damage; ++i)
        {
            _healthUI[ (int)Math.Ceiling((decimal)_currentHealth / 2) - 1 ].
                transform.GetChild( _currentHealth % 2 ).
                gameObject.SetActive(false);

            --_currentHealth;
            
            yield return new WaitForSeconds(_delay);
        }
    }

    IEnumerator AddHealth(int heal)
    {
        for (int i = 0; i < heal; ++i)
        {
            _healthUI[ (int)Math.Ceiling((decimal)_currentHealth / 2) - 1 ].
                transform.GetChild( _currentHealth % 2 ).
                gameObject.SetActive(true);

            ++_currentHealth;
            
            yield return new WaitForSeconds(_delay);
        }
    }

    void OnDisable()
    {
        Debug.Log("was disabled");
    }
 
}