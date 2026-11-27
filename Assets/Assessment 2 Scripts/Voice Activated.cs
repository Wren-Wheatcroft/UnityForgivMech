using UnityEngine;

public class VoiceActivated : MonoBehaviour
{
    [SerializeField] private AudioClip m_VoiceClip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(m_VoiceClip, transform, 1f);
       
         Destroy(gameObject);            
        
    }
}
