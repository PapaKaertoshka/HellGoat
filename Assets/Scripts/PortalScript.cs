using System.Collections;
using System.Collections.Generic;
using System;
using Player;
using UnityEngine;
public class PortalScript : MonoBehaviour
{

    [SerializeField] private ParticleSystem _particles;
    [SerializeField]private bool _isFinishPortal = false;
    private bool _isAllKilled;
    public event Action finishPortalTrigger;

    void Awake()
    {
        _particles.Stop();
        _isAllKilled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (!player || !_isAllKilled) return;
        _particles.Play();
        if (_isFinishPortal)
        {
            finishPortalTrigger?.Invoke();
        }
           
    }

    private void OnTriggerExit(Collider other)
    {
        _particles.Stop();
        Debug.Log("left");
    }

    public void SetAllKilled(bool value)
    {
        _isAllKilled = value;
    }
}
