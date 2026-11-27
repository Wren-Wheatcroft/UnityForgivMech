using UnityEngine;
using UnityEngine.UIElements;

public class GameUIHandler : MonoBehaviour
{
    public PlayerHealth PlayerHealth;
    public UIDocument UIDoc;
    private Label m_HealthLabel;
    private VisualElement[] m_HealthBars;

    private void Awake()
    {
        #region m_HealthBar
        m_HealthBars = new VisualElement[] 
        {
            UIDoc.rootVisualElement.Q<VisualElement>("Heart1"),
            UIDoc.rootVisualElement.Q<VisualElement>("Heart2"),
            UIDoc.rootVisualElement.Q<VisualElement>("Heart3"),
            UIDoc.rootVisualElement.Q<VisualElement>("Heart4"),
            UIDoc.rootVisualElement.Q<VisualElement>("Heart5"),
        };
        for (int i = 0; i <m_HealthBars.Length; i++)
        {
            if (m_HealthBars[i] == null)
            {
                Debug.LogError("healthbar is missing number - " + (i+ 1));
                return;
            }
        }
  
        m_HealthLabel = UIDoc.rootVisualElement.Q<Label>("HealthLabel");

        #endregion
    }
    private void Start()
    {
        if (PlayerHealth == null) 
        {
            Debug.LogError("PlyrHealth is missing");
            return;
        }
         if (UIDoc == null) 
        {
            Debug.LogError("UiDoc is missing");
            return;
        }

        PlayerHealth.ui_OnHealthChange += HealthChanged;
        HealthChanged();

        HealthChanged();
    }


    void HealthChanged()
    {
        int health = Mathf.Clamp(PlayerHealth.m_CurrentHealth, 1, 5);
        for (int i = 0; i <m_HealthBars.Length; i++)
        {
            m_HealthBars[i].style.display = i == health -1 ? DisplayStyle.Flex : DisplayStyle.None;
        }
        m_HealthLabel.text = $"{PlayerHealth.m_CurrentHealth}/{PlayerHealth.m_MaxHealth}";
    }

    private void OnDestroy()
    {
        if (PlayerHealth != null)
        {
            PlayerHealth.ui_OnHealthChange -= HealthChanged;
        }
    }
}