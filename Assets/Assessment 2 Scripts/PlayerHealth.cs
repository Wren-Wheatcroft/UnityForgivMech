using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region Variables!
    private SpriteRenderer m_SpriteRenderer;
    [Header("Health")]
    [SerializeField] private int m_MaxHealth = 5;
    [SerializeField] private int m_CurrentHealth;
    [Header("Damage")]
    [SerializeField] private Color m_DamageColor = Color.red;
    [SerializeField] private float m_FlashDuration = 0.1f;
    [SerializeField] private int m_FlashNum = 3;
    [SerializeField] bool m_CanTakeDamage = true;
    [SerializeField] float m_InvTime;
    private Color m_OgColor;
    private Coroutine m_FlashCoroutine;
    private Coroutine m_InvCoroutine;
#endregion

    private void Awake()
    {
        m_SpriteRenderer = transform.Find("Sprite").GetComponentInChildren<SpriteRenderer>();
        m_CurrentHealth = m_MaxHealth;
        m_OgColor = m_SpriteRenderer.color;
        Debug.Log("Player Health is:" + m_CurrentHealth);
    }

    public void TakeDamage(int Damage)
    {
        if (!m_CanTakeDamage || m_CurrentHealth <= 0)
        {
            return;
        }

        m_CurrentHealth -= Damage; 
        m_CurrentHealth = Mathf.Max(m_CurrentHealth, 0);
        Debug.Log("Player Health: " + m_CurrentHealth);

        if (m_FlashCoroutine !=null)
        {
            StopCoroutine(m_FlashCoroutine);
        }

        m_FlashCoroutine = StartCoroutine(DamageFlash());
        m_InvCoroutine = StartCoroutine(Invulenerability());

        if (m_CurrentHealth <= 0)
        {
            Die();
        }

    }

#region Death
private void Die()
    {
        Debug.Log("Player Died");

    }
    #endregion

    #region Dmg Flash
private IEnumerator DamageFlash()
    {
        for (int i = 0; i < m_FlashNum; i++)
        {
            m_SpriteRenderer.color = m_DamageColor;
            yield return new WaitForSeconds (m_FlashDuration);
            m_SpriteRenderer.color = m_OgColor;
            yield return new WaitForSeconds (m_FlashDuration);
        }
        m_SpriteRenderer.color = m_OgColor;
        m_FlashCoroutine = null;
    }
    #endregion
    #region Invulenerability
    private IEnumerator Invulenerability ()
    {
        m_CanTakeDamage = false;
        yield return new WaitForSeconds (m_InvTime);
        m_CanTakeDamage = true;
        m_InvCoroutine= null;
    }
    #endregion
}
