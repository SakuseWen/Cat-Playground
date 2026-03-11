/*******************************************************
 *  LevelUp.cs ―― 升级 / 经验条 / 升级面板
 *******************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUp : MonoBehaviour
{
    [Header("当前等级 / 经验")]
    public int level = 1;
    public int exp = 0;

    [Header("UI 引用 (运行时重绑)")]
    public Image expImage;
    public GameObject levelUpPanel;

    Player player;

    /* ---------------- 生命周期 ---------------- */
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (levelUpPanel == null)
        {
            Canvas cvs = Object.FindFirstObjectByType<Canvas>();
            if (cvs) levelUpPanel = cvs.transform.Find("LevelUpPanel")?.gameObject;
        }
        if (levelUpPanel) levelUpPanel.SetActive(false);
    }
    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void Start()
    {
        player = GetComponent<Player>();
        RefreshPanelRef();
        UpdateExpBar();
    }

    /* ---------------- 场景切换 ---------------- */
    void OnSceneLoaded(Scene s, LoadSceneMode m) => RefreshPanelRef();
    void RefreshPanelRef()
    {
        if (!levelUpPanel)
        {
            Canvas cvs = Object.FindFirstObjectByType<Canvas>();
            if (cvs) levelUpPanel = cvs.transform.Find("LevelUpPanel")?.gameObject;
        }
        if (levelUpPanel) levelUpPanel.SetActive(false);
    }

    /* ---------------- 获得经验 ---------------- */
    public void OnGetGem(int e)
    {
        exp += e;
        int maxExp = CalcLevelExp(level);

        if (exp >= maxExp)
        {
            OnLevelUp();
            maxExp = CalcLevelExp(level);
        }
        UpdateExpBar(maxExp);
    }

    /* ---------------- 升级触发 ---------------- */
    void OnLevelUp()
    {
        level++;
        exp = 0;

        if (!levelUpPanel) return;

        // 刷新三个按钮的文字 & 可点状态
        UpdateButton(0, player.leftSkill);
        UpdateButton(1, player.rightSkill);
        UpdateButton(2, null);                 // HP 始终可点

        levelUpPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /* —— 根据技能是否满级刷新按钮 —— */
    void UpdateButton(int index, SkillDefine sd)
    {
        Transform btnTr = levelUpPanel.transform.GetChild(index);
        Button btn = btnTr.GetComponent<Button>();
        Text txt = btnTr.GetChild(0).GetComponent<Text>();

        if (sd == null)                        // HP 按钮
        {
            btn.interactable = true;
            txt.text = "HP +1";
            return;
        }

        bool isMax = sd.skillLevel >= sd.cooldown.Count;
        btn.interactable = !isMax;
        txt.text = isMax ? "MAX" : $"{sd.skillName} {sd.skillLevel + 1}";
    }

    /* ---------------- 经验条刷新 ---------------- */
    public void UpdateExpBar(int maxExp = -1)
    {
        if (!expImage) return;
        if (maxExp < 0) maxExp = CalcLevelExp(level);
        expImage.fillAmount = (float)exp / maxExp;
    }
    public int CalcLevelExp(int lv) => lv * 5;

    /* ---------------- 面板按钮 ---------------- */
    public void OnButton1() => TryUpSkill(player.leftSkill);
    public void OnButton2() => TryUpSkill(player.rightSkill);
    public void OnButton3()
    {
        HidePanel();
        var bh = player.GetComponent<PlayerBeHit>();
        bh.maxHp += 1;
        bh.HealAllHp();
    }

    void TryUpSkill(SkillDefine sd)
    {
        // 已经 MAX → 无动作
        if (sd.skillLevel >= sd.cooldown.Count) return;

        sd.skillLevel++;
        sd.Remember();                 // 保存到缓存
        HidePanel();
    }

    public void HidePanel()
    {
        if (levelUpPanel) levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
