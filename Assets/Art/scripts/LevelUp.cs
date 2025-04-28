/*******************************************************
 *  LevelUp.cs �D�D ���� / ������ / �������
 *******************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUp : MonoBehaviour
{
    [Header("��ǰ�ȼ� / ����")]
    public int level = 1;
    public int exp = 0;

    [Header("UI ���� (����ʱ�ذ�)")]
    public Image expImage;
    public GameObject levelUpPanel;

    Player player;

    /* ---------------- �������� ---------------- */
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

    /* ---------------- �����л� ---------------- */
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

    /* ---------------- ��þ��� ---------------- */
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

    /* ---------------- �������� ---------------- */
    void OnLevelUp()
    {
        level++;
        exp = 0;

        if (!levelUpPanel) return;

        // ˢ��������ť������ & �ɵ�״̬
        UpdateButton(0, player.leftSkill);
        UpdateButton(1, player.rightSkill);
        UpdateButton(2, null);                 // HP ʼ�տɵ�

        levelUpPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /* ���� ���ݼ����Ƿ�����ˢ�°�ť ���� */
    void UpdateButton(int index, SkillDefine sd)
    {
        Transform btnTr = levelUpPanel.transform.GetChild(index);
        Button btn = btnTr.GetComponent<Button>();
        Text txt = btnTr.GetChild(0).GetComponent<Text>();

        if (sd == null)                        // HP ��ť
        {
            btn.interactable = true;
            txt.text = "HP +1";
            return;
        }

        bool isMax = sd.skillLevel >= sd.cooldown.Count;
        btn.interactable = !isMax;
        txt.text = isMax ? "MAX" : $"{sd.skillName} {sd.skillLevel + 1}";
    }

    /* ---------------- ������ˢ�� ---------------- */
    public void UpdateExpBar(int maxExp = -1)
    {
        if (!expImage) return;
        if (maxExp < 0) maxExp = CalcLevelExp(level);
        expImage.fillAmount = (float)exp / maxExp;
    }
    public int CalcLevelExp(int lv) => lv * 5;

    /* ---------------- ��尴ť ---------------- */
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
        // �Ѿ� MAX �� �޶���
        if (sd.skillLevel >= sd.cooldown.Count) return;

        sd.skillLevel++;
        sd.Remember();                 // ���浽����
        HidePanel();
    }

    public void HidePanel()
    {
        if (levelUpPanel) levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
