using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuFunctions : MonoBehaviour
{
    public GameObject TutorialPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void openTutorial()
    {
        TutorialPanel.SetActive(true);
    }
    
    public void closeTutorial()
    {
        TutorialPanel.SetActive(false);
    }
}
