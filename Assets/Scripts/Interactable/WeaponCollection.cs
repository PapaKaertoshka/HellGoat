using System;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;
using Random = System.Random;
public class WeaponCollection : Interactable
{
    [SerializeField] private List<GameObject> _weapons = new List<GameObject>();
    
    [FormerlySerializedAs("index")] [SerializeField] private int _index = 0;
    
    [SerializeField] private bool _customIndex = false;

    private PlayerInteract playerInteractScript = null;
    private PlayerWeapon playerWeapon = null;
    
    private void Awake()
    {
        SetActivatedByPlayer(true);
        
        if (!_customIndex)
        {
            Random rnd = new Random();
            _index = rnd.Next(_weapons.Count);
        }
        
        foreach (var weapon in _weapons)
        {
            weapon.SetActive(false);
        }
        
        _weapons[_index].SetActive(true);
    }
    
    public override void Interact()
    {
        base.Interact();

        int lastWeapon = playerWeapon.GetWeaponCurrentId();

        if (lastWeapon == _index) return;
        
        playerWeapon.SetActiveWeapon(_index);

        _weapons[lastWeapon].SetActive(true);
        
        _weapons[_index].SetActive(false);
        _index = lastWeapon;

        if (_index == 0)
        {
            playerInteractScript.OnInteract -= Interact;
            playerInteractScript.SetHintCanvas(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        playerInteractScript = other.GetComponent<PlayerInteract>();
        
        if(playerInteractScript == null || _index == 0) return;

        playerWeapon = other.GetComponent<PlayerWeapon>();

        playerInteractScript.OnInteract += Interact;
        playerInteractScript.SetHintCanvas(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if(playerInteractScript == null) return;

        playerInteractScript.OnInteract -= Interact;
        playerInteractScript.SetHintCanvas(false);
        
        playerInteractScript = null;
        playerWeapon = null;
    }
}
