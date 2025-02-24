using UnityEngine;
using System.Collections;

public class GroundScript : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    [SerializeField] private LayerMask m_GroundLayer;

    [Header("Ground Saver Settings")]
    [SerializeField] private float m_SaveFrequency = 5.0f;
    [SerializeField] private bool m_SavePosition = true;
    [SerializeField] private float m_RayCastLength = 1.1f;
    private bool m_KeepSaving = true;
    private Vector2 m_LastGroundPosition = Vector2.zero;

    void Start()
    {
        m_KeepSaving = m_SavePosition;

        m_LastGroundPosition = transform.position;

        StartCoroutine(SaveGround());
    }

    void Update()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, m_RayCastLength, m_GroundLayer);

        // Stop the coroutine if it is not told to save the position
        if (!m_SavePosition && m_KeepSaving)
        {
            m_KeepSaving = false;
        }
        // Reactivate the coroutine if it was stopped and it is told to save the position
        else if (m_SavePosition && !m_KeepSaving)
        {
            m_KeepSaving = true;
            StartCoroutine(SaveGround());
        }
    }

    private IEnumerator SaveGround()
    {
        while (m_KeepSaving)
        {
            yield return new WaitUntil(() => IsGrounded);

            m_LastGroundPosition = transform.position;

            yield return new WaitForSeconds(m_SaveFrequency);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            GameManagerScript.Instance.TriggerDeathSequence(m_LastGroundPosition);
        }
    }
}
