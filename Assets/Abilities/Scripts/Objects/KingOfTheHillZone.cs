using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KingOfTheHillZone : MonoBehaviour
{

    public  event Action onInsideZone;
    public event Action onOutsideZone;
    
    private void OnTriggerEnter(Collider other)
    {
        onInsideZone?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        onOutsideZone?.Invoke();
    }
}