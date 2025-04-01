using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 闪红光的效果
// 原理：用协程，慢慢改变精灵渲染器SpriteRender的颜色
public class FlashColor : MonoBehaviour
{
    SpriteRenderer render;
    public Color flashColor = new Color(1, 0, 0, 1);

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void Flash(float time)
    {
        StopAllCoroutines();
        StartCoroutine(CoFlash(time));
    }

    IEnumerator CoFlash(float time)
    {
        float beg = Time.time;
        while (Time.time < beg + time)
        {
            float percent = (Time.time - beg) / time;
            Color c = Color.Lerp(Color.white, flashColor, percent);

            render.color = c;
            yield return null;
        }

        beg = Time.time;
        time /= 2;
        while (Time.time < beg + time)
        {
            float percent = (Time.time - beg) / time;
            Color c = Color.Lerp(flashColor, Color.white, percent);

            render.color = c;
            yield return null;
        }
    }
}
