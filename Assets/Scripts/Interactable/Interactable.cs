using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    private bool ActivatedByPlayer = false;
    public virtual void Interact()
    {
    }

    public bool isActivatedByPlayer()
    {
        return ActivatedByPlayer;
    }

    public void SetActivatedByPlayer(bool predicate)
    {
        ActivatedByPlayer = predicate;
    } 
}