using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private GameObject loseUI;
    [SerializeField] private TextMeshProUGUI kalanCanUI;
    [SerializeField] private EnemyController enemyController;
    
    // Þu anki sahneyi yeniden yükler

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
        kalanCanUI.text = "Kalan Canýnýz\n" + (5 - enemyController.kizmaCounter);

        if (3 - enemyController.kizmaCounter <= 0)
            kalanCanUI.text = "Kalan Canýnýz \n0";

        if(enemyController.kizmaCounter >= 5)
        {
            if (loseUI.activeSelf == false)
            {
                loseUI.SetActive(true);
                Time.timeScale = 0;
            }
        }    
    }
    // Build Settings'teki 1 numaralý sahneyi yükler
    public void LoadSceneIndex1()
    {
        if (SceneManager.sceneCountInBuildSettings > 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogWarning("Build Settings'te 1 numaralý sahne yok.");
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}