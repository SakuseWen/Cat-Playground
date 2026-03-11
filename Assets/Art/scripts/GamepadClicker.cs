using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamepadClicker : MonoBehaviour
{
    void Update()
    {
        // 按下 PS5 手柄的 ✖ Cross（JoystickButton1）
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            // 获取当前选中的 UI 元素
            var go = EventSystem.current.currentSelectedGameObject;
            if (go == null) return;

            // 如果它有 Button 组件，就调用一次 onClick
            var btn = go.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.Invoke();
                return;
            }

            // 如果它有其他可点击的行为，也可以像下面这样手动派发 PointerClick 事件：
            ExecuteEvents.Execute(go,
                new PointerEventData(EventSystem.current),
                ExecuteEvents.pointerClickHandler);
        }
    }
}
