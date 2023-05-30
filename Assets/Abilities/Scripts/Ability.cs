using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;


public class Ability : MonoBehaviour, IAbility
{
    /*[SerializeField]*/ private AbilityContextGeneral abilityContext;

    private float currentCooldownTime = -1;
    private float currentActiveTime = -1;
    
    protected enum AbilityState
    {
        ready,
        active, 
        cooldown
    }
    
    protected AbilityState state = AbilityState.ready;

    private void Awake()
    {
        Debug.Log("Ability awake");
        abilityContext = (AbilityContextGeneral)Resources.Load("Abilities/Conxtext/AbilityStatsGeneral", typeof(ScriptableObject));
    }

    protected virtual void Update()
    {
        switch (state)
        {
            case AbilityState.ready:
                return;
            case AbilityState.active:
                if(currentActiveTime > 0)
                {
                    currentActiveTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.cooldown;
                    currentActiveTime = abilityContext.ActiveTime;
                    Cooldown();
                }
                break;
            case AbilityState.cooldown:
                if (currentCooldownTime > 0)
                {
                    currentCooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.ready;
                    currentCooldownTime = abilityContext.CooldownTime;
                }
                break;
        }
    }

    public void Execute(GameObject go)
    {
        if (state != AbilityState.ready) return;
        
        currentCooldownTime = abilityContext.CooldownTime;
        currentActiveTime = abilityContext.ActiveTime;

        Activate(go);
    }

    public virtual void Activate(GameObject go)
    {
        state = AbilityState.active;
    }

    public virtual void Cooldown()
    {
        
    }

    public float getCooldownAmount()
    {
        switch (state)
        {
            case AbilityState.active:
                return currentActiveTime / abilityContext.ActiveTime;
            case AbilityState.cooldown:
                return currentCooldownTime / abilityContext.CooldownTime;
        } 
        return 0f;
    }
    
    public Sprite getIcon()
    {
        return abilityContext.Icon;
    }

    public string GetName()
    {
        return abilityContext.Name;
    }

    protected void ConvertContext(ScriptableObject Conxtext)
    {
        abilityContext = (AbilityContextGeneral)Conxtext;
    }
}
