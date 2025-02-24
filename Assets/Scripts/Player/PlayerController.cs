using UnityEngine;

[RequireComponent(typeof(HealthScript))]
[RequireComponent(typeof(PlayerMovementScript))]
[RequireComponent(typeof(PlayerInputScript))]
[RequireComponent(typeof(GroundScript))]
public class PlayerController : MonoBehaviour
{
    public PlayerMovementScript m_PlayerMovementScript;
    public PlayerInputScript m_PlayerInputScript;
    internal LampController m_LampController;
    internal GroundScript m_GroundScript;

    void Awake()
    {
        GetAllComponents();
    }

    private void GetAllComponents()
    {
        m_PlayerMovementScript = GetComponent<PlayerMovementScript>();
        m_PlayerInputScript = GetComponent<PlayerInputScript>();
        m_LampController = GetComponentInChildren<LampController>();
        m_GroundScript = GetComponent<GroundScript>();
    }
}
