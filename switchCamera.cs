using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//build时，Default Is Native Resolution 不勾选 即可自定义分辨率
public class switchCamera : MonoBehaviour
{
    private GameObject camera0;
    private GameObject camera1;
    // 随便挂在一个物体上,不会消失的那种
    void Start()
    {
        camera0 = GameObject.Find("Camera");    //主摄像机
        camera1 = GameObject.Find("CameraFollow");
        camera1.SetActive(false);
    }

    // 切换摄像机，设置相机能否用
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            camera0.SetActive(true);
            camera1.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            camera0.SetActive(true);
            camera1.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            camera0.SetActive(true);
            camera1.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            camera0.SetActive(true);
            camera1.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            camera0.SetActive(true);
            camera1.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            camera0.SetActive(true);
            camera1.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            camera0.SetActive(false);
            camera1.SetActive(true);
        }
    }
}
