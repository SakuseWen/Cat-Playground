/********************************************************
 * PanelSwitcher.cs —— 终版（保证高亮不会丢）
 ********************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PanelSwitcher : MonoBehaviour
{
    [Header("面板对象")]
    public GameObject background;       // 有 Start / Help / Quit
    public GameObject info;             // 有 Close / Prev / Next

    [Header("首选按钮")]
    public GameObject backgroundFirst;  // StartButton
    public GameObject infoFirst;        // CloseButton

    /* ---------- Help 按钮 ---------- */
    public void OpenInfo()
    {
        background.SetActive(false);
        info.SetActive(true);
        StartCoroutine(SelectNextFrame(infoFirst));
    }

    /* ---------- Close 按钮 ---------- */
    public void CloseInfo()
    {
        info.SetActive(false);
        background.SetActive(true);
        StartCoroutine(SelectNextFrame(backgroundFirst));
    }

    /* ---------- 共用协程：等一帧再选中 ---------- */
    IEnumerator SelectNextFrame(GameObject go)
    {
        yield return null;                         // 等 PointerEnterSelect 清空
        EventSystem.current.SetSelectedGameObject(go);
    }
}
