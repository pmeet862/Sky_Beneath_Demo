using SkyBeneathDemo.Input;
using System;
using UnityEngine;

namespace SkyBeneathDemo
{
    public class PlayerController : MonoBehaviour
    {
        public event Action onFallenIntoVoid;
        public event Action onCubeCollected;

        [SerializeField] PlayerMovement m_playerMovement;
        [SerializeField] PlayerGravityManipulator m_playerGravityManipulator;
        [SerializeField] PlayerAnimationManager m_playerAnimationManager;
        [SerializeField] CameraManager m_cameraManager;
        [SerializeField] InputManager m_inputManager;
        [SerializeField] LayerMask m_groundCheckLayerMask;
        [SerializeField] LayerMask m_playerLayerMask;
        [SerializeField] float m_checkForVoidTime;
        [SerializeField] float m_currentFallingTime;

        private float m_groundCheckDistance;
        private float m_groundCheckRadius;
        private bool m_isGrounded;
        private bool m_isFallenIntoVoid;

        private void Start()
        {
            var capsule = GetComponent<CapsuleCollider>();
            m_groundCheckDistance = capsule.height + 0.1f;
            m_groundCheckRadius = capsule.radius;
        }

        private void Update()
        {
            if (m_isFallenIntoVoid)
            {
                return;
            }
            m_isGrounded = IsGrounded();
            if (m_isGrounded) m_playerGravityManipulator?.HandleGravityInput();
            m_cameraManager?.HandleLook();
            float moveAmount = Mathf.Clamp01(Mathf.Abs(m_inputManager.MoveInput.x) + Mathf.Abs(m_inputManager.MoveInput.y));
            m_playerAnimationManager?.UpdateAnimatorParameters(0f, moveAmount, m_isGrounded);
            if (!m_isGrounded)
            {
                m_currentFallingTime += Time.deltaTime;
                if (m_currentFallingTime >= m_checkForVoidTime)
                {
                    if (!Physics.SphereCast(transform.position, m_groundCheckRadius, -transform.up, out _, float.MaxValue, ~m_playerLayerMask))
                    {
                        onFallenIntoVoid?.Invoke();
                        m_isFallenIntoVoid = true;
                        Debug.Log("Fallen into void");
                    }
                    else
                    {
                        m_currentFallingTime = 0f;
                    }
                }
            }
            else
            {
                m_currentFallingTime = 0f;
            }
        }

        private void FixedUpdate()
        {
            if (m_isGrounded)
                m_playerMovement?.HandleAllMovement();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Collectable"))
            {
                Destroy(collision.gameObject);
                onCubeCollected?.Invoke();
            }
        }

        public void SetToIdleAnimation()
        {
            m_playerAnimationManager?.UpdateAnimatorParameters(0f, 0f, true);
        }

        private bool IsGrounded()
        {
            return Physics.SphereCast(transform.position, m_groundCheckRadius, -transform.up, out _, m_groundCheckDistance, m_groundCheckLayerMask);
        }
    }
}
