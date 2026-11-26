using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int m_Score;
    public static GameManager instance;

    public void Awake()
    {
        instance = this;

    }
    public void AddScore(int amount)
    {
        m_Score += amount;

        Debug.Log("Current Score" + m_Score );
    }
}
