using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class DmgSpike : MonoBehaviour
{
    private static readonly int IsHurtHash = Animator.StringToHash("IsHurt");
    #region Variables!
    [Header("Damage")]
    [SerializeField] private int m_DamageAmount = 1;
    [SerializeField] private float m_DamageInterval =1f;
    private Coroutine m_DamageCorotuine;
    #endregion
    #region OnEnter/Exit
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth m_PlayerHealth = other.GetComponentInParent<PlayerHealth>();
        if (m_PlayerHealth != null)
        {
           // m_PlayerHealth.TakeDamage(m_DamageAmount);
            //Debug.Log("DamageApplied:"+ m_DamageAmount );
            m_DamageCorotuine = StartCoroutine (DmgOverTime(m_PlayerHealth));
        }
        else
        {
            Debug.Log("NotFound!");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
         PlayerHealth m_PlayerHealth = other.GetComponentInParent<PlayerHealth>();
            if (m_PlayerHealth!=null && m_DamageCorotuine !=null)
            {
                StopCoroutine(m_DamageCorotuine);
                m_DamageCorotuine = null;
            }
    }
    #endregion
    #region DamageOVerTime
    private IEnumerator DmgOverTime(PlayerHealth m_PlayerHealth)
    {
        while (true)
        {
            m_PlayerHealth.TakeDamage(m_DamageAmount);
            Debug.Log("DamageApplied:"+ m_DamageAmount );

            yield return new WaitForSeconds(m_DamageInterval);

        }
    }
    #endregion
 
}
