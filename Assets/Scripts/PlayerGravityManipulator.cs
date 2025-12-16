using SkyBeneathDemo.Input;
using UnityEngine;

namespace SkyBeneathDemo
{
    public class PlayerGravityManipulator : MonoBehaviour
    {
        [SerializeField] Transform m_hologramPivot;
        private InputManager m_inputManager;

        private void Start()
        {
            m_inputManager = GetComponent<InputManager>();
        }

        public void HandleGravityInput()
        {
            if (!m_inputManager) return;
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
