using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using cakeslice;

namespace CourtScript
{
    //此函数主要对各面板内容的操作
    public class StartB : MonoBehaviour
    {
        GameObject plane;   //切割面
        static public int changeCanvas = 0;  //用于不让BarCanvasSimple一开始就动(初始化button的控制值)
        Vector3 RcanvasTransferPosition;
        static public Vector3 RcanvasinitPosition;
        Vector3 LcanvasTransferPosition;
        Vector3 LcanvasinitPosition;
        bool isLeftMove;
        Button homeButton;
        Button exitButton;
        GameObject MoveRightPanel;  //点击左面板传回来的需要移动的右面板
        GameObject Right_Canvas;    //右面板
        GameObject Left_Canvas;
        GameObject cutFace;
        
        //--------------------------------------------------------------------------------------------------------------------------//
        void Start()
        {
            //找左面版（大）
            Left_Canvas = GameObject.Find("LeftCanvas");   //找到左面板，这个是为了移动左面板
            LcanvasinitPosition = Left_Canvas.transform.localPosition;
            LcanvasTransferPosition = Left_Canvas.transform.localPosition + Vector3.right * 1080;

            plane = GameObject.Find("Quad");
            Right_Canvas = GameObject.Find("RightCanvasF");   //找到右面板集合
            RcanvasinitPosition = Right_Canvas.transform.GetChild(0).localPosition; //右面板初始位置btnAsset.
            RcanvasTransferPosition = RcanvasinitPosition + Vector3.left * 840;     //右面板移动至位置

            //监听切分层按钮，寻找其名字
            cutFace = GameObject.Find("CutFace");
            foreach (Transform child in cutFace.transform)
            {
                child.GetComponent<Button>().onClick.AddListener(
                    delegate ()
                    {
                        this.CutFloor(child.name);
                    });
            }

            //home按键添加监听
            homeButton = GameObject.Find("homeBt").GetComponent<Button>();
            homeButton.onClick.AddListener(HomeBtUse);

            //exit按键添加监听
            exitButton = GameObject.Find("btnExit").GetComponent<Button>();
            exitButton.onClick.AddListener(GameExit);

            //遍历所有能影响右面板的button，并设置监听，要求fristPanel及secPanel里面的物体都有button
            foreach (Transform child in Left_Canvas.transform)
            {
                if (child.name == "fristPanel" || child.name == "secPanel")
                    foreach (Transform btnchild in child)
                    {
                        btnchild.GetComponent<Button>().onClick.AddListener(
                            delegate ()                                     //此处若不用委托不能传递btnchild.name进去
                            {
                                MoveRightPanel = this.SwitchPanels(btnchild.name);
                            });
                    }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------//
        void Update()
        {
            if (changeCanvas == 1)
            {
                MoveLeftCanvas(isLeftMove);
                
            }
            else if (changeCanvas == 2) MoveRightCanvas(MoveRightPanel);
            else if (changeCanvas == 3)     //3表示移动摄像机至初始位置
            {
                switchCamera.camera0.GetComponent<mainCamera>().MainCameraToInitPositon();
                if (switchCamera.camera0.transform.position == mainCamera.camInitposition) changeCanvas = 0;
                
            }      
        }
        //--------------------------------------------------------------------------------------------------------------------------//

        public void GameExit()
        {
            Application.Quit();
        }


        IEnumerator SplitWallTime1() //直到移动到位置后开始协程
        {
            GameObject court = GameObject.Find("court");
            int b = 0;
            float destination = 100f;
            float speed = 50f;
            foreach (Transform child in court.transform)
            {
                b++;
                while (child.position != new Vector3(child.position.x, destination, child.position.z))
                {
                    child.position = Vector3.MoveTowards(child.position, new Vector3(child.position.x, destination, child.position.z), speed * Time.deltaTime);
                    yield return 0; //注意等待时间的写法
                }
                //destination -= 20f;
                if (b == 6)
                {
                    yield return StartCoroutine(WalltoInit());   //可以在IEnumerator中yield return其他的协程。
                    break;
                }
            }     
        }

        IEnumerator SplitWallTime2() //过几秒开始协程???????????????????????????????????
        {
            GameObject court = GameObject.Find("court");
            int b = 0;
            foreach (Transform child in court.transform)
            {
                b++;
                float atime = 0f;
                while (true)
                {
                    atime += Time.deltaTime;
                    child.position = Vector3.MoveTowards(child.position, new Vector3(child.position.x, 100, child.position.z), 50 * Time.deltaTime);
                    if (atime > 1.8f) break;
                    yield return 0;
                }
                yield return StartCoroutine(MyWait(1f)); //注意等待时间的写法
                if (b == 6) break;
            }
        }

        public IEnumerator MyWait(float duration)  //等待时间
        {
            for (float timer = 0; timer < duration; timer += Time.deltaTime)
                yield return 0;
        }

        IEnumerator WalltoInit()    //墙回原位
        {
            GameObject court = GameObject.Find("court");
            int b = 0;
            foreach (Transform child in court.transform)
            {
                b++;
                while (child.position != new Vector3(child.position.x, 0, child.position.z))
                {
                    child.position = Vector3.MoveTowards(child.position, new Vector3(child.position.x, 0, child.position.z), 50 * Time.deltaTime);
                    yield return 0; //注意等待时间的写法
                }
                if (b == 6) break;
            }
        }


        void CutFloor(string fname)  //切剖面
        {
            Vector3[] floorHigh = new Vector3[4];
            floorHigh[0] = new Vector3(0, 10f, 0);
            floorHigh[1] = new Vector3(0, 40f, 0);
            floorHigh[2] = new Vector3(0, 70f, 0);
            floorHigh[3] = new Vector3(0, 200f, 0);
            switch (fname)
            {
                case "thirdlevelButton":
                    plane.transform.position = floorHigh[2];
                    ObjectRend("court");
                    changeCanvas = 3;
                    break;
                case "secondlevelButton":
                    plane.transform.position = floorHigh[1];
                    ObjectRend("court");
                    changeCanvas = 3;
                    break;
                case "firstlevelButton":
                    plane.transform.position = floorHigh[0];
                    ObjectRend("court");
                    changeCanvas = 3;
                    break;
                case "ReturnFullModel":
                    plane.transform.position = floorHigh[3];
                    ObjectRend("court");
                    changeCanvas = 3;
                    GameObject.Find("Camera").GetComponent<OutlineEffect>().enabled = false;
                    GameObject.Find("Camera").GetComponent<OutlineAnimation>().enabled = false;
                    break;
                case "tubeButton":
                    ObjectRend("court");
                    changeCanvas = 3;
                    GameObject.Find("Camera").GetComponent<OutlineEffect>().enabled = true;
                    GameObject.Find("Camera").GetComponent<OutlineAnimation>().enabled = true;
                    break;
            }
        }

        void HomeBtUse()
        {
            switchCamera.isCubeFade = false;
            changeCanvas = 1;
            isLeftMove = !isLeftMove;
            StartCoroutine(SplitWallTime1());    //启动协程，瞬发
        }

        float canvasMoveSpeed = 2600;
        void MoveLeftCanvas(bool aa)   //使左面板移动
        {
            if (aa) Left_Canvas.transform.localPosition = Vector3.MoveTowards(Left_Canvas.transform.localPosition, LcanvasTransferPosition, Time.deltaTime * canvasMoveSpeed);
            else Left_Canvas.transform.localPosition = Vector3.MoveTowards(Left_Canvas.transform.localPosition, LcanvasinitPosition, Time.deltaTime * canvasMoveSpeed);
        }
        
        void MoveRightCanvas(GameObject Rcanvas) //使右面板移动,始终是从外往里移动，因为移出去直接都是瞬发。这里需使诸多面板中的一个移动
        {
            Rcanvas.transform.localPosition = Vector3.MoveTowards(Rcanvas.transform.localPosition, RcanvasTransferPosition, Time.deltaTime * canvasMoveSpeed);
        }

        void ObjectRend(string name)    //启动渲染
        {
            Vector3 normal = plane.transform.TransformVector(new Vector3(0, 0, -1));    //把自己负向的向量映射到世界坐标系(????)
            Vector3 position = plane.transform.position;
            Transform[] grandFa = GameObject.Find(name).GetComponentsInChildren<Transform>();   //父物体下所有子物体的集合
            Renderer rend;
            foreach (Transform child in grandFa)
            {
                if (child.name == name) continue;
                rend = child.GetComponent<Renderer>();
                rend.material.SetVector("_PlaneNormal", normal);    //这两个映射到OnePlaneBSP这个shader中,世界坐标系
                rend.material.SetVector("_PlanePosition", position);    //这两个映射到OnePlaneBSP这个shader中,世界坐标系
            }
        }

        //返回一个选中的canvas
        GameObject SwitchPanels(string Cname) //Cname既表示button的name也表示对应的Canvas的name
        {
            GameObject fgameObject = GameObject.Find("canvasGroups/RightCanvasF/btnElec");      //初始化fgameObject
            foreach (Transform child in Right_Canvas.transform) //这里的Right_Canvas为右面板集合
            {
                if (child.name == Cname)
                {
                    //child.gameObject.SetActive(true);
                    changeCanvas = 2;  //表示启动右面板移动动画
                    fgameObject = child.gameObject;
                }
                else
                {
                    child.localPosition = RcanvasinitPosition;   //其余的回归原位
                                                                 //child.gameObject.SetActive(false);
                }
            }
            return fgameObject;
        }

        public void MainCamRoPo(out Vector3 ro, out Vector3 po)
        {
            GameObject camera = switchCamera.camera0;
            po = camera.transform.position;
            ro = camera.transform.eulerAngles;
            camera.GetComponent<Camera>().depth = 1;
            switchCamera.camera1.GetComponent<Camera>().depth = 0;
        }
    }
}