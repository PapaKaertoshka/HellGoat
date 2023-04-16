using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float _maxHealth;
        private float _currentHealth;

        [SerializeField] private HealthBarScript healthBar;
    
        private Animator _animator;
    
        private PlayerInput _playerInput;

    

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
        }
    
        void Start()
        {
            _currentHealth = _maxHealth;
            healthBar.SetMaxhealth(_currentHealth);
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            healthBar.SetHealth(_currentHealth);
            if (_currentHealth > 0)
            {
                _animator.SetTrigger("DamageTrigger");
                StartCoroutine(DeactivateInputForSecondsCoroutine(
                    _animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1));
            }
            else
            {
                _animator.SetTrigger("DieTrigger");
            
                CharacterController characterController = GetComponent<CharacterController>();
                _playerInput.DeactivateInput();
                characterController.enabled = false;

            }
        }

        IEnumerator DeactivateInputForSecondsCoroutine(float animationTime)
        {
            _playerInput.DeactivateInput();
            yield return new WaitForSeconds(animationTime/1.5f);
            _playerInput.ActivateInput();
        }

        public void AddHealth(float health)
        {
            _currentHealth += health;
            if (_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;
            healthBar.SetHealth(_currentHealth);
        }

        public float GetHealth()
        {
            return _currentHealth;
        
        }

        public void SetHealth(float health)
        {
            _currentHealth = health;
            healthBar.SetHealth(_currentHealth);
        }

        public void AddMaxHealth(float health)
        {
            _maxHealth += health;
            _currentHealth += health;
            healthBar.AddMaxHealth(health);
        }

        public void SubstractMaxHealth(float health)
        {
            _maxHealth -= health;
            _currentHealth -= health;
            healthBar.SubstractMaxHealth(health);
        }
    }
}
