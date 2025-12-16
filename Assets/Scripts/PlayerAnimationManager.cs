using UnityEngine;

namespace SkyBeneathDemo
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        private static readonly int s_horizontalHash = Animator.StringToHash("Horizontal");
        private static readonly int s_verticalHash = Animator.StringToHash("Vertical");

        [SerializeField] Animator m_playerAnimator;

        public void UpdateAnimatorParameters(float horizontal, float vertical)
        {
            m_playerAnimator.SetFloat(s_horizontalHash, horizontal);
            m_playerAnimator.SetFloat(s_verticalHash, vertical);
        }

    }
}
