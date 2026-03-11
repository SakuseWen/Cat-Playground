/*******************************************************
 * PlayerBeHit.cs —— HP / 受击
 *******************************************************/
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBeHit : MonoBehaviour
{
    public int maxHp = 5;
    [HideInInspector] public Image hpImage;

    int hp;
    FlashColor flash;

    void Start()
    {
        flash = GetComponent<FlashColor>();
        hp = maxHp;
        RefreshHpUI();
    }

    public void RefreshHpUI()
    {
        if (hpImage) hpImage.fillAmount = (float)hp / maxHp;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

        flash?.Flash(0.1f);
        AudioManager.Instance?.PlaySFX(0);

        hp--;
        RefreshHpUI();

        if (hp <= 0)
        {
            GameManager.I?.Lose();
            Destroy(gameObject);
        }
    }

    public void HealAllHp()
    {
        hp = maxHp;
        RefreshHpUI();
    }

    internal void TakeDamage()
    {
        hp--;
        RefreshHpUI();

        if (hp <= 0)
        {
            GameManager.I?.Lose();
            Destroy(gameObject);
        }
    }
}
