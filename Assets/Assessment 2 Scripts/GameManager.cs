using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public int m_Score;
    public static GameManager instance;
    public event Action ui_OnScoreChange;

    public void Awake()
    {
        instance = this;

    }
    public void AddScore(int amount)
    {
        m_Score += amount;
        ui_OnScoreChange?.Invoke();

        Debug.Log("Current Score" + m_Score );
    }
}
