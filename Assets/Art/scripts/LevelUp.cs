using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 玩家升级脚本。涉及到较多UI操作
public class LevelUp : MonoBehaviour
{
    public int level = 1;   // 当前等级
    public int exp = 0;     // 当前经验值
    public Image expImage;  // 经验条图片

    public GameObject levelUpPanel;

    Player player;

    private void Start()
    {
        player = GetComponent<Player>();

        if (expImage)
        {
            expImage.fillAmount = (float)exp / CalcLevelExp(level);
        }

        if (levelUpPanel)
        {
            levelUpPanel.SetActive(false);
        }
    }

    // 当获取到经验宝石时被调用
    public void OnGetGem(int e)
    {
        exp += e;

        int maxExp = CalcLevelExp(level);
        if (exp >= maxExp)
        {
            OnLevelUp();
            maxExp = CalcLevelExp(level);
        }
        if (expImage)
        {
            expImage.fillAmount = (float)exp / maxExp;
        }
    }

    public void OnLevelUp()
    {
        Debug.Log("升级！");
        level++;
        exp = 0;

        if (levelUpPanel)
        {
            Text text1 = levelUpPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>();

            Text text2 = levelUpPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>();

            Text text3 = levelUpPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>();

            text1.text = player.leftSkill.skillName + $" {player.leftSkill.skillLevel + 1}level";
            text2.text = player.rightSkill.skillName + $" {player.rightSkill.skillLevel + 1}level";
            text3.text = "Life limit +1";

            levelUpPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public int CalcLevelExp(int level)
    {
        return level * 5;
    }

    public void OnButton1()
    {
        Debug.Log("OnButton1");
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;

        if (player.leftSkill.skillLevel >= player.leftSkill.cooldown.Count)
        {
            Debug.Log($"技能{player.leftSkill.name}已经满级");
            return;
        }
        player.leftSkill.skillLevel++;
    }

    public void OnButton2()
    {
        Debug.Log("OnButton2");
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;

        if (player.rightSkill.skillLevel >= player.rightSkill.cooldown.Count)
        {
            Debug.Log($"技能{player.rightSkill.name}已经满级");
            return;
        }

        player.rightSkill.skillLevel++;
    }

    public void OnButton3()
    {
        Debug.Log("OnButton3");
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;

        var behit = player.GetComponent<PlayerBeHit>();
        behit.maxHp += 1;
        behit.HealAllHp();
    }
}
