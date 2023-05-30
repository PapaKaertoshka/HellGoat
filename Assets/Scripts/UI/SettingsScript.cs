using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;

    public void ToMainMenu()
    {
        _mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
