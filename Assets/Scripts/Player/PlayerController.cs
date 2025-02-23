using UnityEngine;

[RequireComponent(typeof(HealthScript))]
[RequireComponent(typeof(PlayerMovementScript))]
[RequireComponent(typeof(PlayerInputScript))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovementScript m_PlayerMovementScript;
    private PlayerInputScript m_PlayerInputScript;

    void Awake()
    {
        GetAllComponents();
    }

    private void GetAllComponents()
    {
        m_PlayerMovementScript = GetComponent<PlayerMovementScript>();
        m_PlayerInputScript = GetComponent<PlayerInputScript>();
    }
}
