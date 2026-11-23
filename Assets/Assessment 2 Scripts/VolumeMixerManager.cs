using UnityEngine;
using UnityEngine.Audio;

public class VolumeMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;

    public void SetMasterVolume(float m_Level)
    {
        m_AudioMixer.SetFloat("MasterVolume", m_Level);
    }

    public void SetEffectsVolume(float m_Level)
    {
        m_AudioMixer.SetFloat("SoundEffectsVolume", m_Level);

    }

    public void SetMusicVolume(float m_Level)
    {
        m_AudioMixer.SetFloat("MusicVolume", m_Level);

    }

}
