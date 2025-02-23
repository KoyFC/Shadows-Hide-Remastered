using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementScript : MonoBehaviour
{
    private PlayerController m_PlayerController = null;
    private Rigidbody2D m_Rigidbody = null;
    
    void Start()
    {
        m_PlayerController = GetComponent<PlayerController>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
}
