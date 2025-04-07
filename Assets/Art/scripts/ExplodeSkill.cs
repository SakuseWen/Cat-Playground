using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 爆炸技能
public class ExplodeSkill : MonoBehaviour
{
    Collider2D coll;
    public int attack = 1;

    // 爆炸技能原理——先激活碰撞体，等1帧物理帧，再关闭碰撞体即可
    IEnumerator Start()
    {
        // 播放爆炸特效音
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(2);  
        }

        Destroy(gameObject, 1.0f);      // 过1秒销毁自身

        coll = GetComponent<Collider2D>();

        coll.enabled = false;      // 关闭碰撞体
        yield return new WaitForFixedUpdate();

        coll.enabled = true;       // 激活碰撞体
        yield return new WaitForFixedUpdate();

        coll.enabled = false;      // 关闭碰撞体
    }

}
