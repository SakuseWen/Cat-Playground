using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 玩家脚本
public class SimplePlayer : MonoBehaviour
{
    Rigidbody2D rigid;          // 刚体组件
    SpriteRenderer sprite;      // 精灵图组件（在子物体上）
    Animator anim;              // 动画状态机组件

    Vector2 move;

    [Tooltip("移动速度")]
    public float walkSpeed = 3;
    public float runSpeed = 6;

    // 用于在 Update 内部计算好当前速度，然后在 FixedUpdate 中使用
    private float currentSpeed;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 获取横向和纵向输入，变量范围0.0f ~ 1.0f
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        move = new Vector2(h, v);
        if (move.magnitude > 1)     // 当上和右同时按下时，move向量长度会超过1
        {
            move = move.normalized;
        }

        // 按住 Shift 就把 speed 设置为 runSpeed，否则就是 walkSpeed
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // 方向变量dir
        Dir dir = Dir.None;
        if (move.magnitude > 0.1f)
        {
            dir = Utils.Vec2Dir(move);
        }

        // 玩家动画不分左右，需要将精灵图翻转
        if (dir == Dir.Right)
        {
            sprite.flipX = false;
        }
        else if (dir == Dir.Left)
        {
            sprite.flipX = true;
        }

        // 设置动画变量，speed区分 停止/跑，dir区分方向 
        anim.SetFloat("speed", Mathf.Max(Mathf.Abs(move.x), Mathf.Abs(move.y)));

        if (dir != Dir.None)
        {
            anim.SetInteger("dir", (int)dir);
        }
    }

    private void FixedUpdate()
    {
        // 修改刚体速度，才能真的动起来
        rigid.linearVelocity = move * currentSpeed;
    }
}
