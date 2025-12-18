using SkyBeneathDemo.Input;
using UnityEngine;

namespace SkyBeneathDemo
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] InputManager m_inputManager;
        [SerializeField] Transform m_followTarget;
        [SerializeField] float m_rotationSpeed = 150f; // Increased for deltaTime usage
        [SerializeField] float m_minTiltAngle = -80f; // Wider range usually feels better
        [SerializeField] float m_maxTiltAngle = 80f;

        private float m_horizontalViewAngle = 0f;
        private float m_tiltAngle = 0f;

        public void HandleLook()
        {

            m_followTarget.position = transform.position;

            if (m_inputManager.IsAnyGravityKeyHeld) return;
            m_horizontalViewAngle += m_inputManager.LookInput.x * m_rotationSpeed * Time.deltaTime;
            m_tiltAngle -= m_inputManager.LookInput.y * m_rotationSpeed * Time.deltaTime;
            m_tiltAngle = Mathf.Clamp(m_tiltAngle, m_minTiltAngle, m_maxTiltAngle);


            Quaternion targetLocalRotation = Quaternion.Euler(m_tiltAngle, m_horizontalViewAngle, 0f);


            Quaternion gravityAlignment = Quaternion.FromToRotation(Vector3.up, transform.up);


            m_followTarget.rotation = gravityAlignment * targetLocalRotation;
        }

        public void AlignToGravity(Vector3 newUpDirection)
        {
            // Rotate the follow target so its Up aligns with the new gravity
            // preserving its current forward facing direction as much as possible
            Quaternion targetRotation = Quaternion.FromToRotation(m_followTarget.up, newUpDirection) * m_followTarget.rotation;
            m_followTarget.rotation = targetRotation;
        }

        public Transform GetFollowTarget()
        {
            return m_followTarget;
        }
    }
}