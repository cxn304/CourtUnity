using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

    //https://blog.csdn.net/ChinarCSDN/article/details/81058773 在ui界面显示物体
public class LogIn : MonoBehaviour
{
    private string userName;
    private string passWord;
    private string username;
    private string password;
    public InputField userInput;//登录面板输入用户名
    public InputField passInput;//登录面板输入密码
    private Button loginBtn;
    private Button homeButton;

    void Start()
    {
        username = "admin";//设置账号
        password = "admin";//设置密码
        loginBtn = GameObject.Find("loginButton").GetComponent<Button>();
        userInput = GameObject.Find("userInput").GetComponent<InputField>();
        passInput = GameObject.Find("passInput").GetComponent<InputField>();
        loginBtn.onClick.AddListener(LogInBtn);//监听LogInBtn函数
        homeButton = GameObject.Find("homeBt").GetComponent<Button>();
    }

    void LogInBtn()
    {
        //通过InputField获取账号和密码
        userName = userInput.text;
        passWord = passInput.text;
        
        //判断输入账号和密码是否与设置的账号密码一致
        if (username == userName && password == passWord)
        {
            GameObject can_vas1 = GameObject.Find("LoginCanvas");
            can_vas1.SetActive(false);  //防止影响到后面的canvas
            homeButton.transform.localPosition = homeButton.transform.localPosition + Vector3.right * 150;
            GameObject.Find("informationCanvas").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("date_time/Text").GetComponent<Text>().text = DateTime.Now.ToShortDateString().ToString();
        }   
    }

    private void OnEnable()
    {
        
    }
}
