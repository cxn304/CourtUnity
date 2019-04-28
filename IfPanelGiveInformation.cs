using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
