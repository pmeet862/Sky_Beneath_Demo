using SkyBeneathDemo.Input;
using UnityEngine;

namespace SkyBeneathDemo
{
    public class PlayerGravityManipulator : MonoBehaviour
    {
        [SerializeField] Transform m_hologramPivot;
        private CameraManager m_cameraManager;
        private InputManager m_inputManager;
        private Rigidbody m_selfRB;

        private void Start()
        {
            m_inputManager = GetComponent<InputManager>();
            m_cameraManager = GetComponent<CameraManager>();
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
            }
            else if (m_hologramPivot.gameObject.activeSelf)
            {
                Vector3 newUp = m_hologramPivot.up;
                newUp.Normalize();
                m_selfRB.Sleep();


                Quaternion alignRot = Quaternion.FromToRotation(transform.up, newUp) * transform.rotation;
                transform.rotation = alignRot;


                m_cameraManager.AlignToGravity(newUp);

                ChangeGravity(-newUp);

                m_hologramPivot.localRotation = Quaternion.identity;
                m_hologramPivot.gameObject.SetActive(false);
            }
        }

        private async void ChangeGravity(Vector3 gravityDirection)
        {
            await Awaitable.FixedUpdateAsync();
            Physics.gravity = gravityDirection * 9.81f;
            m_selfRB.WakeUp();
        }
    }
}