using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 玩家脚本
public class Player : MonoBehaviour
{
    Rigidbody2D rigid;          // 刚体组件
    SpriteRenderer sprite;      // 精灵图组件（在子物体上）
    Animator anim;              // 动画状态机组件

    Vector2 move;
    Vector2 fireDir = new Vector2(0, -1);

    [Tooltip("移动速度")]
    public float speed = 3;

    [Tooltip("左键技能")]
    public SkillDefine leftSkill;
    float nextFireTime;     // 计算左键技能的CD时间

    [Tooltip("右键技能")]
    public SkillDefine rightSkill;
    float nextExplodeTime;  // 计算右键技能的CD时间

    [Tooltip("技能图标的半透明遮挡图")]
    public Image explodeIconMask;

    void Start()
    {
        rigid= GetComponent<Rigidbody2D>();
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

        // 获取鼠标指向的射击方向
        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePos_World = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos_World.z = transform.position.z;
        fireDir = mousePos_World - transform.position;
        fireDir = fireDir.normalized;

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

        // 如果设置了左键技能，且按下左键，则释放左键技能
        if (leftSkill)
        {
            if (Input.GetButton("Fire1"))
            {
                Fire();
            }
        }

        // 如果设置了右键技能
        if (rightSkill)
        {
            // 如果设置了技能遮罩图，展示旋转冷却的效果（旋转的比例 0~1）
            if (explodeIconMask)
            {
                float explodeTime = nextExplodeTime - Time.time;
                int skillLevel = rightSkill.skillLevel;
                if (skillLevel <= 0)
                {
                    explodeIconMask.fillAmount = 1;
                }
                else
                {
                    float cd = rightSkill.cooldown[skillLevel - 1];
                    explodeIconMask.fillAmount = explodeTime / cd;
                }
            }

            // 如果按下了鼠标右键，释放右键技能
            if (Input.GetButtonDown("Fire2"))
            {
                Explode();
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rigid.linearVelocity = move * speed * 2;
            return;
        }
        rigid.linearVelocity = move * speed;
    }

    private void Fire()
    {
        if (Time.time < nextFireTime)
        {
            return;
        }

        int skillLevel = leftSkill.skillLevel;
        float cd = leftSkill.cooldown[skillLevel - 1];
        nextFireTime = Time.time + cd;

        Transform prefab = leftSkill.prefabs[skillLevel - 1];

        Transform bullet = Instantiate(prefab);
        bullet.position = transform.position;
        bullet.up = fireDir.normalized;
    }

    private void Explode()
    {
        if (Time.time < nextExplodeTime)
        {
            return;
        }

        int skillLevel = rightSkill.skillLevel;
        if (skillLevel <= 0)
        {
            return;
        }

        float cd = rightSkill.cooldown[skillLevel - 1];
        nextExplodeTime = Time.time + cd;

        Transform prefab = rightSkill.prefabs[skillLevel - 1];

        Transform e = Instantiate(prefab);
        e.position = transform.position;
    }


}
