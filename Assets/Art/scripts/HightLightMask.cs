using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public class GlobalHighlightCursor : MonoBehaviour
{
    RectTransform rt;        // 蒙版 Rect
    CanvasGroup cg;        // 用于显隐
    Image img;       // 蒙版 Image

    [Header("边距 / 缩放（二选一）")]
    public Vector2 padding = Vector2.zero; // 正数 = 放大
    [Range(0.5f, 2f)] public float scale = 1f;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        img = GetComponent<Image>();

        cg.alpha = 0f;  // 初始隐藏
        cg.blocksRaycasts = false;
    }

    void LateUpdate()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        if (!go) { cg.alpha = 0; return; }

        Image btnImg = go.GetComponent<Image>();     // 按钮的 Image
        if (!btnImg) { cg.alpha = 0; return; }

        // 1️⃣ 同步 sprite & Image 设置
        img.sprite = btnImg.sprite;
        img.type = btnImg.type;            // 支持 9-slice
        img.pixelsPerUnitMultiplier = btnImg.pixelsPerUnitMultiplier;
        img.preserveAspect = true;                   // 防止拉伸

        // 2️⃣ 位置 / 尺寸
        RectTransform target = btnImg.rectTransform;
        rt.position = target.position;
        rt.sizeDelta = target.sizeDelta * scale + padding;

        // 3️⃣ 保证渲染在最上层
        rt.SetAsLastSibling();

        cg.alpha = 1f;
    }
}
