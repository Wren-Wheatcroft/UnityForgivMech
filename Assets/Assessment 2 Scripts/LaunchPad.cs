using UnityEngine;

 public class LaunchPad : MonoBehaviour
{
     public CharacterMovement m_PlayerMoveVars;
     [SerializeField] private AudioClip[] m_BoingAudioClips;
     [SerializeField] private ParticleSystem  m_JumpPadParticles;




 private void OnCollisionEnter2D(Collision2D collision)
  {
       if (collision.gameObject.TryGetComponent(out Rigidbody2D rb2d));
        {
         m_JumpPadParticles.Play();
         SoundManager.instance.PlayRandomSound(m_BoingAudioClips, transform, 1f);
         rb2d.AddForce(transform.up * m_PlayerMoveVars.m_JumpStrength, ForceMode2D.Impulse); 
         
        }
     }
 }
