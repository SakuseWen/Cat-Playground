using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 技能定义类，可以定义出一个技能的一串等级
public class SkillDefine : MonoBehaviour
{
    [Tooltip("技能名称")]
    public string skillName;

    [Tooltip("技能等级（填初始等级）")]
    public int skillLevel = 1;

    [Tooltip("技能冷却时间，按等级填写多个")]
    public List<float> cooldown;

    [Tooltip("技能预制体，按等级填多个，数量与CD时间相同")]
    public List<Transform> prefabs;
}

