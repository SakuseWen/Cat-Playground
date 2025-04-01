using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人脚本
// 敌人只是简单冲向玩家、设置动画方向
public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;

    Dir dir;
    Vector2 move;
    public float speed = 3;

    Transform player;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        player = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        if (!player)
        {
            return;
        }

        // 玩家位置减去自身位置，得到从自身到玩家的向量
        Vector2 to = player.position - transform.position;

        if (to.magnitude < 0.1f)
        {
            return;
        }

        // 移动方向: 朝向玩家
        move = to.normalized;

        if (to.magnitude > 0.1f)
        {
            dir = Utils.Vec2Dir(move);
        }

        anim.SetInteger("dir", (int)dir);
    }

    private void FixedUpdate()
    {
        rigid.linearVelocity = move * speed;
    }
}
