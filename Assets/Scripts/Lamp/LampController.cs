using UnityEngine;

[RequireComponent(typeof(LampMovementScript))]
[RequireComponent(typeof(LampInputScript))]
public class LampController : MonoBehaviour
{
    internal PlayerController m_PlayerController;
    private LampMovementScript m_LampMovementScript;
    internal LampInputScript m_LampInputScript;

    void Awake()
    {
        GetAllComponents();
    }

    private void GetAllComponents()
    {
        m_PlayerController = GetComponentInParent<PlayerController>();
        m_LampMovementScript = GetComponent<LampMovementScript>();
        m_LampInputScript = GetComponent<LampInputScript>();
    }
}
