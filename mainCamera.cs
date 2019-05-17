using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//注释：     先CTRL+K，然后CTRL+C
//取消注释： 先CTRL+K，然后CTRL+U
namespace CourtScript
{
    public class mainCamera : MonoBehaviour
    {
        static public Vector3 camInitposition;    //主摄像机初始位置
        Vector3 camInitRo;
        GameObject zCamera;   //主摄像机
        Vector3 zhuanzhou_postion;//中心轴位置
        float rotationY;

        //方向灵敏度  
        float sensitivityX = 10F;
        float sensitivityY = 10F;

        //布尔鼠标按键
        bool isRotating;
        bool isSee;
        bool isMove;
        bool isLRMove;

        float dir_v; //摄像机前后移动速度
        float ro_v; //摄像机左右移动速度
        public float limitDistance = 250f;

        //--------------------------------------------------------------------------------------------------------------------------//
        void Start()
        {
            zCamera = GameObject.Find("Camera");
            camInitposition = zCamera.transform.position;
            camInitRo = zCamera.transform.eulerAngles;
            zhuanzhou_postion = GameObject.Find("zhuanzhou").transform.position; // 获取转轴的三围坐标
            rotationY = -transform.localEulerAngles.x;
        }
        //--------------------------------------------------------------------------------------------------------------------------//

        void Update()
        {
            ScrollView();
            ControlCameraMove();
        }
        //-----------------------------------------------------------------------------------------------------------------//

        void cameraRotate(Vector3 centerPosition)    //摄像机围绕目标旋转,一段时间无操作后
        {
            transform.RotateAround(centerPosition, Vector3.up, 15 * Time.deltaTime);
        }


        void CamRotateByClick(Vector3 zhuanzhou_postion) //限制转的角度
        {
            Vector3 ca_floor = new Vector3(transform.position.x, zhuanzhou_postion.y, transform.position.z);
            var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
            var mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴移动
            float angleLow = Mathf.Clamp(mouse_y, 0f, 0.6f);
            float angleHigh = Mathf.Clamp(mouse_y, -0.6f, 0f);
            float angle = Vector3.Angle(transform.position - zhuanzhou_postion, ca_floor - zhuanzhou_postion);
            if (transform.position.y > 10 && angle < 60)
            {
                //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                transform.RotateAround(zhuanzhou_postion, Vector3.up, mouse_x * 5);
                transform.RotateAround(zhuanzhou_postion, transform.right, mouse_y * 5);
            }
            else if (transform.position.y <= 10)
            {
                transform.RotateAround(zhuanzhou_postion, Vector3.up, mouse_x * 5);
                transform.RotateAround(zhuanzhou_postion, transform.right, angleLow * 5);
                //transform.position = new Vector3(transform.position.x, 0.001f, transform.position.z);
            }
            else if (angle >= 60)
            {
                transform.RotateAround(zhuanzhou_postion, Vector3.up, mouse_x * 5);
                transform.RotateAround(zhuanzhou_postion, transform.right, angleHigh * 5);
            }
        }

        //旋转视野(以摄像机为第一视角)
        void sbRotate()
        {
            //根据鼠标移动的快慢(增量), 获得相机左右旋转的角度(处理X)  
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            //总体设置一下相机角度  
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            //根据鼠标移动的快慢(增量), 获得相机上下旋转的角度(处理Y)  
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            //角度限制. rotationY小于min,返回min. 大于max,返回max. 否则返回value，Clamp函数的方法   
            rotationY = Mathf.Clamp(rotationY, -50f, 60f);
        }

        void CameraMove(float v) //向前后移动
        {
            Ray ray = new Ray(zhuanzhou_postion, transform.position - zhuanzhou_postion);
            Vector3 newPosition = ray.GetPoint(limitDistance - 0.1f); //限制住移动距离
            float limitDis = (transform.position - zhuanzhou_postion).magnitude;
            if (limitDis <= limitDistance)   //限制摄像机范围
            {
                transform.Translate(Vector3.forward * Time.deltaTime * v, Space.Self); //摄像机移动
            }
            else transform.position = newPosition;
        }

        void ControlCameraMove()
        {
            //鼠标右键按下可以围绕目标旋转
            if (Input.GetMouseButtonDown(1)) isRotating = true;
            if (Input.GetMouseButtonUp(1)) isRotating = false;
            if (isRotating) CamRotateByClick(zhuanzhou_postion);

            //鼠标中键按下可以旋转视野
            if (Input.GetMouseButtonDown(2)) isSee = true;
            if (Input.GetMouseButtonUp(2)) isSee = false;
            if (isSee) sbRotate();

            //摄像机移动--WASD
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isMove = true;
                dir_v = 20.0f;
            }
            if (Input.GetKeyUp(KeyCode.UpArrow)) isMove = false;
            if (isMove) CameraMove(dir_v);

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                isMove = true;
                dir_v = -20.0f;
            }
            if (Input.GetKeyUp(KeyCode.DownArrow)) isMove = false;
            if (isMove) CameraMove(dir_v);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isLRMove = true;
                ro_v = 20.0f;
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow)) isLRMove = false;
            if (isLRMove) MoveLR(ro_v);

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                isLRMove = true;
                ro_v = -20.0f;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow)) isLRMove = false;
            if (isLRMove) MoveLR(ro_v);
        }

        void MoveLR(float v)   //向左右移动
        {
            Ray ray = new Ray(zhuanzhou_postion, transform.position - zhuanzhou_postion);
            Vector3 newPosition = ray.GetPoint(limitDistance - 0.1f);
            float limitDis = (transform.position - zhuanzhou_postion).magnitude;
            if (limitDis <= limitDistance)
            {
                transform.Translate(Vector3.left * Time.deltaTime * v, Space.Self);
            }
            else transform.position = newPosition;
        }

        void ScrollView()   //鼠标放大缩小
        {
            Ray ray = new Ray(zhuanzhou_postion, transform.position - zhuanzhou_postion);
            Vector3 newPosition = ray.GetPoint(limitDistance - 0.1f);
            float limitDis = (transform.position - zhuanzhou_postion).magnitude;
            if (limitDis <= limitDistance)
            {
                float ScrollSpeed = Input.GetAxis("Mouse ScrollWheel") * 50f;
                transform.Translate(Vector3.forward * ScrollSpeed, Space.Self);
            }
            else transform.position = newPosition;
        }

        public void MainCameraToInitPositon()
        {
            zCamera.transform.position = Vector3.MoveTowards(zCamera.transform.position, camInitposition, Time.deltaTime * limitDistance);
            zCamera.transform.LookAt(zhuanzhou_postion);
            //zCamera.transform.rotation = Quaternion.RotateTowards(zCamera.transform.rotation,Quaternion.Euler(camInitRo), 1f);
        }
    }
}
