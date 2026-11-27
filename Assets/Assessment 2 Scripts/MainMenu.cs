using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
[Header("Ui Refernce")]
[SerializeField] private UIDocument UIDoc;

private VisualElement m_MainMenu;
private Button m_PlayButton;
private Button m_QuitButton;
private Label m_FinalTimeCounter;
private Label m_FinalCoinCounter;
public static string FinalTime;
public static int FinalCoins;

private void Start()
{
        VisualElement root = UIDoc.rootVisualElement;

        m_MainMenu = root.Q<VisualElement>("MainMenu");
        m_PlayButton = root.Q<Button>("PlayButton");
        m_QuitButton = root.Q<Button>("QuitButton");
        m_FinalTimeCounter = root.Q<Label>("FinalTimeCounter");
        m_FinalCoinCounter = root.Q<Label>("FinalCoinCounter");
        m_PlayButton.clicked += PlayGame;
        m_QuitButton.clicked += QuitGame;

        if(UIDoc == null )
        {
            Debug.LogError("UiDocument Is missing!");
            return;
        }
        Debug.Log("MainMenu" + (m_MainMenu != null));
        Debug.Log("PlayButton" + (m_PlayButton != null));
        Debug.Log("QuitButton" + (m_QuitButton != null));
        m_FinalTimeCounter.text = FinalTime;
        m_FinalCoinCounter.text = FinalCoins.ToString();



}

private void PlayGame()
{
    Debug.Log("Play Button Pressed");
    SceneManager.LoadScene("Development Scene");

}
private void QuitGame()
{
    Application.Quit();
}
}
