using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class IfPanelGiveInformation : MonoBehaviour
{
    public GameObject informationPanel;
    bool isShowTip;

    // Start is called before the first frame update
    void Start()
    {
        informationPanel = GameObject.Find("informationPanel");
        isShowTip = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckGuiRaycastObjects()) return;
        ShowInformation();
    }

    void OnMouseEnter()
    {
        isShowTip = true;
    }

    void OnMouseExit()
    {
        isShowTip = false;
    }

    void ShowInformation()
    {
        if (isShowTip)
        {
            Vector3 mousePos = Input.mousePosition;
            informationPanel.transform.position = mousePos;
        }
        else
            informationPanel.transform.position = new Vector3(2000, 2000, 0);
    }

    bool CheckGuiRaycastObjects()   //判断当前鼠标上重叠的UI有多少个
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);  //返回射线共碰了几次UI

        return results.Count > 0;
    }
}
