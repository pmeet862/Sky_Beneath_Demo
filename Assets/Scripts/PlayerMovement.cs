using SkyBeneathDemo.Input;
using UnityEngine;

namespace SkyBeneathDemo
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] Transform m_hologramPivot;
        [SerializeField] float m_moveSpeed = 5f;
        [SerializeField] float m_rotationSpeed = 10f;

        private InputManager m_inputManager;
        private Transform m_camTR;
        private Rigidbody m_selfRB;

        private Vector3 m_moveDirection;

        private void Start()
        {
            m_inputManager = GetComponent<InputManager>();
            m_camTR = Camera.main.transform;
            m_selfRB = GetComponent<Rigidbody>();
        }

        public void HandleAllMovement()
        {
            if (!m_inputManager) return;
            SetMovementDirection();
            HandleMovement();
            HandleRotation();
        }

        private void SetMovementDirection()
        {
            // 1. Get Camera vectors
            Vector3 camFwd = m_camTR.forward;
            Vector3 camRight = m_camTR.right;

            // 2. Project them onto the plane defined by the Player's UP vector.
            // This ensures "Forward" is always parallel to the wall/ceiling you are standing on.
            Vector3 fwdProjected = Vector3.ProjectOnPlane(camFwd, transform.up).normalized;
            Vector3 rightProjected = Vector3.ProjectOnPlane(camRight, transform.up).normalized;

            // 3. Calculate direction
            m_moveDirection = fwdProjected * m_inputManager.MoveInput.y + rightProjected * m_inputManager.MoveInput.x;

            // Normalize safely
            if (m_moveDirection.sqrMagnitude > 1f) m_moveDirection.Normalize();
        }

        private void HandleMovement()
        {
            // We cannot just set linearVelocity directly because that wipes out gravity's pull.
            // We need to manipulate velocity ONLY on the plane we are standing on.

            // Get current velocity relative to player orientation
            Vector3 currentVelocity = m_selfRB.linearVelocity;

            // Extract the vertical component (gravity falling speed) relative to player Up
            float verticalSpeed = Vector3.Dot(currentVelocity, transform.up);
            Vector3 gravityVelocity = transform.up * verticalSpeed;

            // Calculate target planar velocity
            Vector3 targetMoveVelocity = m_moveDirection * m_moveSpeed;

            // Combine them: Movement (Planar) + Gravity (Vertical relative to local)
            m_selfRB.linearVelocity = targetMoveVelocity + gravityVelocity;
        }

        private void HandleRotation()
        {
            if (m_moveDirection.sqrMagnitude > 0.01f)
            {
                // Rotate towards move direction, but keep our Head aligned with transform.up
                Quaternion targetRotation = Quaternion.LookRotation(m_moveDirection, transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_rotationSpeed);
            }
        }
    }
}