using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Variables: Movement

        private Vector2 _input;
        private CharacterController _characterController;
        private Vector3 _direction;

        [SerializeField] private float speed = 10;

        #endregion
        #region Variables: Rotation

        [SerializeField] private float smoothTime = 0.05f;
        private float _currentVelocity;

        #endregion
        #region Variables: Gravity

        private float _gravity = -9.81f;
        [SerializeField] private float gravityMultiplier = 3.0f;
        private float _velocity;

        #endregion
        private Animator _animator;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }


        private void Update()
        {
            ApplyGravity();
            ApplyRotation();

            ApplyMovement();
        }

        private void ApplyGravity()
        {
            if (_characterController.isGrounded && _velocity < 0.0f)
            {
                _velocity = -1.0f;
            }
            else
            {
                _velocity += _gravity * gravityMultiplier * Time.deltaTime;
            }

            _direction.y = _velocity;
        }

        private void ApplyRotation()
        {
            if (_input.sqrMagnitude == 0) return;

            var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }

        private void ApplyMovement()
        {
            _characterController.Move(_direction * speed * Time.deltaTime);
        }

        public void Move(InputAction.CallbackContext context)
        {
            _input = context.ReadValue<Vector2>();
            _direction =  Matrix4x4.Rotate(Quaternion.Euler(0, 45,0))
                          * new Vector3(_input.x, 0.0f, _input.y);
        }
    }
}