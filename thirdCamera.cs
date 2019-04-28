using System;
using UnityEngine;


// 挂到第三人称摄像机上---------------------------------------------------------------------------------------------------------
public class thirdCamera : MonoBehaviour
{
    float rotationY = 0f;
    Vector3 relCameraPos;   //摄像机与模型相对朝向和大小的向量
    public Transform m_player;  //机器人对象(静态)
    bool isDisan;
    //滚轮缩放速度
    private float wheelSpeed = 6;
    private float Distance;
    public Vector3 offsetPosition; //滚轮位置偏移
    public Vector3 InitCamDis; //一开始相机位置
    //---------------------------------------------------------------------------------------------------------

    void Start()
    {
        m_player = GameObject.Find("Robot Kyle").transform;     //定义机器人(如果是其他物体要改变名字)
        Vector3 m_position = m_player.position;   //玩家的位置
        offsetPosition = this.transform.position - m_position;
        InitCamDis = offsetPosition;
    }
    //---------------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        //CameraRay();
        //摄像机转移到第三人称
        if (Input.GetKeyDown(KeyCode.G))
            isDisan = true;
        if (Input.GetKeyUp(KeyCode.G))
            isDisan = false;
        if (isDisan)
            sbRotate();
    }
    //---------------------------------------------------------------------------------------------------------

    public void camThird() //第三人称视角旋转
    {
        Vector3 m_position = m_player.position;   //玩家的位置
        relCameraPos = this.transform.position - m_player.position;  //摄像机与模型相对朝向和大小的向量
        Ray cray = new Ray(m_position, relCameraPos);
        Vector3 thePoint = cray.GetPoint(50);  //摄像机与人物面对的点
        camRotateFromPlayer(); //相机围绕目标旋转
        relCameraPos = this.transform.position - m_player.position;
    }


    public void camRotateFromPlayer() //第三人称视角旋转
    {
        var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
        var mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴移动
        this.transform.Translate(Vector3.left * (mouse_x * 15f) * Time.deltaTime);
        this.transform.Translate(Vector3.up * (mouse_y * 15f) * Time.deltaTime);
        this.transform.RotateAround(m_player.position, Vector3.up, mouse_x * 5);
        this.transform.RotateAround(m_player.position, this.transform.right, mouse_y * 5);
    }


    private void ScrollView()   //鼠标放大缩小
    {
        if (offsetPosition.magnitude < 15f && offsetPosition.magnitude > 5f)
        {
            Distance = Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
            transform.Translate(Vector3.forward * Distance, Space.Self);
            offsetPosition = transform.position - m_player.position;
        }
        else if (offsetPosition.magnitude >= 15f)
        {
            transform.Translate(Vector3.forward * -Distance, Space.Self);
            offsetPosition = transform.position - m_player.position;
        }
        else if (offsetPosition.magnitude <= 5f)
        {
            transform.Translate(Vector3.forward * -Distance, Space.Self);
            offsetPosition = transform.position - m_player.position;
        }
    }

    private void sbRotate()
    {
        //根据鼠标移动的快慢(增量), 获得相机左右旋转的角度(处理X)  
        float rotationX = m_player.localEulerAngles.y + Input.GetAxis("Mouse X") * 10f;
        //根据鼠标移动的快慢(增量), 获得相机上下旋转的角度(处理Y)  
        rotationY += Input.GetAxis("Mouse Y") * 10f;
        //角度限制. rotationY小于min,返回min. 大于max,返回max. 否则返回value，Clamp函数的方法   
        rotationY = Mathf.Clamp(rotationY, -30f, 60f);
        //总体设置一下相机角度  
        m_player.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }

    private void CameraRay()   //防止相机穿墙(这个有问题!!)
    {
        //第一个参数放的是发射射线的物体的位置，第二个参数放的是发射射线的方向
        Ray ray = new Ray(m_player.position, transform.position - m_player.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            string name = hit.collider.name;
            if (name != "HumanoidWalk")
            {
                transform.position = m_player.position;
                transform.Translate(Vector3.forward * -3f, Space.Self);
                transform.Translate(Vector3.up * 2.5f, Space.Self);
                Debug.Log(name);
            }
            else if (name == "HumanoidWalk")
            {
                transform.position = m_player.position;
                transform.Translate(Vector3.forward * -16f, Space.Self);
                transform.Translate(Vector3.up * 2.5f, Space.Self);
                Debug.Log(name);
            }
        }
    }
}
