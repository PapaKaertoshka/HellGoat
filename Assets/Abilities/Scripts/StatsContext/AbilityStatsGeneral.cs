using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="AbilityStatsGeneral", menuName = "Abilities/AbilityStatsGeneral")]
public class AbilityContextGeneral : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float activeTime;
    [SerializeField] private Sprite _icon;
    
    public string Name => name;
    
    public float CooldownTime=>cooldownTime;

    public float ActiveTime=>activeTime;

    public Sprite Icon => _icon;
}