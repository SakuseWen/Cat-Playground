using UnityEngine;

public class EnemyBullet : MonoBehaviour
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
            AudioManager.Instance.PlaySFX(4);
        }
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    // 碰到其它物体销毁自身
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 对玩家造成伤害
            PlayerBeHit health = collision.GetComponent<PlayerBeHit>();
            if (health != null)
            {
                health.TakeDamage();
            }
            Destroy(gameObject);

        }
        Destroy(gameObject);
    }
}
