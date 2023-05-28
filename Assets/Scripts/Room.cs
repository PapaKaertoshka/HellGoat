using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Room : MonoBehaviour
{
    public event Action OnRoomCompleted;

    [SerializeField] private Transform _spawnPlayerPoint;
    public Transform GetSpawnPlayerPoint()
    {
        return _spawnPlayerPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnRoomCompleted?.Invoke();
    }
}
