using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadText : MonoBehaviour
{
    TextMesh text;
    MeshRenderer render;

    void Awake()
    {
        text= GetComponent<TextMesh>();
        render= GetComponent<MeshRenderer>();

        render.sortingLayerID = 0;
        render.sortingOrder = 100;
    }

    public void Show(int num)
    {
        text.text = num.ToString();

        StartCoroutine(CoFade(0.5f));
    }


    IEnumerator CoFade(float total)
    {
        float begin = Time.time;
        float end = Time.time + total;
        float y = transform.position.y;

        while (Time.time <= end)
        {
            float percent = (Time.time - begin) / total;
            transform.position = new Vector3(transform.position.x, y + 0.2f * percent, transform.position.z);

            Color color = render.material.color;
            color.a = (1 - percent);
            render.material.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }

}
