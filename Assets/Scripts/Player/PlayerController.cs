using UnityEngine;

[RequireComponent(typeof(HealthScript))]
[RequireComponent(typeof(PlayerMovementScript))]
[RequireComponent(typeof(PlayerInputScript))]
[RequireComponent(typeof(GroundScript))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    [HideInInspector] public PlayerMovementScript m_PlayerMovementScript;
    [HideInInspector] public PlayerInputScript m_PlayerInputScript;
    internal LampController m_LampController;
    internal GroundScript m_GroundScript;

    [SerializeField] private GameObject m_Lamp;
    public bool m_LampActive = true;
    #endregion

    #region Main Methods
    void Awake()
    {
        GetAllComponents();

        m_Lamp.SetActive(m_LampActive);
    }

    private void Update()
    {
        if (m_PlayerInputScript.m_SummonPressed)
        {
            m_LampActive = !m_LampActive;
            m_Lamp.SetActive(m_LampActive);
        }
    }
    #endregion

    #region Helper Methods
    private void GetAllComponents()
    {
        m_PlayerMovementScript = GetComponent<PlayerMovementScript>();
        m_PlayerInputScript = GetComponent<PlayerInputScript>();
        m_LampController = GetComponentInChildren<LampController>();
        m_GroundScript = GetComponent<GroundScript>();
    }
    #endregion
}
