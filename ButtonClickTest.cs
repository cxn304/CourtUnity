using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

//此脚本需挂载到BtnParent父对象上，此脚本需挂载到BtnParent父对象上
public class ButtonClickTest : MonoBehaviour
{
    //所有Button的父节点
    
    private bool isActive;
    public Transform btnParent;
    private Button[] btns;
    private Transform[] father;    //所有按钮对象的集合,不论有多少个都可以
    private GameObject ButtonHome;  //home按键
    private GameObject ButtonF3;
    private GameObject ButtonF1;
    private GameObject ButtonF2;
    CanvasGroup can_gro;

    //--------------------------------------------------------------------------------------------------------------------------//
    void Start()
    {
        father = GetComponentsInChildren<Transform>();  //寻找父对象下所有子对象（包括孙对象和父对象本身）
        GameObject can_vas = GameObject.Find("Canvas2");
        can_gro = can_vas.transform.GetComponentInParent<CanvasGroup>();    //找到Canvas2的CanvasGroup
        can_gro.alpha = 0f;
        ButtonHome = GameObject.Find("ButtonHome");

        //初始化数组长度,这个是对按钮本身的赋值
        btns = new Button[btnParent.childCount];
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i] = btnParent.GetChild(i).GetComponent<Button>();
        }
        ButtonHome.GetComponent<Button>().onClick.AddListener(HomeClick);   //为按钮添加点击事件
    }

    //--------------------------------------------------------------------------------------------------------------------------//
    void Update()
    {
        
    }

    //--------------------------------------------------------------------------------------------------------------------------//
    public void HomeClick()
    {
        //choosed = EventSystem.current.currentSelectedGameObject;
        //isActive = choosed.activeInHierarchy;
        //string btnName = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name; //按下时的按钮名字

        //按下时 启动button home事件,对按钮移位进行处理
        foreach (Transform child in father)
        {
            if (child.name != "ButtonHome" && child.name != "BtnParent" && child.name != "Text")
                if (child.position.y < 500) child.Translate(Vector3.up * 300);
                else child.Translate(Vector3.up * -300);
            Debug.Log(child.position.y);
        }
    }
}
