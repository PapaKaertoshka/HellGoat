using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    void Execute(GameObject go);

    Sprite getIcon();

    float getCooldownAmount();

    string GetName();
    
    //todo: abilityStatsGeneral getter
}