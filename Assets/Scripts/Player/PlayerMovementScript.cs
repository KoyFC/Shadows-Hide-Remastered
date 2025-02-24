using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementScript : MonoBehaviour
{
    #region Variables
    private PlayerController m_PlayerController = null;
    private PlayerInputScript m_PlayerInputScript = null;
    private Rigidbody2D m_Rigidbody = null;
    [SerializeField] Transform m_Hinge;

    [Header("Movement")]
    [SerializeField] private float m_MovementSpeed = 5f;
    [SerializeField] private float m_RunSpeedMultiplier = 1.5f;
    private float m_CurrentSpeed = 0f;
    public bool GoingRight { get; private set; }

    [Header("Jump")]
    [SerializeField] private float m_JumpForce = 10f;
    [SerializeField] private int m_MaxExtraJumps = 0;
    private int m_RemainingJumps = 0;

    [SerializeField] private float m_CoyoteTime = 0.5f;
    [SerializeField] private float m_JumpBufferTime = 0.5f;
    private bool m_CoyoteTimeActive = true;
    private bool m_JumpBuffered = false;
    private bool m_HasJumped = false;

    [Header("Damage")]
    [SerializeField] private float m_KnockbackForce = 10f;
    #endregion

    #region Main Methods
    void Start()
    {
        m_PlayerController = GetComponent<PlayerController>();
        m_PlayerInputScript = m_PlayerController.m_PlayerInputScript;

        m_Rigidbody = GetComponent<Rigidbody2D>();

        m_CurrentSpeed = m_MovementSpeed;
        GoingRight = true;
    }

    void Update()
    {
        bool sprintPressed = m_PlayerInputScript.m_SprintPressed;

        if (sprintPressed && m_CurrentSpeed == m_MovementSpeed) // Walking to running
        {
            m_CurrentSpeed = m_MovementSpeed * m_RunSpeedMultiplier;
        }
        else if (sprintPressed) // Running to walking
        {
            m_CurrentSpeed = m_MovementSpeed;
        }

        HandleJump();
        HandleRotation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }
    #endregion

    #region Handling Methods
    private void HandleMovement()
    {
        float movementInput = m_PlayerController.m_PlayerInputScript.m_Movement.x;

        m_Rigidbody.linearVelocity = new Vector2(movementInput * m_CurrentSpeed, m_Rigidbody.linearVelocity.y);
    }

    private void HandleRotation()
    {
        Vector3 currentAngle = m_Hinge.localRotation.eulerAngles;

        if (m_PlayerController.m_LampActive)
        {
            LampInputScript lampInputScript = m_PlayerController.m_LampController.m_LampInputScript;
            Vector3 aimDirection = lampInputScript.m_AimInput;

            if (!lampInputScript.IsGamepad)
            {
                aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            }

            if (aimDirection.x > 0 && !GoingRight) // Turn right
            {
                GoingRight = true;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (aimDirection.x < 0 && GoingRight) // Turn left
            {
                GoingRight = false;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            if (GoingRight && m_PlayerInputScript.m_Movement.x < 0) // Turn left
            {
                GoingRight = false;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (!GoingRight && m_PlayerInputScript.m_Movement.x > 0) // Turn right
            {
                GoingRight = true;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void HandleJump()
    {
        HandleCoyoteTime();
        HandleJumpBuffer();

        if (m_CoyoteTimeActive && m_JumpBuffered && !m_HasJumped)
        {
            m_JumpBuffered = false;
            m_CoyoteTimeActive = false;

            // Clearing the y velocity to avoid taking into account current vertical velocity
            m_Rigidbody.linearVelocity = new Vector2(
                m_Rigidbody.linearVelocity.x,
                0);

            // Adding the jump force: if the jump is being held during the first frame, we jump higher
            if (m_PlayerInputScript.m_JumpHeld)
            {
                m_Rigidbody.AddForce(Vector2.up * m_JumpForce, ForceMode2D.Impulse);
            }
            else // This helps when buffering the jump
            {
                m_Rigidbody.AddForce(0.5f * m_JumpForce * Vector2.up, ForceMode2D.Impulse);
            }
        }

        // Reducing the jump height if the jump button is released
        if (m_PlayerInputScript.m_JumpReleased && m_Rigidbody.linearVelocity.y > 0)
        {
            m_HasJumped = true;

            m_Rigidbody.linearVelocity = new Vector2(
                m_Rigidbody.linearVelocity.x,
                m_Rigidbody.linearVelocity.y * 0.5f);
        }
        else if (m_HasJumped && m_PlayerController.m_GroundScript.IsGrounded)
        {
            m_HasJumped = false;
        }
    }
    #endregion

    #region Helper Methods
    private void HandleCoyoteTime()
    {
        if (m_PlayerController.m_GroundScript.IsGrounded && !m_CoyoteTimeActive)
        {
            StartCoroutine(CoyoteTimeCoroutine());
        }
    }

    private void HandleJumpBuffer()
    {
        if (m_PlayerInputScript.m_JumpPressed && !m_JumpBuffered)
        {
            StartCoroutine(JumpBufferCoroutine());
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator CoyoteTimeCoroutine()
    {
        m_CoyoteTimeActive = true;
        yield return new WaitForSeconds(m_CoyoteTime);
        m_CoyoteTimeActive = false;
    }

    private IEnumerator JumpBufferCoroutine()
    {
        m_JumpBuffered = true;
        yield return new WaitForSeconds(m_JumpBufferTime);
        m_JumpBuffered = false;
    }
    #endregion
}

