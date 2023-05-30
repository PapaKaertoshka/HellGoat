using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

using Player;
public class AbilityItem : MonoBehaviour
{
    [SerializeField] private bool _customIndex = false;
    [SerializeField] private int index = 0;

    private void Awake()
    {
        if (!_customIndex)
        {
            Random rnd = new Random();
            index = rnd.Next(AbilityDictionary.abilityDictionary.Count);
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out AbilityHolder abilityHolder))
        {
            AbilityDictionary.abilityDictionary.TryGetValue(index, out var ability);
            Debug.Log(ability);
            abilityHolder.setAbility(ability);
            Destroy(gameObject);
        }
    }
}