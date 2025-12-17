using SkyBeneathDemo.Input;
using UnityEngine;

namespace SkyBeneathDemo
{
    public class PlayerGravityManipulator : MonoBehaviour
    {
        [SerializeField] Transform m_hologramPivot;
        private InputManager m_inputManager;
        private Vector2Int m_lastGravityDirection = Vector2Int.zero;
        private Rigidbody m_selfRB;

        private void Start()
        {
            m_inputManager = GetComponent<InputManager>();
            m_selfRB = GetComponent<Rigidbody>();
        }

        public void HandleGravityInput()
        {
            if (!m_inputManager) return;
            if (m_inputManager.IsAnyGravityKeyHeld)
            {
                var direction = m_inputManager.GravityManipulationInput;

                m_hologramPivot.localRotation = Quaternion.Euler(-direction.y * 90f, 0f, direction.x * 90f);
                m_hologramPivot.gameObject.SetActive(true);
                m_lastGravityDirection = direction;
            }
            else if (m_hologramPivot.gameObject.activeSelf)
            {
                //m_selfRB.isKinematic = true;
                Vector3 worldDirection = transform.TransformDirection(new Vector3(m_lastGravityDirection.x, 0f, m_lastGravityDirection.y));
                worldDirection.Normalize();
                transform.up = -worldDirection;
                Physics.gravity = new Vector3(worldDirection.x * 9.81f, 0f, worldDirection.y * 9.81f);
                m_hologramPivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
                m_hologramPivot.gameObject.SetActive(false);
            }
        }

    }
}
