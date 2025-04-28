using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 子弹脚本（猫爪），简单向前方飞行
public class Bullet : MonoBehaviour
{
    Rigidbody2D rigid;
    public float speed = 10;
    public int attack = 1;
    
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        // 播放射击特效音
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(1);
        }
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    // 碰到其它物体销毁自身
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        Destroy(gameObject);
    }
}
