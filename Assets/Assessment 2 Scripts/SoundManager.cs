using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource m_SoundObject;
    float m_ClipLength;
    int m_Rand;

    private void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
#region Play Sound
    public void PlaySound(AudioClip m_AudioClip, Transform m_SpawnTransform, float m_Volume)
    {
        AudioSource m_AudioSource = Instantiate(m_SoundObject, m_SpawnTransform.position, Quaternion.identity);
        m_AudioSource.clip = m_AudioClip;
        m_AudioSource.volume = m_Volume;
        m_AudioSource.Play();
        m_ClipLength = m_AudioSource.clip.length;
        Destroy(m_AudioSource.gameObject, m_ClipLength);

    }
    #endregion
    #region PlayRandomSound
       public void PlayRandomSound(AudioClip[] m_AudioClip, Transform m_SpawnTransform, float m_Volume)
    {
        int m_Rand = Random.Range(0, m_AudioClip.Length);
        AudioSource m_AudioSource = Instantiate(m_SoundObject, m_SpawnTransform.position, Quaternion.identity);
        m_AudioSource.clip = m_AudioClip[m_Rand];
        m_AudioSource.volume = m_Volume;
        m_AudioSource.Play();
        m_ClipLength = m_AudioSource.clip.length;
        Destroy(m_AudioSource.gameObject, m_ClipLength);

    }
    #endregion

}
