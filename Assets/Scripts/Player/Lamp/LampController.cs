using UnityEngine;

[RequireComponent(typeof(LampMovementScript))]
[RequireComponent(typeof(LampInputScript))]
public class LampController : MonoBehaviour
{
    internal PlayerController m_PlayerController;
    [HideInInspector] public LampMovementScript m_LampMovementScript;
    [HideInInspector] public LampInputScript m_LampInputScript;

    void Awake()
    {
        GetAllComponents();
    }

    private void GetAllComponents()
    {
        m_PlayerController = GetComponentInParent<PlayerController>();
        m_LampInputScript = GetComponent<LampInputScript>();
    }
}
