using UnityEngine;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;

public class Collectible : MonoBehaviour
{
    public int m_Score;
    public int m_Value = 1;
    [SerializeField] private AudioClip m_CoinNoise;


    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(m_CoinNoise, transform, 1f);
        if (collision.CompareTag("Player"))
        {
            m_Score += m_Value;
           // SoundManager.instance.PlaySound(m_CoinNoise, transform, 1f);

            Debug.Log("+" + m_Value + "Collected - " + "Score Now - " + m_Score);
            Destroy(gameObject);
            
        }
    }
}
