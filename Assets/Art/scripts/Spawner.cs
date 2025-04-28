/**************************************************************************
 *  Spawner.cs —— 普通敌人 + Boss 生成器
 *  2025-04-28 版本：支持
 *      • Boss 数量上限
 *      • originalEnemies 备份，供 GameManager 每关裁剪
 **************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    /* ---------- Inspector 参数 ---------- */
    [Tooltip("刷怪半径（随机方向）")]
    public float randomRadius = 5f;

    [Tooltip("普通怪生成间隔")]
    public float timeGap = 1f;

    [Tooltip("可随机挑选的普通敌人列表")]
    public List<Enemy> enemies = new List<Enemy>();

    [Header("Boss Settings")]
    public Enemy bossEnemy;                // Boss 预制体
    public float bossSpawnInterval = 60f;  // 两次 Boss 之间的间隔
    public float bossSpawnRadius = 7f;

    /* ---------- 运行时状态 ---------- */
    [HideInInspector] public Transform follow;          // 跟随玩家移动
    [HideInInspector] public List<Enemy> originalEnemies;   // Inspector 列表备份

    private int normalSpawnLimit = 0;
    private int normalSpawnedCount = 0;

    private int bossSpawnLimit = 0;
    private int bossSpawnedCount = 0;
    private float bossTimer = 0f;

    private Coroutine normalCo;

    /* ---------- Awake：备份原始敌人列表 ---------- */
    void Awake()
    {
        originalEnemies = new List<Enemy>(enemies);
    }

    /* ---------- 公共 API：由 GameManager 调用 ---------- */
    public void Configure(int normalLimit, int bossLimit)
    {
        /* 普通怪 */
        normalSpawnLimit = Mathf.Max(0, normalLimit);
        normalSpawnedCount = 0;

        /* Boss */
        bossSpawnLimit = Mathf.Max(0, bossLimit);
        bossSpawnedCount = 0;
        bossTimer = 0f;

        /* 重新开始普通怪刷怪协程 */
        if (normalCo != null) StopCoroutine(normalCo);
        if (normalSpawnLimit > 0)
            normalCo = StartCoroutine(RefreshNormalEnemies());
    }

    /* ---------- Update：计时刷 Boss & 跟随 ---------- */
    void Update()
    {
        if (follow) transform.position = follow.position;

        if (bossSpawnLimit == 0 || bossEnemy == null) return;
        if (bossSpawnedCount >= bossSpawnLimit) return;

        bossTimer += Time.deltaTime;
        if (bossTimer >= bossSpawnInterval)
        {
            SpawnBoss();
            bossTimer = 0f;
        }
    }

    /* ---------- 普通敌人协程 ---------- */
    IEnumerator RefreshNormalEnemies()
    {
        while (normalSpawnedCount < normalSpawnLimit)
        {
            SpawnNormalEnemy();
            normalSpawnedCount++;
            yield return new WaitForSeconds(timeGap);
        }
    }

    void SpawnNormalEnemy()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector2 pos = dir * randomRadius;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, randomRadius, LayerMask.GetMask("Wall"));
        if (hit.collider) pos = dir * hit.distance;

        if (enemies.Count == 0) return;
        Enemy prefab = enemies[Random.Range(0, enemies.Count)];

        Enemy e = Instantiate(prefab);
        e.transform.position = (Vector2)transform.position + pos;
    }

    /* ---------- Boss 生成 ---------- */
    void SpawnBoss()
    {
        if (bossSpawnedCount >= bossSpawnLimit || bossEnemy == null) return;

        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector2 pos = dir * bossSpawnRadius;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, bossSpawnRadius, LayerMask.GetMask("Wall"));
        if (hit.collider) pos = dir * hit.distance;

        Enemy boss = Instantiate(bossEnemy);
        boss.transform.position = (Vector2)transform.position + pos;

        bossSpawnedCount++;
        Debug.Log($"Boss spawned at {boss.transform.position}   ({bossSpawnedCount}/{bossSpawnLimit})");
    }
}
