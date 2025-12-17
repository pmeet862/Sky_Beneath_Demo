using SkyBeneathDemo.Input;
using UnityEngine;

namespace SkyBeneathDemo
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] InputManager m_inputManager;
        [SerializeField] Transform m_followTarget;
        [SerializeField] float m_rotationSpeed = 2f;
        [SerializeField] float m_minTiltAngle = -35f;
        [SerializeField] float m_maxTiltAngle = 35f;

        private float m_horizontalViewAngle = 0f;
        private float m_tiltAngle = 0f;


        private void Update()
        {
            m_followTarget.position = transform.position;


            m_horizontalViewAngle += m_inputManager.LookInput.x * m_rotationSpeed * Time.deltaTime;
            m_tiltAngle -= m_inputManager.LookInput.y * m_rotationSpeed * Time.deltaTime;
            m_tiltAngle = Mathf.Clamp(m_tiltAngle, m_minTiltAngle, m_maxTiltAngle);

            Vector3 rotation = Vector3.zero;
            rotation.y = m_horizontalViewAngle;
            rotation.x = m_tiltAngle;
            m_followTarget.rotation = Quaternion.Euler(rotation);
        }
    }
}
