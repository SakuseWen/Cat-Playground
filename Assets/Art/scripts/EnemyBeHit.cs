using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人受伤脚本
public class EnemyBeHit : MonoBehaviour
{
    public int hp = 5;

    FlashColor flashColor;      // 闪红光的脚本

    public Transform prefabHeadNum;   // 预制体：头顶冒出的数字

    public Gem prefabGem;       // 预制体：死后掉落的宝石
    
    void Start()
    {
        flashColor = GetComponent<FlashColor>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 根据被不同的物体碰到，读取伤害值
        int damage = 1;
        if (collision.gameObject.GetComponent<Bullet>())
        {
            damage = collision.gameObject.GetComponent<Bullet>().attack;
        }
        else if (collision.gameObject.GetComponent<ExplodeSkill>())
        {
            damage = collision.gameObject.GetComponent<ExplodeSkill>().attack;
        }

        // 闪红光
        if (flashColor)
        {
            flashColor.Flash(0.1f);
        }

        // 头顶冒数字，数字为伤害值
        if (prefabHeadNum)
        {
            Transform headNum = Instantiate(prefabHeadNum);
            headNum.position = transform.position + new Vector3(0, 0.9f, 0);
            headNum.GetComponent<HeadText>().Show(damage);
        }

        hp -= damage;       // 实际掉血
        if (hp <= 0)        // 如果hp<=0则死亡，销毁自身，掉落宝石
        {
            Destroy(gameObject);
            if (prefabGem)
            {
                Gem gem = Instantiate(prefabGem);
                gem.transform.position = transform.position;
            }
        }

    }

}
