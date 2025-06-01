using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private GameObject loseUI;
    [SerializeField] private TextMeshProUGUI kalanCanUI;
    [SerializeField] private EnemyController enemyController;
    
    // �u anki sahneyi yeniden y�kler

    private void Start()
    {
        if (enemyController == null)
            enemyController = GameObject.FindObjectOfType<EnemyController>();

        loseUI.SetActive(false);
    }
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    private void Update()
    {
        kalanCanUI.text = "Kalan Can�n�z\n" + (5 - enemyController.kizmaCounter);

        if (3 - enemyController.kizmaCounter <= 0)
            kalanCanUI.text = "Kalan Can�n�z \n0";

        if(enemyController.kizmaCounter >= 5)
        {
            if (loseUI.activeSelf == false)
            {
                loseUI.SetActive(true);
                Time.timeScale = 0;
            }
        }    
    }
    // Build Settings'teki 1 numaral� sahneyi y�kler
    public void LoadSceneIndex1()
    {
        if (SceneManager.sceneCountInBuildSettings > 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogWarning("Build Settings'te 1 numaral� sahne yok.");
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}