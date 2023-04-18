using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="KingOfTheHillStats", menuName = "Abilities/KingOfTheHillStats")]
public class KingOfTheHillStats : AbilityContextGeneral
{
    [SerializeField] private GameObject _zone;
    [SerializeField] private GameObject _banner;
    [SerializeField] private int _tickCount;
    [SerializeField] private float _waitTime;
    
    [SerializeField] private float _regeneration;
    [SerializeField] private float _additionalDamage;
    
    [SerializeField] private float _zoneRadius;
    
    
    public GameObject Zone => _zone;
    public GameObject Banner => _banner;
    public int TickCount => _tickCount;
    public float WaitTime => _waitTime;
    public float Regeneration => _regeneration;
    public float AdditionalDamage => _additionalDamage;
    public float ZoneRadius => _zoneRadius;
    
}