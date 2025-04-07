using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;

    [Header("胜利条件")]
    [Tooltip("击杀目标数量，可在 Inspector 中修改")]
    public int killTarget = 10;
    private int currentKillCount = 0;

    private bool isPaused;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    // 当敌人被击杀时调用
    public void EnemyKilled()
    {
        currentKillCount++;
        Debug.Log("Enemy killed: " + currentKillCount);
        if (currentKillCount >= killTarget)
        {
            Win();
        }
    }

    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        pausePanel.SetActive(false);
    }


    /* ---------- 公共 API ---------- */
    public void Win()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
        AudioManager.Instance.PlaySFX(0);  
    }

    public void Lose()
    {
        Time.timeScale = 0f;
        losePanel.SetActive(true);
        AudioManager.Instance.PlaySFX(1);   
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu() => SceneManager.LoadScene("MainMenu");
}
