using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private GameObject m_SoundPrefab;
    float m_ClipLength;
    int m_Rand;

    private void Start ()
    {
        m_SoundPrefab = new GameObject();
        m_SoundPrefab.AddComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
        }

        Debug.Log(m_SoundPrefab + " - Meow");
    }

#region Play Sound
    public void PlaySound(AudioClip m_AudioClip, Transform m_SpawnTransform, float m_Volume)
    {
        GameObject m_SoundObject = Instantiate(m_SoundPrefab, m_SpawnTransform.position, Quaternion.identity);
        AudioSource m_AudioSource = m_SoundObject.GetComponent<AudioSource>();
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
        GameObject m_SoundObject = Instantiate(m_SoundPrefab, m_SpawnTransform.position, Quaternion.identity);
        AudioSource m_AudioSource = m_SoundObject.GetComponent<AudioSource>();
        m_AudioSource.clip = m_AudioClip[m_Rand];
        m_AudioSource.volume = m_Volume;
        m_AudioSource.Play();
        m_ClipLength = m_AudioSource.clip.length;
        Destroy(m_AudioSource.gameObject, m_ClipLength);

    }
    #endregion

}
