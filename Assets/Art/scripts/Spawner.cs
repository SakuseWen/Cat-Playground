using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 刷怪脚本
public class Spawner : MonoBehaviour
{
    [Tooltip("刷怪半径，有随机性")]
    public float randomRadius = 5.0f;
    [Tooltip("刷怪时间间隔")]
    public float timeGap = 1.0f;
    [Tooltip("敌人列表，随机选取敌人")]
    public List<Enemy> enemies;
    [Tooltip("刷怪点持续追随一个目标")]
    public Transform follow;

    void Start()
    {
        StartCoroutine(RefreshEnemy());
    }

    private void Update()
    {
        if (follow)
        {
            transform.position = follow.position;
        }
    }

    IEnumerator RefreshEnemy()
    {
        while (true)
        {
            Vector2 vec = Random.insideUnitCircle.normalized;
            Vector2 pos = vec * randomRadius;

            // 由于pos的范围可能越界，所以从玩家位置发射线，如果碰到边缘就按边缘位置
            RaycastHit2D hit = Physics2D.Raycast(transform.position, vec, 99999, LayerMask.GetMask("Wall"));
            if (hit.collider && hit.distance < randomRadius)
            {
                pos = vec * hit.distance;
            }

            int index = Random.Range(0, enemies.Count);

            Enemy enemy = Instantiate(enemies[index]);

            enemy.transform.position = pos + (Vector2)transform.position;
            yield return new WaitForSeconds(timeGap);
        }
    }
}
