using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        #region SphereCast

        [SerializeField] private Transform HitPoint;
        [SerializeField] private LayerMask enemyLayerMask;
        private RaycastHit[] hits = new RaycastHit[3];

        #endregion
    
        public event Action<GameObject> OnAttack; //для эффектов атаки на противнике

        [SerializeField] private float _attackDamage;

        [SerializeField] private InputActionReference _attack;
        [SerializeField] private InputActionReference _mousePos;

        private Animator _animator;
    
        [SerializeField] private Camera _camera ;

    
        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _attack.action.performed += OnPlayerAttackPerformed;
        }
        void Start()
        {
            _camera = Camera.main;
        }
    
        void Update()
        {
        }

        void OnPlayerAttackPerformed(InputAction.CallbackContext context)
        {
            //_animator.SetTrigger("AttackTrigger");
            
            var rayHit = _camera.ScreenPointToRay((Vector3)_mousePos.action.ReadValue<Vector2>());
            Physics.Raycast(rayHit, out var hit);
            var point = hit.point;
            point.y = transform.position.y;
            transform.forward = (point-transform.position).normalized;
            
        }

    
        public void HitMeleeAttack()
        {
            var size = Physics.SphereCastNonAlloc(HitPoint.position, 0.15f, 
                HitPoint.forward, hits, 0f, enemyLayerMask);
        
            if (size > 0)
            {
                // Health healthScript= hits[0].transform.gameObject.GetComponent<Health>();
                // if (healthScript != null)
                // {
                //     OnAttack?.Invoke(hits[0].transform.gameObject);
                //     
                //     healthScript.TakeDamage(_attackDamage);
                // }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(HitPoint.position, 0.15f);
        }

        public void SetAttackDamage(float attackDamage)
        {
            _attackDamage = attackDamage;
        }

        public float GetAttackDamage()
        {
            return _attackDamage;
        }
    }
}

