using UnityEngine;
using UnityEngine.UIElements;
using System;


public class TimeUiHandler : MonoBehaviour
{
    public UIDocument UIDoc;
    private Label m_TimerLabel;
    public float m_TimeElapsed;

    int m_Mins;
    int m_Secs;

    private void Start()
    {
        m_TimerLabel = UIDoc.rootVisualElement.Q<Label>("TimerLabel");
    }
    private void Update()
    {
        m_TimeElapsed += Time.deltaTime;
        m_Mins = Mathf.FloorToInt(m_TimeElapsed / 60);
        m_Secs = Mathf.FloorToInt(m_TimeElapsed % 60);


        m_TimerLabel.text = $"{m_Mins:0}:{m_Secs:00}";

        

    }
        public string GetFormattedTime()
    {
        return $"{m_Mins:0}:{m_Secs:00}";
    }
}
