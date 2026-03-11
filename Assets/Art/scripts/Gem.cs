using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 宝石脚本Gem
public class Gem : MonoBehaviour
{
    Collider2D coll;
    Transform target;

    public float speed = 10;

    void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    // 宝石有较大的触发器范围，只要玩家进入范围，则关闭触发器，飞向目标target
    private void OnTriggerEnter2D(Collider2D collision)
    {
        coll.enabled = false;
        target = collision.transform;
    }

    // 当存在target时，飞向target
    private void Update()
    {
        if (target)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            transform.position = pos;

            float dist = Vector3.Distance(transform.position, target.position);

            // 当离目标仅有0.1米时，则销毁宝石，目标的LevelUp脚本增加1点经验
            if (dist < 0.1f)
            {
                target.GetComponent<LevelUp>().OnGetGem(1);
                Destroy(gameObject);
            }
        }
    }
}
