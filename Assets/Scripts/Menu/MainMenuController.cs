using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string sceneName;
    public void QuitGame()
    {
        Application.Quit();
    }


public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
