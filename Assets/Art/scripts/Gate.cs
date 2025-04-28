using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gate : MonoBehaviour
{
    public Transform to;
    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 只对带有 "Player" Tag 的物体生效
        if (collision.gameObject.CompareTag("Player"))
        {
            // 播放 SFX Array 中的第 4 个元素（Element?3 → 索引 3）
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(3);
            }

            // 传送玩家
            collision.gameObject.transform.position = to.position;
        }
    }
}
