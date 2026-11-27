using UnityEngine;
using System.Collections;

public class VoiceActivated : MonoBehaviour
{
    [SerializeField] private AudioClip m_VoiceClip;
    [SerializeField] private bool m_FreezeGameWhilePlaying;
    float m_PrevTimeScale;
    private Coroutine m_VoiceCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // SoundManager.instance.PlaySound(m_VoiceClip, transform, 1f);
       
        //  Destroy(gameObject);            
        m_VoiceCoroutine = StartCoroutine(m_PlayVoiceLines());

        
    }
    private IEnumerator m_PlayVoiceLines()
    {
        m_PrevTimeScale = Time.timeScale;
        if(m_FreezeGameWhilePlaying)
        {
            Time.timeScale = 0f;
        }
        SoundManager.instance.PlaySound(m_VoiceClip, transform, 1f);
        if (m_FreezeGameWhilePlaying)
        {
            yield return new WaitForSecondsRealtime(m_VoiceClip.length);
            Time.timeScale = m_PrevTimeScale;
        }
        Destroy(gameObject);

    }
}
