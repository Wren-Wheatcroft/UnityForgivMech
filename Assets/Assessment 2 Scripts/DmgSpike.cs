using UnityEngine;

public class DmgSpike : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int m_DamageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth m_PlayerHealth = other.GetComponentInParent<PlayerHealth>();
        if (m_PlayerHealth != null)
        {
            m_PlayerHealth.TakeDamage(m_DamageAmount);
            Debug.Log("DamageApplied:"+ m_DamageAmount );
        
        }
        else
        {
            Debug.Log("NotFound!");
        }
    }
}
