using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBeHit : MonoBehaviour
{
    public int maxHp = 5;

    int hp;

    FlashColor flashColor;
    AudioSource audioSource;

    public Image hpImage;

    void Start()
    {
        flashColor = GetComponent<FlashColor>();
        hp = maxHp;
    }

    void RefreshHpUI()
    {
        if (hpImage)
        {
            hpImage.fillAmount = (float)hp / maxHp;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            return;
        }
        if (flashColor)
        {
            flashColor.Flash(0.1f);
        }
        //角色受击音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(0);
        }

        hp -= 1;
        RefreshHpUI();

        // 角色死亡
        if (hp <= 0)
        {
            // 失败
            if (GameManager.I != null)
            {
                GameManager.I.Lose();
            }
            Destroy(gameObject);
        }
    }

    public void HealAllHp()
    {
        hp = maxHp;
        RefreshHpUI();
    }
}
