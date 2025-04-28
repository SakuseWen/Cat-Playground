using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ч���ű�
/// ����������ĳ����˺��߼�
/// </summary>
public class PoisonGas : MonoBehaviour
{
    [Header("��������")]
    public float duration = 3f;           // ��������ʱ��
    public float damageInterval = 1f;     // �˺����ʱ��


    public void Start()
    {
        Initialize(3,1);
        AudioManager.Instance?.PlaySFX(5);
    }

    /// <summary>
    /// ��ʼ����������
    /// </summary>
    public void Initialize(float duration, float damageInterval)
    {
        this.duration = duration;
        this.damageInterval = damageInterval;

        StartCoroutine(DestroyAfterDuration());  // �����Զ�����Э��
    }

    /// <summary>
    /// ��������ʱ��������Զ�����
    /// </summary>
    IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // ���������˺�
            PlayerBeHit health = collision.GetComponent<PlayerBeHit>();
            if (health != null)
            {
                health.TakeDamage();
            }
            Destroy(gameObject);

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // ���������˺�
            PlayerBeHit health = collision.GetComponent<PlayerBeHit>();
            if (health != null)
            {
                health.TakeDamage();
            }
            Destroy(gameObject);

        }
    }
}