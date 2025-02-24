using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputScript : MonoBehaviour
{
    #region Variables
    public PlayerInput m_PlayerInput;

    [SerializeField][Min(0)] private Vector2 m_LeftStickDeadZone;
    [SerializeField][Min(0)] private Vector2 m_RightStickDeadZone;

    [HideInInspector] public Vector2 m_Movement = Vector2.zero;
    [HideInInspector] public bool m_SprintPressed = false;

    [HideInInspector] public bool m_JumpPressed = false;
    [HideInInspector] public bool m_JumpHeld = false;
    [HideInInspector] public bool m_JumpReleased = false;

    [HideInInspector] public bool m_SummonPressed = false;
    [HideInInspector] public bool m_ActionPressed = false;
    
    [HideInInspector] public float m_MouseWheel = 0f;
    #endregion

    #region Main Methods
    void Awake()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        m_Movement = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
        // Setting the deadzone / limits for the left stick
        if (m_Movement.x > 0 && 
            m_Movement.x > m_LeftStickDeadZone.x && // Minimum X value
            m_Movement.y < m_LeftStickDeadZone.y) // Maximum Y value
        {
            m_Movement.x = 1;
        }
        else if (m_Movement.x < 0 && 
            m_Movement.x < -m_LeftStickDeadZone.x && 
            m_Movement.y > -m_LeftStickDeadZone.y)
        {
            m_Movement.x = -1;
        }
        else
        {
            m_Movement.x = 0;
        }

        m_SprintPressed = m_PlayerInput.actions["Sprint"].WasPressedThisFrame();

        m_JumpPressed = m_PlayerInput.actions["Jump"].WasPressedThisFrame();
        m_JumpHeld = m_PlayerInput.actions["Jump"].IsPressed();
        m_JumpReleased = m_PlayerInput.actions["Jump"].WasReleasedThisFrame();

        m_SummonPressed = m_PlayerInput.actions["Summon"].WasPressedThisFrame();
        m_ActionPressed = m_PlayerInput.actions["Attack"].WasPressedThisFrame();

        m_MouseWheel = m_PlayerInput.actions["MouseWheel"].ReadValue<float>();
    }
    #endregion
}
