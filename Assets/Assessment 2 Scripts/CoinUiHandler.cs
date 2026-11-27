using UnityEngine;
using UnityEngine.UIElements;

public class CoinUiHandler : MonoBehaviour
{
    public GameManager GameManager;
    public UIDocument UIDoc;
    private Label m_CoinLabel;

    private void Start()
    {
        m_CoinLabel = UIDoc.rootVisualElement.Q<Label>("CoinLabel");
        GameManager.ui_OnScoreChange += ScoreChanged;
        ScoreChanged();
    }
    private void ScoreChanged()
    {
        m_CoinLabel.text = GameManager. m_Score.ToString();
    }
    private void OnDestroy()
    {
        if (GameManager != null)
        {
            GameManager.ui_OnScoreChange -=ScoreChanged;
        }
    }
}