using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 毒气效果脚本
/// 处理毒气区域的持续伤害逻辑
/// </summary>
public class PoisonGas : MonoBehaviour
{
    [Header("毒气参数")]
    public float duration = 3f;           // 毒气持续时间
    public float damageInterval = 1f;     // 伤害间隔时间


    public void Start()
    {
        Initialize(3,1);
        AudioManager.Instance?.PlaySFX(5);
    }

    /// <summary>
    /// 初始化毒气参数
    /// </summary>
    public void Initialize(float duration, float damageInterval)
    {
        this.duration = duration;
        this.damageInterval = damageInterval;

        StartCoroutine(DestroyAfterDuration());  // 启动自动销毁协程
    }

    /// <summary>
    /// 毒气持续时间结束后自动销毁
    /// </summary>
    IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 对玩家造成伤害
            PlayerBeHit health = collision.GetComponent<PlayerBeHit>();
            if (health != null)
            {
                health.TakeDamage();
            }
            Destroy(gameObject);

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 对玩家造成伤害
            PlayerBeHit health = collision.GetComponent<PlayerBeHit>();
            if (health != null)
            {
                health.TakeDamage();
            }
            Destroy(gameObject);

        }
    }
}