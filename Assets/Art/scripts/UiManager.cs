/********************************************************
 * PanelSwitcher.cs ���� �հ棨��֤�������ᶪ��
 ********************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PanelSwitcher : MonoBehaviour
{
    [Header("������")]
    public GameObject background;       // �� Start / Help / Quit
    public GameObject info;             // �� Close / Prev / Next

    [Header("��ѡ��ť")]
    public GameObject backgroundFirst;  // StartButton
    public GameObject infoFirst;        // CloseButton

    /* ---------- Help ��ť ---------- */
    public void OpenInfo()
    {
        background.SetActive(false);
        info.SetActive(true);
        StartCoroutine(SelectNextFrame(infoFirst));
    }

    /* ---------- Close ��ť ---------- */
    public void CloseInfo()
    {
        info.SetActive(false);
        background.SetActive(true);
        StartCoroutine(SelectNextFrame(backgroundFirst));
    }

    /* ---------- ����Э�̣���һ֡��ѡ�� ---------- */
    IEnumerator SelectNextFrame(GameObject go)
    {
        yield return null;                         // �� PointerEnterSelect ���
        EventSystem.current.SetSelectedGameObject(go);
    }
}
