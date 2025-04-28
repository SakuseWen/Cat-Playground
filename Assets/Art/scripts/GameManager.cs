/**************************************************************************
 *  GameManager.cs —— 关卡流程 / UI 面板 / 摄像机绑定
 *  2025-04-28 版本：支持
 *      • 每关单独配置普通怪上限、Boss 数量
 *      • enemyIndices 任选多种怪混合刷
 **************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Cinemachine;

#region 数据结构：每关的配置
[System.Serializable]
public class LevelConfig
{
    [Header("本关刷怪上限")]
    public int spawnLimit = 30;

    [Header("本关胜利条件 (击杀数)")]
    public int killTarget = 15;

    [Header("本关应生成多少只 Boss (0 = 不生成)")]
    public int bossCount = 0;

    [Header("本关要刷的怪 index 列表 (留空 = 全部)")]
    public List<int> enemyIndices = new List<int>();     // ← 多选！
}
#endregion

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    /* ---------- Inspector 字段 ---------- */
    [Header("关卡配置列表")]
    public List<LevelConfig> levelsConfig = new List<LevelConfig>();

    [Header("场景中挂有 Spawner.cs 的物体引用")]
    public Spawner spawner;                     // 运行时自动查找

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    public GameObject nextLevelPanel;

    /* ---------- 运行时状态 ---------- */
    private static int staticLevelIndex = 0;    // 跨场景保存
    private int currentLevelIndex => staticLevelIndex;
    private int currentKillCount = 0;
    private bool isPaused = false;

    /* ---------- 生命周期 ---------- */
    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    /* ---------- 场景加载后初始化 ---------- */
    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        currentKillCount = 0;

        // 重新查找 Spawner / UI
        spawner = Object.FindFirstObjectByType<Spawner>();
        winPanel = GameObject.Find("Canvas/WinPanel");
        losePanel = GameObject.Find("Canvas/LosePanel");
        pausePanel = GameObject.Find("Canvas/PausePanel");
        nextLevelPanel = GameObject.Find("Canvas/NextLevelPanel");
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);
        if (nextLevelPanel) nextLevelPanel.SetActive(false);
        GameObject.Find("Canvas/LevelUpPanel")?.SetActive(false);

        StartCoroutine(ClearSelectionNextFrame());

        BindButtons();

        // 绑定 Cinemachine 跟随玩家
        Player player = Object.FindFirstObjectByType<Player>();
        if (player)
        {
            var vcam = Object.FindFirstObjectByType<CinemachineVirtualCamera>();
            if (vcam) vcam.Follow = vcam.LookAt = player.transform;

            if (spawner) spawner.follow = player.transform;
        }

        /* ---------- 把本关参数交给 Spawner ---------- */
        InitLevel(currentLevelIndex);

        Time.timeScale = 1f;
    }

    /* ---------- 私有协程：等 1 帧再清焦点 ---------- */
    System.Collections.IEnumerator ClearSelectionNextFrame()       
    {
        yield return null;                                         // 等 EventSystem 完成默认选中
        EventSystem.current?.SetSelectedGameObject(null);          // 清焦点 → 蒙版立即隐藏
    }

    /* ---------- 关卡初始化核心 ---------- */
    void InitLevel(int idx)
    {
        if (idx < 0 || idx >= levelsConfig.Count || spawner == null) return;

        LevelConfig cfg = levelsConfig[idx];

        /* 1️⃣ 先恢复 Spawner 的完整敌人列表 */
        spawner.enemies = new List<Enemy>(spawner.originalEnemies);

        /* 2️⃣ 如果 enemyIndices 非空 → 裁剪成子集 */
        if (cfg.enemyIndices != null && cfg.enemyIndices.Count > 0)
        {
            List<Enemy> subset = new List<Enemy>();
            foreach (int id in cfg.enemyIndices)
                if (id >= 0 && id < spawner.originalEnemies.Count)
                    subset.Add(spawner.originalEnemies[id]);

            if (subset.Count > 0)
                spawner.enemies = subset;

            Debug.Log($"[Level {idx + 1}] 只刷 index = {string.Join(",", cfg.enemyIndices)}");
        }
        else
        {
            Debug.Log($"[Level {idx + 1}] enemyIndices 为空 → 混合刷全部");
        }

        /* 3️⃣ 再启动刷怪协程（此时列表已确定） */
        spawner.Configure(cfg.spawnLimit, cfg.bossCount);

        Debug.Log($"[Level {idx + 1}] spawnLimit={cfg.spawnLimit}, " +
                  $"killTarget={cfg.killTarget}, bossCount={cfg.bossCount}");
    }

    /* ---------- UI / 按钮 ---------- */
    void BindButtons()
    {
        void Bind(GameObject panel, string child, UnityEngine.Events.UnityAction act)
        {
            if (!panel) return;
            var t = panel.transform.Find(child);
            if (t) t.GetComponent<Button>().onClick.AddListener(act);
        }
        Bind(winPanel, "RestartButton", RestartLevel);
        Bind(winPanel, "PlayAgainButton", PlayAgain);
        Bind(winPanel, "QuitButton", QuitGame);
        Bind(nextLevelPanel, "NextLevelButton", NextLevel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton9))
            TogglePause();

    }

    /* ---------- 关卡流程 ---------- */
    public void EnemyKilled()
    {
        currentKillCount++;
        if (currentKillCount >= levelsConfig[currentLevelIndex].killTarget)
            OnLevelComplete();
    }

    void OnLevelComplete()
    {
        GameObject.Find("Canvas/LevelUpPanel")?.SetActive(false);
        Time.timeScale = 0f;

        if (currentLevelIndex < levelsConfig.Count - 1)
            nextLevelPanel?.SetActive(true);
        else
            winPanel?.SetActive(true);

        AudioManager.Instance?.PlaySFX(0);
    }

    /* ---------- 场景跳转 ---------- */
    public void RestartLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    public void PlayAgain() { staticLevelIndex = 0; SkillDefine.ResetCache(); RestartLevel(); }
    public void NextLevel() { staticLevelIndex++; RestartLevel(); }

    /* ---------- 失败 / 暂停 / 退出 ---------- */
    public void Lose()
    {
        Time.timeScale = 0f;
        losePanel?.SetActive(true);
        AudioManager.Instance?.PlaySFX(1);
    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel?.SetActive(isPaused);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
