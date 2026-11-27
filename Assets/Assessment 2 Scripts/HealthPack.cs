using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private int m_HealAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.m_CurrentHealth < playerHealth.m_MaxHealth)
            {
                playerHealth.Heal(m_HealAmount);

                Destroy(gameObject);
            }
        }
    }
}