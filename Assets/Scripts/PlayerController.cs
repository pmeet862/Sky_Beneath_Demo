using SkyBeneathDemo.Input;
using UnityEngine;

namespace SkyBeneathDemo
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerMovement m_playerMovement;
        [SerializeField] PlayerGravityManipulator m_playerGravityManipulator;
        [SerializeField] PlayerAnimationManager m_playerAnimationManager;

        private InputManager m_inputManager;
        private void Start()
        {
            m_inputManager = GetComponent<InputManager>();
        }

        private void Update()
        {
            m_playerGravityManipulator?.HandleGravityInput();
            float moveAmount = Mathf.Clamp01(Mathf.Abs(m_inputManager.MoveInput.x) + Mathf.Abs(m_inputManager.MoveInput.y));
            m_playerAnimationManager?.UpdateAnimatorParameters(0f, moveAmount);
        }

        private void FixedUpdate()
        {
            m_playerMovement?.HandleAllMovement();
        }
    }
}
