using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weapons = new List<GameObject>();

    private int _currentId;

    void Awake()
    {
        _currentId = PlayerPrefs.GetInt("WeaponIndex", 0);
        foreach (var weapon in _weapons)
        {
            weapon.SetActive(false);
        }

        _weapons[_currentId].SetActive(true);
    }
    
    public void SetActiveWeapon(int newIndex)
    {
        _weapons[_currentId].SetActive(false);
        _currentId = newIndex;
        _weapons[_currentId].SetActive(true);
    }

    public int GetWeaponCurrentId()
    {
        return _currentId;
    }
}
