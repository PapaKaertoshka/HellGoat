using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;
    
    [SerializeField] private Image abilityImage;
    void Awake()
    {
        cooldownImage.fillAmount= 0;
        abilityImage.enabled = true;
    }

    public void SetAbilitySprite(Sprite sprite)
    {
        abilityImage.enabled = true;
        abilityImage.sprite = sprite;
    }
    
    
    public void SetCooldownAmount(float amount)
    {
        cooldownImage.fillAmount = amount;
    }
}