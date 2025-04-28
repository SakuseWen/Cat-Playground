using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能定义 —— 负责保存 / 还原自己的等级
/// </summary>
public class SkillDefine : MonoBehaviour
{
    [Tooltip("技能名称（必须唯一！）")]
    public string skillName;

    [Tooltip("技能等级（Inspector 中为初始等级）")]
    public int skillLevel = 1;

    [Tooltip("技能冷却时间，按等级填写多个")]
    public List<float> cooldown;

    [Tooltip("技能预制体，按等级填多个，数量与 CD 相同")]
    public List<Transform> prefabs;

    /* ---------- 全局缓存 ---------- */
    static Dictionary<string, int> cache = new Dictionary<string, int>();

    void Awake()
    {
        // 若缓存中已有记录，则覆盖 Inspector 中的默认等级
        if (cache.TryGetValue(skillName, out int lv))
            skillLevel = lv;
    }

    /// <summary>把当前等级写入缓存，供下一个场景读取</summary>
    public void Remember() => cache[skillName] = skillLevel;

    /// <summary>重新开一局时调用，清空所有缓存</summary>
    public static void ResetCache() => cache.Clear();
}
