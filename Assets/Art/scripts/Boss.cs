using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss���˽ű����̳���Enemy����
/// ��������⹥���������෽���ӵ������Ͷ�������
/// </summary>
public class Boss : Enemy
{
    [Header("��������")]
    public float bulletAttackInterval = 3f;  // �ӵ��������ʱ�䣨�룩
    public float poisonAttackInterval = 5f;  // �����������ʱ�䣨�룩
    public EnemyBullet bossBulletPrefab;         // Boss�ӵ�Ԥ����
    public GameObject poisonGasPrefab;      // ����Ч��Ԥ����
    public float poisonGasDuration = 3f;    // ��������ʱ�䣨�룩
  
    public float poisonDamageInterval = 1f; // �����˺����ʱ�䣨�룩

    private float nextBulletAttackTime;     // ��һ���ӵ�����ʱ��
    private float nextPoisonAttackTime;    // ��һ�ζ�������ʱ��

    /// <summary>
    /// ��ʼ���������̳в���չ�����Start
    /// </summary>
    public void Start()
    {
        base.Start();  // ���ø����Start����

        // ��ʼ���´ι���ʱ��
        nextBulletAttackTime = Time.time + bulletAttackInterval;
        nextPoisonAttackTime = Time.time + poisonAttackInterval;
    }

    /// <summary>
    /// ÿ֡���·������̳в���չ�����Update
    /// </summary>
    void Update()
    {
        base.Update(); // ���ø����Update������������ƶ��߼�

        // ����Ƿ񵽴��ӵ�����ʱ��
        if (Time.time >= nextBulletAttackTime)
        {
            StartCoroutine(PerformBulletAttack());  // ִ���ӵ�����Э��
            nextBulletAttackTime = Time.time + bulletAttackInterval;  // �����´ι���ʱ��
        }

        // ����Ƿ񵽴ﶾ������ʱ��
        if (Time.time >= nextPoisonAttackTime)
        {
            PerformPoisonAttack();  // ִ�ж�������
            nextPoisonAttackTime = Time.time + poisonAttackInterval;  // �����´ι���ʱ��
        }
    }

    /// <summary>
    /// ִ���ӵ�����Э��
    /// ���ѡ��4�����8���򹥻�
    /// </summary>
    IEnumerator PerformBulletAttack()
    {
        // �������ʹ��4������8����50%���ʣ�
        bool useEightDirections = Random.value > 0.5f;

        if (useEightDirections)
        {
            // 8���򹥻���ÿ45��һ������
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f;  // ���㵱ǰ�Ƕ�
                Vector2 direction = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),  // ����X����
                    Mathf.Sin(angle * Mathf.Deg2Rad));   // ����Y����
                FireBullet(direction);  // ���÷������ӵ�
            }
        }
        else
        {
            // 4���򹥻���ÿ90��һ������
            for (int i = 0; i < 4; i++)
            {
                float angle = i * 90f;  // ���㵱ǰ�Ƕ�
                Vector2 direction = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad));
                FireBullet(direction);  // ���÷������ӵ�
            }
        }

        yield return null;  // Э����ͣһ֡
    }

    /// <summary>
    /// �����ӵ�����
    /// </summary>
    /// <param name="direction">���䷽��</param>
    void FireBullet(Vector3 direction)
    {
        if (bossBulletPrefab)
        {
            // ʵ�����ӵ������÷���
            EnemyBullet bullet = Instantiate(bossBulletPrefab, transform.position, Quaternion.identity);
           
            bullet.transform.up= direction.normalized;
        }
    }

    /// <summary>
    /// ִ�ж�������
    /// </summary>
    void PerformPoisonAttack()
    {
        if (poisonGasPrefab)
        {
            // ʵ��������Ч������ʼ������
            GameObject poisonGas = Instantiate(poisonGasPrefab, transform.position, Quaternion.identity);
            poisonGas.GetComponent<PoisonGas>().Initialize(
                poisonGasDuration,
                poisonDamageInterval);
        }
    }
}