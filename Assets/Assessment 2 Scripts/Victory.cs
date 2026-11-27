using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Unity.VisualScripting;

public class Victory : MonoBehaviour
{
    [SerializeField] private AudioClip  m_VictorySound;
    [SerializeField] private TimeUiHandler m_Timer;
    [SerializeField] private GameManager m_GameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {

    SoundManager.instance.PlaySound(m_VictorySound, transform, 1f);
       MainMenu.FinalTime = m_Timer.GetFormattedTime();
        MainMenu.FinalCoins = m_GameManager.m_Score;

    SceneManager.LoadScene("WinScreen");
     

    }
}
