using UnityEngine;
using UnityEngine.InputSystem;

public class LampInputScript : MonoBehaviour
{
    private LampController m_LampController;
    private PlayerInput m_PlayerInput;

    [HideInInspector] public Vector2 m_AimInput;

    public bool IsGamepad { get; private set; }

    void Start()
    {
        m_LampController = GetComponent<LampController>();
        m_PlayerInput = m_LampController.m_PlayerController.m_PlayerInputScript.m_PlayerInput;
    }

    void Update()
    {
        m_AimInput = m_PlayerInput.actions["Look"].ReadValue<Vector2>();
    }

    public void OnDeviceChange(PlayerInput playerInput)
    {
        IsGamepad = playerInput.currentControlScheme.Equals("Gamepad");
    }
}
