using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Room : MonoBehaviour
{
    public event Action OnRoomCompleted;

    [SerializeField] private Transform _startPortal;
    [SerializeField] private PortalScript _finishPortal;

    [SerializeField] private List<GameObject> _enemies = new List<GameObject>();

    private void Update()
    {
        if (_enemies.Count == 0)
        {
            _finishPortal.SetAllKilled(true);
        }
    }

    private void Awake()
    {
        _finishPortal.finishPortalTrigger += PortalTriggered;
    }

    private void PortalTriggered()
    {
        OnRoomCompleted?.Invoke();
        _finishPortal.finishPortalTrigger -= PortalTriggered;
    }

    public Transform GetSpawnPlayerPoint()
    {
        return _startPortal;
    }
}
