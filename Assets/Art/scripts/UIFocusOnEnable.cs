/******************************************************
 * UIFocusOnEnable.cs —— 面板激活时把焦点放到首个按钮
 ******************************************************/
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocusOnEnable : MonoBehaviour
{
    [Tooltip("面板打开时默认选中的按钮")]
    public GameObject firstSelected;

    void OnEnable()
    {
        if (firstSelected)
            EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    // 可选：面板关闭时清焦点，防止指向隐藏对象
    void OnDisable()
    {
      EventSystem.current.SetSelectedGameObject(null);
    }
}
