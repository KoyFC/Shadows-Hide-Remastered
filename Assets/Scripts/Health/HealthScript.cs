using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    [SerializeField, Min(0)] private int m_StartingHealth = 80;
    [SerializeField, Min(0)] public int m_MaxHealth = 100;
    [Min(0)] public int m_CurrentHealth = 0;

    [Header("Regeneration")]
    [Tooltip("Is the regeneration system enabled?")]
    [SerializeField] private bool m_CanRegen = true;
    [Tooltip("Should the health start regenerating? (Only works if m_CanRegen is true)")]
    [SerializeField] private bool m_StartRegen = true;
    [SerializeField, Min(0)] private int m_RegenRate = 1;
    [SerializeField, Min(0)] private float m_RegenDelay = 1f;

    [Header("Invincibility")]
    [SerializeField] private GameObject m_ShieldPrefab = null;
    [SerializeField] private Vector2 m_ShieldScale = new Vector2(2.5f, 2.5f);
    [SerializeField] private Color m_FlashingColor = Color.white;
    private Color m_OriginalColor = Color.white;
    [SerializeField, Min(0)] private int m_FlashCount = 3;
    [SerializeField, Min(0)] private float m_FlashDuration = 0.1f;
    [SerializeField, Min(0)] private float m_InvincibilityTime = 0.5f;
    private bool m_IsInvincible = false;

    [Header("Damage")]
    [SerializeField] private bool m_DestroyOnDeath = true;
    [SerializeField] private float m_TimeBeforeDestroy = 0.25f;
    #endregion

    #region Main Methods
    void Start()
    {
        m_CurrentHealth = m_StartingHealth;
        m_OriginalColor = GetComponent<SpriteRenderer>().material.color;
    }

    void Update()
    {
        // Regenerate health while m_CanRegen is true and we told it to keep regening
        if (m_CanRegen && m_StartRegen && m_CurrentHealth < m_MaxHealth)
        {
            m_StartRegen = false;
            Invoke(nameof(RegenHealth), m_RegenDelay);
        }
    }
    #endregion

    #region Helper Methods
    private void RegenHealth()
    {
        // We stop regenerating health if we're dead. Otherwise we keep going.
        if (m_CurrentHealth > 0)
        {
            m_StartRegen = true;
        }
        else
        {
            m_StartRegen = false;
            return;
        }

        m_CurrentHealth += m_RegenRate;
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, m_MaxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (!m_IsInvincible)
        {
            m_CurrentHealth -= damage;

            if (m_CurrentHealth <= 0)
            {
                if (m_DestroyOnDeath)
                {
                    Destroy(gameObject, m_TimeBeforeDestroy);
                }
                else
                {
                    m_CurrentHealth = 0;
                    gameObject.SetActive(false);
                }
            }

            if (gameObject.activeSelf)
            {
                StartCoroutine(InvincibleAfterHit());
            }
        }
    }

    private IEnumerator InvincibleAfterHit()
    {
        StartCoroutine(Flash());
        
        if (m_InvincibilityTime <= 0) yield break;

        m_IsInvincible = true;

        // Spawning the shield if the prefab is set
        GameObject shield = null;
        if (m_ShieldPrefab != null)
        {
            shield = Instantiate(m_ShieldPrefab, transform);
            shield.transform.localScale = m_ShieldScale;
        }

        yield return new WaitForSeconds(m_InvincibilityTime);

        // If a shield was spawned, destroy it
        if (shield != null)
        {
            Destroy(shield);
        }

        m_IsInvincible = false;
    }

    private IEnumerator Flash()
    {
        Renderer renderer = GetComponent<Renderer>();
        for (int i = 0; i < m_FlashCount; i++)
        {
            renderer.material.color = m_FlashingColor;
            yield return new WaitForSeconds(m_FlashDuration);

            renderer.material.color = m_OriginalColor;
            yield return new WaitForSeconds(m_FlashDuration);
        }
    }
    #endregion
}
