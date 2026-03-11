using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss敌人脚本，继承自Enemy基类
/// 添加了特殊攻击能力：多方向子弹攻击和毒气攻击
/// </summary>
public class Boss : Enemy
{
    [Header("攻击参数")]
    public float bulletAttackInterval = 3f;  // 子弹攻击间隔时间（秒）
    public float poisonAttackInterval = 5f;  // 毒气攻击间隔时间（秒）
    public EnemyBullet bossBulletPrefab;         // Boss子弹预制体
    public GameObject poisonGasPrefab;      // 毒气效果预制体
    public float poisonGasDuration = 3f;    // 毒气持续时间（秒）
  
    public float poisonDamageInterval = 1f; // 毒气伤害间隔时间（秒）

    private float nextBulletAttackTime;     // 下一次子弹攻击时间
    private float nextPoisonAttackTime;    // 下一次毒气攻击时间

    /// <summary>
    /// 初始化方法，继承并扩展基类的Start
    /// </summary>
    public void Start()
    {
        base.Start();  // 调用父类的Start方法

        // 初始化下次攻击时间
        nextBulletAttackTime = Time.time + bulletAttackInterval;
        nextPoisonAttackTime = Time.time + poisonAttackInterval;
    }

    /// <summary>
    /// 每帧更新方法，继承并扩展基类的Update
    /// </summary>
    void Update()
    {
        base.Update(); // 调用父类的Update方法处理基础移动逻辑

        // 检查是否到达子弹攻击时间
        if (Time.time >= nextBulletAttackTime)
        {
            StartCoroutine(PerformBulletAttack());  // 执行子弹攻击协程
            nextBulletAttackTime = Time.time + bulletAttackInterval;  // 重置下次攻击时间
        }

        // 检查是否到达毒气攻击时间
        if (Time.time >= nextPoisonAttackTime)
        {
            PerformPoisonAttack();  // 执行毒气攻击
            nextPoisonAttackTime = Time.time + poisonAttackInterval;  // 重置下次攻击时间
        }
    }

    /// <summary>
    /// 执行子弹攻击协程
    /// 随机选择4方向或8方向攻击
    /// </summary>
    IEnumerator PerformBulletAttack()
    {
        // 随机决定使用4方向还是8方向（50%概率）
        bool useEightDirections = Random.value > 0.5f;

        if (useEightDirections)
        {
            // 8方向攻击（每45度一个方向）
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f;  // 计算当前角度
                Vector2 direction = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),  // 计算X分量
                    Mathf.Sin(angle * Mathf.Deg2Rad));   // 计算Y分量
                FireBullet(direction);  // 朝该方向发射子弹
            }
        }
        else
        {
            // 4方向攻击（每90度一个方向）
            for (int i = 0; i < 4; i++)
            {
                float angle = i * 90f;  // 计算当前角度
                Vector2 direction = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad));
                FireBullet(direction);  // 朝该方向发射子弹
            }
        }

        yield return null;  // 协程暂停一帧
    }

    /// <summary>
    /// 发射子弹方法
    /// </summary>
    /// <param name="direction">发射方向</param>
    void FireBullet(Vector3 direction)
    {
        if (bossBulletPrefab)
        {
            // 实例化子弹并设置方向
            EnemyBullet bullet = Instantiate(bossBulletPrefab, transform.position, Quaternion.identity);
           
            bullet.transform.up= direction.normalized;
        }
    }

    /// <summary>
    /// 执行毒气攻击
    /// </summary>
    void PerformPoisonAttack()
    {
        if (poisonGasPrefab)
        {
            // 实例化毒气效果并初始化参数
            GameObject poisonGas = Instantiate(poisonGasPrefab, transform.position, Quaternion.identity);
            poisonGas.GetComponent<PoisonGas>().Initialize(
                poisonGasDuration,
                poisonDamageInterval);
        }
    }
}