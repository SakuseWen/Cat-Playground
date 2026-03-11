using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /* ─────────── 组件 & 变量 ─────────── */
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;

    Vector2 move;
    Vector2 fireDir = new Vector2(0, -1);

    [Tooltip("移动速度")] public float speed = 3;

    [Header("技能与 CD")]
    public SkillDefine leftSkill;   // 射击
    public SkillDefine rightSkill;  // 爆炸
    float nextFireTime;
    float nextExplodeTime;

    [Tooltip("技能图标的半透明遮挡图")]
    public Image explodeIconMask;

    /* ───────────  PS-5 按键编号固定 ─────────── */
    readonly KeyCode shootKey = KeyCode.JoystickButton1; // ✖ Cross
    readonly KeyCode explodeKey = KeyCode.JoystickButton2; // ⭕ Circle
    readonly KeyCode runKey = KeyCode.JoystickButton6; // L2（若不同请改）

    /* ========== 生命周期 ========== */
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    /* ========== 每帧更新 ========== */
    void Update()
    {
        /* 1️⃣  移动输入 */
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        move = new Vector2(h, v);
        if (move.magnitude > 1f) move = move.normalized;

        /* 2️⃣  瞄准方向：手柄=面向，键鼠=鼠标 */
        bool isControllerShoot = Input.GetKey(shootKey) || Input.GetKey(explodeKey);
        if (isControllerShoot)
        {
            if (move.magnitude > 0.1f) fireDir = move.normalized;
        }
        else
        {
            Vector3 worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMouse.z = transform.position.z;
            fireDir = (worldMouse - transform.position).normalized;
        }

        /* 3️⃣  动画与翻转 */
        Dir dir = Dir.None;
        if (move.magnitude > 0.1f) dir = Utils.Vec2Dir(move);
        sprite.flipX = dir == Dir.Left;
        anim.SetFloat("speed", Mathf.Max(Mathf.Abs(move.x), Mathf.Abs(move.y)));
        if (dir != Dir.None) anim.SetInteger("dir", (int)dir);

        /* 4️⃣  射击（鼠标左键 或 ✖） */
        if (leftSkill &&
            (Input.GetMouseButton(0) || Input.GetKey(shootKey)))
        {
            Fire();
        }

        /* 5️⃣  爆炸（鼠标右键 或 ⭕） */
        if (rightSkill)
        {
            if (explodeIconMask)
            {
                float rem = nextExplodeTime - Time.time;
                int lvl = rightSkill.skillLevel;
                explodeIconMask.fillAmount =
                    lvl <= 0 ? 1f : rem / rightSkill.cooldown[lvl - 1];
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(explodeKey))
            {
                Explode();
            }
        }
    }

    /* ========== 固定帧移动 ========== */
    void FixedUpdate()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(runKey);
        rigid.linearVelocity = move * speed * (isRunning ? 2f : 1f);
    }

    /* ========== 开火 ========== */
    void Fire()
    {
        if (Time.time < nextFireTime) return;
        int lvl = leftSkill.skillLevel;
        float cd = leftSkill.cooldown[lvl - 1];
        nextFireTime = Time.time + cd;

        Transform bullet = Instantiate(leftSkill.prefabs[lvl - 1]);
        bullet.position = transform.position;
        bullet.up = fireDir;
    }

    /* ========== 爆炸 ========== */
    void Explode()
    {
        if (Time.time < nextExplodeTime) return;
        int lvl = rightSkill.skillLevel;
        if (lvl <= 0) return;

        float cd = rightSkill.cooldown[lvl - 1];
        nextExplodeTime = Time.time + cd;

        Transform e = Instantiate(rightSkill.prefabs[lvl - 1]);
        e.position = transform.position;
    }
}
