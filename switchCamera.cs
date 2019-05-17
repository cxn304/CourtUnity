using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.UI;

//build时，Default Is Native Resolution 不勾选 即可自定义分辨率
// 与startB这个脚本挂在同一个物体上
namespace CourtScript
{
    public class switchCamera : MonoBehaviour
    {
        static public GameObject camera0;
        static public GameObject camera1;

        void Start()
        {
            camera0 = GameObject.Find("Camera");    //主摄像机
            camera1 = GameObject.Find("CameraFollow");  //第三人称摄像机
            camera1.SetActive(false);
            camera0.GetComponent<OutlineEffect>().enabled = false;
            camera0.GetComponent<OutlineAnimation>().enabled = false;
            MonitorAreaColor();
        }

        // 切换摄像机，设置相机能否用
        void Update()
        {
            ChangeCamera();
        }

        void ChangeCamera()
        {
            if (Input.GetMouseButtonDown(1))
            {
                camera0.SetActive(true);
                camera1.SetActive(false);
            }
            if (Input.GetMouseButtonDown(2))
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

        IEnumerator CameraLookAtArea(string aname)
        {
            StartB.changeCanvas = 3;    //表示要进行区域的注释操作，这样会移动主摄像机。赋值changeCanvas让startB中的CutFloor不启动相机移动
            GameObject choosed = GameObject.Find(aname);
            Ray ray = new Ray(choosed.transform.position, new Vector3(0, 1, -0.5f));
            Vector3 newPosition = ray.GetPoint(50f);
            GetComponent<StartB>().CutFloor("firstlevelButton");
            while (camera0.transform.position != newPosition)
            {
                camera0.transform.position = Vector3.MoveTowards(camera0.transform.position, newPosition, Time.deltaTime * 250f);
                camera0.transform.LookAt(choosed.transform.position);
                yield return 0;
            }        
        }

        float ColorAlpha = 0f;   //cube一开始的透明度,全局变量
        Color[] cubeColor = new Color[6];
        public Color SetCubeColor(int i)
        {
            cubeColor[0] = new Color(1, 0, 0, 0);
            cubeColor[1] = new Color(0, 1, 0, 0);
            cubeColor[2] = new Color(0, 0, 1, 0);
            cubeColor[3] = new Color(1, 0, 1, 0);
            cubeColor[4] = new Color(1, 1, 0, 0);
            cubeColor[5] = new Color(0, 1, 1, 0);
            return cubeColor[i];
        }

        bool isCubeFadeIn;    //控制颜色区域透明度是否变化
        static public bool isCubeFade;
        Material cubeMatrial;   //控制区域的材质透明度的值,只在协程中使用
        IEnumerator SwitchAreaFade(string cubename)          //只显示一个区域的协程（目前没用上）
        {
            StartCoroutine(CameraLookAtArea("maskCubes/areakw (3)"));
            isCubeFade = true;
            cubeMatrial = GameObject.Find("maskCubes/areazw").GetComponent<Renderer>().material;    //初始赋值
            foreach (Transform child in maskCubes.transform)
            {
                Material tempMat = child.gameObject.GetComponent<Renderer>().material;              //cubes一开始的颜色
                Color newColors = new Color(tempMat.color.r, tempMat.color.g, tempMat.color.b, 0.5f);   //每次调用按钮前初始化透明度
                tempMat.SetColor("_Color", newColors);
                if (child.name == cubename)
                {
                    cubeMatrial = child.gameObject.GetComponent<Renderer>().material;
                }    
            }
            while (isCubeFade)
            {
                ColorAlpha += (isCubeFadeIn ? 1 : -1) * Time.deltaTime / 3;
                Color newColor = new Color(cubeMatrial.color.r, cubeMatrial.color.g, cubeMatrial.color.b, ColorAlpha);  //放到循环里面
                cubeMatrial.SetColor("_Color", newColor);
                if (ColorAlpha > 0.5f) isCubeFadeIn = false;    //此处要注意，数字要加上f表示与ColorAlpha的类型一致
                if (ColorAlpha < 0f) isCubeFadeIn = true;
                yield return 0;
                //if(Input.GetButtonDown("")) StopCoroutine("SwitchAreaFade");  //用于判断是否按下按钮
                if (Input.GetMouseButtonDown(0)) StopCoroutine("SwitchAreaFade");   //点击左键后,循环中下个yield后终止此协程
            }   
        }

        IEnumerator ParkingCubeArea()            //显示若干个停车位的协程
        {
            StartCoroutine(CameraLookAtArea("maskCubes/areakw (3)"));
            isCubeFade = true;
            cubeMatrial = GameObject.Find("maskCubes/areazw").GetComponent<Renderer>().material;    //初始赋值
            foreach (Transform child in maskCubes.transform)
            {
                Material tempMat = child.gameObject.GetComponent<Renderer>().material;              //cubes一开始的颜色
                Color newColors = new Color(tempMat.color.r, tempMat.color.g, tempMat.color.b, 0.5f);   //每次调用按钮前初始化透明度
                tempMat.SetColor("_Color", newColors);
            }
            while (isCubeFade)
            {
                ColorAlpha += (isCubeFadeIn ? 1 : -1) * Time.deltaTime / 3;
                foreach (Transform child in maskCubes.transform)
                {
                    cubeMatrial= child.gameObject.GetComponent<Renderer>().material;
                    Color newColor = new Color(cubeMatrial.color.r, cubeMatrial.color.g, cubeMatrial.color.b, ColorAlpha);
                    cubeMatrial.SetColor("_Color", newColor);
                }
                if (ColorAlpha > 0.5f) isCubeFadeIn = false;    //此处要注意，数字要加上f表示与ColorAlpha的类型一致
                if (ColorAlpha < 0f) isCubeFadeIn = true;
                yield return 0;
                //if(Input.GetButtonDown("")) StopCoroutine("ParkingCubeArea");  //用于判断是否按下按钮
                if (Input.GetMouseButtonDown(0)) StopCoroutine("ParkingCubeArea");   //点击左键后,循环中下个yield后终止此协程
            }
        }

        GameObject TransformedArea; //对应unity中edgeCanvas面板中的各个按钮
        GameObject maskCubes;   //用于上色的区块
        void MonitorAreaColor() //给各区域上颜色,并监听区域按钮
        {
            Material cubeMat;
            TransformedArea = GameObject.Find("TransformedArea");
            foreach (Transform child in TransformedArea.transform)  //child已经是按钮了
            {
                if (child.name.StartsWith("area"))
                {
                    child.GetComponent<Button>().onClick.AddListener(
                                    delegate ()
                                    {
                                        this.StartCoroutine("ParkingCubeArea");
                                    });
                }

            }
            maskCubes = GameObject.Find("maskCubes");
            foreach (Transform child in maskCubes.transform)
            {
                if (child.name.StartsWith("areazw"))
                {
                    cubeMat = child.gameObject.GetComponent<Renderer>().material;
                    cubeMat.SetColor("_Color", SetCubeColor(0));
                }
                if (child.name.StartsWith("areakw"))
                {
                    cubeMat = child.gameObject.GetComponent<Renderer>().material;
                    cubeMat.SetColor("_Color", SetCubeColor(1));
                }
                if (child.name.StartsWith("areayr"))
                {
                    cubeMat = child.gameObject.GetComponent<Renderer>().material;
                    cubeMat.SetColor("_Color", SetCubeColor(2));
                }
            }
        }
    }
}
