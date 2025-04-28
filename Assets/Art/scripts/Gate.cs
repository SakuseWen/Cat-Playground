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
        // ֻ�Դ��� "Player" Tag ��������Ч
        if (collision.gameObject.CompareTag("Player"))
        {
            // ���� SFX Array �еĵ� 4 ��Ԫ�أ�Element?3 �� ���� 3��
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(3);
            }

            // �������
            collision.gameObject.transform.position = to.position;
        }
    }
}
