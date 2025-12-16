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
            m_moveDirection = m_camTR.forward * m_inputManager.MoveInput.y + m_camTR.right * m_inputManager.MoveInput.x;
            m_moveDirection.y = 0f;
            m_moveDirection.Normalize();
        }

        private void HandleMovement()
        {
            Vector3 moveVelocity = m_moveDirection * m_moveSpeed;
            m_selfRB.linearVelocity = new Vector3(moveVelocity.x, m_selfRB.linearVelocity.y, moveVelocity.z);
        }

        private void HandleRotation()
        {
            if (m_moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(m_moveDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_rotationSpeed);
            }
        }

        private void ManageGravityInput()
        {
            if (m_inputManager.IsAnyGravityKeyHeld)
            {
                var direction = m_inputManager.GravityManipulationInput;

                m_hologramPivot.localRotation = Quaternion.Euler(-direction.y * 90f, 0f, direction.x * 90f);
                m_hologramPivot.gameObject.SetActive(true);
            }
            else if (m_hologramPivot.gameObject.activeSelf)
            {
                m_hologramPivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
                m_hologramPivot.gameObject.SetActive(false);
            }
        }
    }
}