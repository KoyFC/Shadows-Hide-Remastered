using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputScript : MonoBehaviour
{
    private PlayerInput m_PlayerInput;

    [HideInInspector] public Vector2 m_Look = Vector2.zero;

    [HideInInspector] public Vector2 m_Movement = Vector2.zero;
    [HideInInspector] public bool m_SprintPressed = false;

    [HideInInspector] public bool m_JumpPressed = false;
    [HideInInspector] public bool m_JumpHeld = false;
    [HideInInspector] public bool m_JumpReleased = false;

    [HideInInspector] public bool m_ActionPressed = false;
    
    [HideInInspector] public float m_MouseWheel = 0f;

    void Awake()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        m_Look = m_PlayerInput.actions["Look"].ReadValue<Vector2>();

        m_PlayerInput.actions["Move"].ReadValue<Vector2>();
        m_SprintPressed = m_PlayerInput.actions["Sprint"].WasPressedThisFrame();

        m_JumpPressed = m_PlayerInput.actions["Jump"].WasPressedThisFrame();
        m_JumpHeld = m_PlayerInput.actions["Jump"].IsPressed();
        m_JumpReleased = m_PlayerInput.actions["Jump"].WasReleasedThisFrame();

        m_ActionPressed = m_PlayerInput.actions["Attack"].WasPressedThisFrame();

        m_MouseWheel = m_PlayerInput.actions["MouseWheel"].ReadValue<float>();
    }
}
