using UnityEngine;

[RequireComponent(typeof(LampMovementScript))]
[RequireComponent(typeof(LampInputScript))]
public class LampController : MonoBehaviour
{
    #region Variables
    internal PlayerController m_PlayerController;
    [HideInInspector] public LampMovementScript m_LampMovementScript;
    [HideInInspector] public LampInputScript m_LampInputScript;
    #endregion

    #region Main Methods
    void Awake()
    {
        GetAllComponents();
    }

    private void GetAllComponents()
    {
        m_PlayerController = GetComponentInParent<PlayerController>();
        m_LampInputScript = GetComponent<LampInputScript>();
    }
    #endregion
}
