using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkyBeneathDemo.Input
{
    public class InputManager : MonoBehaviour
    {
        private PlayerControls m_playerControls;


        public Vector2 MoveInput;
        public Vector2 LookInput;
        public Vector2Int GravityManipulationInput;
        private readonly List<Vector2Int> m_heldDirections = new();
        private readonly Dictionary<InputAction, Action<InputAction.CallbackContext>> m_performed = new();

        private readonly Dictionary<InputAction, Action<InputAction.CallbackContext>> m_canceled = new();

        public bool IsAnyGravityKeyHeld => m_heldDirections.Count > 0;

        private void Awake()
        {
            m_playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            m_playerControls.Enable();
            m_playerControls.PlayerMovement.Movement.performed += OnMovementPerformed;
            m_playerControls.PlayerMovement.Look.performed += OnLookPerformed;
            Bind(m_playerControls.PlayerMovement.GravityUp, Vector2Int.up);
            Bind(m_playerControls.PlayerMovement.GravityDown, Vector2Int.down);
            Bind(m_playerControls.PlayerMovement.GravityLeft, Vector2Int.left);
            Bind(m_playerControls.PlayerMovement.GravityRight, Vector2Int.right);
        }

        private void OnDisable()
        {
            m_playerControls.PlayerMovement.Movement.performed -= OnMovementPerformed;
            m_playerControls.PlayerMovement.Look.performed -= OnLookPerformed;
            UnbindAll();
            m_playerControls.Disable();
            MoveInput = Vector2.zero;
            LookInput = Vector2.zero;
            GravityManipulationInput = Vector2Int.zero;
            m_heldDirections.Clear();
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        private void OnLookPerformed(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        private void Bind(InputAction action, Vector2Int direction)
        {
            Action<InputAction.CallbackContext> performed = ctx => OnPressed(direction);

            Action<InputAction.CallbackContext> canceled = ctx => OnReleased(direction);

            m_performed[action] = performed;
            m_canceled[action] = canceled;

            action.performed += performed;
            action.canceled += canceled;
        }

        private void UnbindAll()
        {
            foreach (var pair in m_performed)
                pair.Key.performed -= pair.Value;

            foreach (var pair in m_canceled)
                pair.Key.canceled -= pair.Value;

            m_performed.Clear();
            m_canceled.Clear();
        }

        private void OnPressed(Vector2Int direction)
        {
            // Remove if already exists (prevents duplicates)
            m_heldDirections.Remove(direction);

            // Add as most recent
            m_heldDirections.Add(direction);

            GravityManipulationInput = direction;
        }

        private void OnReleased(Vector2Int direction)
        {
            m_heldDirections.Remove(direction);

            // If something still held → use last pressed
            if (m_heldDirections.Count > 0)
            {
                GravityManipulationInput = m_heldDirections[^1]; // last element
            }
            else
            {
                GravityManipulationInput = Vector2Int.zero; // nothing held
            }
        }
    }
}
