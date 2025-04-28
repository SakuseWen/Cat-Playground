/******************************************************
 * UIFocusOnEnable.cs ���� ��弤��ʱ�ѽ���ŵ��׸���ť
 ******************************************************/
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocusOnEnable : MonoBehaviour
{
    [Tooltip("����ʱĬ��ѡ�еİ�ť")]
    public GameObject firstSelected;

    void OnEnable()
    {
        if (firstSelected)
            EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    // ��ѡ�����ر�ʱ�役�㣬��ָֹ�����ض���
    void OnDisable()
    {
      EventSystem.current.SetSelectedGameObject(null);
    }
}
