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

        void CameraLookAtArea()
        {

        }

        float ColorAlpha = 0.5f;   //cube一开始的透明度,全局变量
        Color[] cubeColor = new Color[6];
        private Color SetCubeColor(int i)
        {
            cubeColor[0] = new Color(1, 0, 0, 0.5f);
            cubeColor[1] = new Color(0, 1, 0, 0.5f);
            cubeColor[2] = new Color(0, 0, 1, 0.5f);
            cubeColor[3] = new Color(1, 0, 1, 0.5f);
            cubeColor[4] = new Color(1, 1, 0, 0.5f);
            cubeColor[5] = new Color(0, 1, 1, 0.5f);
            return cubeColor[i];
        }

        bool isCubeFadeIn;    //控制颜色区域透明度是否变化
        static public bool isCubeFade;
        Material cubeMatrial;   //控制区域的材质透明度的值,只在协程中使用
        IEnumerator SwitchCubeFade(string cubename)
        {
            isCubeFade = true;
            cubeMatrial = GameObject.Find("maskCubes/areaxf").GetComponent<Renderer>().material;    //初始赋值
            foreach (Transform child in maskCubes.transform)
            {
                Material tempMat = child.gameObject.GetComponent<Renderer>().material;
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
                if (Input.GetMouseButtonDown(0)) StopCoroutine("SwitchCubeFade");   //点击某个按钮后,下个yield后不再执行
            }   
        }

        GameObject Transformed; //对应unity中edgeCanvas面板中的各个按钮
        GameObject maskCubes;
        void MonitorAreaColor() //给各区域上颜色,并监听区域按钮
        {
            Material cubeMat;
            Transformed = GameObject.Find("Transformed");
            foreach (Transform child in Transformed.transform)  //child已经是按钮了
            {
                if (child.name.StartsWith("area"))
                {
                    child.GetComponent<Button>().onClick.AddListener(
                                    delegate ()
                                    {
                                        this.StartCoroutine("SwitchCubeFade",child.name);
                                    });
                }

            }
            maskCubes = GameObject.Find("maskCubes");
            int n = 0;
            foreach (Transform child in maskCubes.transform)
            {
                cubeMat = child.gameObject.GetComponent<Renderer>().material;
                cubeMat.SetColor("_Color", SetCubeColor(n));
                n++;
            }
        }
    }
}
