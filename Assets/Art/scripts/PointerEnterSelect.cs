/**********************************************************
 * PointerEnterSelect —— 悬停即选中；点击后一帧清除选中
 * 只需挂在 Button 上，无须额外 UtilityRunner
 **********************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PointerEnterSelect : MonoBehaviour,
                                   IPointerEnterHandler, IPointerClickHandler
{
    // 1️⃣ 鼠标悬停：把按钮设为 Selected，让蒙版立即跟随
    public void OnPointerEnter(PointerEventData e) =>
        EventSystem.current.SetSelectedGameObject(gameObject);

    // 2️⃣ 点击：在下一帧把 Selected 清空（蒙版立即隐藏）
    public void OnPointerClick(PointerEventData e)
    {
        // 协程挂在 EventSystem（一直激活），避免按钮失活时报错
        (EventSystem.current as MonoBehaviour)
            .StartCoroutine(ClearNextFrame());
    }

    IEnumerator ClearNextFrame()
    {
        yield return null;                              // 等一帧，让 onClick 逻辑跑完
        EventSystem.current.SetSelectedGameObject(null);
    }
}
