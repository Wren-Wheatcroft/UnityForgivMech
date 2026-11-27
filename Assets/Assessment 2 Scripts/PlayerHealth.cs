using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    #region Variables!
    private SpriteRenderer m_SpriteRenderer;
    [Header("Health")]
    [SerializeField] public int m_MaxHealth = 5;
    [SerializeField] public int m_CurrentHealth;
    public event System.Action ui_OnHealthChange;
   
    [Header("Damage")]
    [SerializeField] private Color m_DamageColor = Color.red;
    [SerializeField] private float m_FlashDuration = 0.1f;
    [SerializeField] private int m_FlashNum = 3;
    [SerializeField] bool m_CanTakeDamage = true;
    [SerializeField] float m_InvTime;
    [SerializeField] private AudioClip[] m_DamageSoundClips;
    [SerializeField] public bool m_IsHurt;
    [SerializeField] private float m_HurtDuration = 0.3f;
    private Coroutine m_HurtCoroutine;



    [Header("Particles")]
    [SerializeField] private ParticleSystem  m_BloodParticles;    
    [SerializeField] private ParticleSystem  m_BloodBurstParticles;


    [Header("Death")]
    [SerializeField] public bool m_IsDead;
    [SerializeField] public float m_DeathDelay = 2f;
    private CharacterMovement m_CharacterMovement;
    [SerializeField] private AudioClip[] m_DeathSoundClips;
    private Coroutine DeathCoroutine;
    private Color m_OgColor;
    private Coroutine m_FlashCoroutine;
    private Coroutine m_InvCoroutine;
    
    [Header("Knockback")]
    [SerializeField] private float m_KnockbackForce =5f;
    [SerializeField] private float m_KnockbackUpForce = 2f;
    private float m_KnockDirection;

    private Rigidbody2D m_RB;
    private CameraShake m_CameraShake;
    #endregion

    private void Awake()
    {
        m_SpriteRenderer = transform.Find("Sprite").GetComponentInChildren<SpriteRenderer>();
        m_CurrentHealth = m_MaxHealth;
        m_OgColor = m_SpriteRenderer.color;
        Debug.Log("Player Health is:" + m_CurrentHealth);
        m_CharacterMovement = GetComponent<CharacterMovement>();
        m_RB = GetComponent<Rigidbody2D>();
        m_CameraShake = Camera.main.GetComponent<CameraShake>();
    }
#region TakeDamage
    public void TakeDamage(int Damage)
    {
        if (!m_CanTakeDamage || m_CurrentHealth <= 0)
        {
            return;
        }

        m_CurrentHealth -= Damage; 
        m_CurrentHealth = Mathf.Max(m_CurrentHealth, 0);
        ui_OnHealthChange?.Invoke();
        SoundManager.instance.PlayRandomSound(m_DamageSoundClips, transform, 1f);
        Debug.Log("TakeDamage Was Called");
        Debug.Log("Player Health: " + m_CurrentHealth);
        if (m_HurtCoroutine != null)
        {
            StopCoroutine(m_HurtCoroutine);
        }
        m_HurtCoroutine = StartCoroutine(HurtAnimation());
        Knockback();
        if (m_CameraShake!= null)
        {
            m_CameraShake.ShakeCamera();
        }

        


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
    #endregion
#region Knockback
private void Knockback()
{
    
    m_KnockDirection = m_SpriteRenderer.flipX ? -1f : 1f;
    Vector2 m_KnockbackDirection = new Vector2 (m_KnockDirection * m_KnockbackForce, m_KnockbackUpForce);
    m_CharacterMovement.Knockback(m_KnockbackDirection);

}

#endregion
#region Death
private void Die()
    {
        Debug.Log("Player Died");
        m_IsDead = true;
        m_CharacterMovement.m_Animator.SetBool("IsDead", true);
        SoundManager.instance.PlayRandomSound(m_DeathSoundClips, transform, 1f);
        m_CharacterMovement.enabled = false;
        
        Rigidbody2D m_RB = GetComponent<Rigidbody2D>();
        m_RB.linearVelocity = Vector2.zero;
        m_RB.bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine (Death());

    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(m_DeathDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            m_BloodParticles.Play();
            m_BloodBurstParticles.Play();


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
    #region HurtAnimation 
    private IEnumerator HurtAnimation()
    {
        m_IsHurt = true;
        yield return new WaitForSeconds(m_HurtDuration);
        m_IsHurt = false;
        m_HurtCoroutine = null;
    }

    private void Update()
    {
           m_CharacterMovement.m_Animator.SetBool("IsHurt", m_IsHurt);

    }
    #endregion
}
