using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStart : MonoBehaviour
{
    CanvasGroup can_gro2;
    private GameObject buttonStart;  //start按键
    //--------------------------------------------------------------------------------------------------------------------------//
    void Start()
    {
        buttonStart = GameObject.Find("loginButton");
        buttonStart.GetComponent<Button>().onClick.AddListener(StartClick);   //为按钮添加点击事件
    }

    //--------------------------------------------------------------------------------------------------------------------------//
    void Update()
    {
        
    }

    //--------------------------------------------------------------------------------------------------------------------------//
    public void StartClick()
    {
        GameObject can_vas2 = GameObject.Find("RightCanvasF");
        can_gro2 = can_vas2.transform.GetComponentInParent<CanvasGroup>();    //找到Canvas2的CanvasGroup
        can_gro2.alpha = 1f;

        GameObject can_vas1 = GameObject.Find("LoginCanvas");
        can_vas1.SetActive(false);  //防止影响到后面的canvas
    }
}
