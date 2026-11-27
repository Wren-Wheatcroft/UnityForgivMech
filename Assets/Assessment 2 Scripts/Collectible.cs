using UnityEngine;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;


public class Collectible : MonoBehaviour
{
    public int m_Value = 1;
    [SerializeField] private AudioClip m_CoinNoise;
    [SerializeField] private float m_TimeBetweenDestroy = 0.3f;
    [SerializeField] private ParticleSystem  m_CoinBurstParticle;
 
    void Awake()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(m_CoinNoise, transform, 1f);
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.AddScore(m_Value);

            Debug.Log("+" + m_Value + "Collected - " + "Score Now - " + GameManager.instance.m_Score);
        
            Destroy(gameObject);            
        }
    }

}
