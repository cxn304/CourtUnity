using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TableCreate : MonoBehaviour
{
    
    GameObject Row_Prefab;   //表头预设，这个要指定表头那个prefab

    //------------//------------//------------//------------//------------//------------//------------//------------
    void Start()
    {
        string[,] matl = Read_Csv("Pipe_Tee_RED");
        CreaTable(matl);
    }

    // Update is called once per frame
    void Update()
    {

    }
    //------------//------------//------------//------------//------------//------------//------------//------------

    private void CreaTable(string[,] matl)
    {
        int rown = matl.GetLength(0);   //数据的第0维（行），第1维是（列）
        Transform[] grandFa = GameObject.Find("btou").GetComponentsInChildren<Transform>();   //父物体下所有子物体的集合
        Row_Prefab = GameObject.Find("btou");   //找到预制件
        for (int i = 0; i < rown - 1; i++)  //添加并修改预设的过程，将创建10行
        {
            //在Table下创建新的预设实例
            GameObject table = GameObject.Find("canvasGroups/RightCanvasF/btnElec/Panel/Table");
            int CellCount = Row_Prefab.transform.childCount; //列数,子物体数量
            //Instantiate函数实例化是将original对象的所有子物体和子组件完全复制，
            //成为一个新的对象。这个新的对象拥有与源对象完全一样的东西，包括坐标值等。
            GameObject row = Instantiate(Row_Prefab, table.transform.position, table.transform.rotation) as GameObject;
            row.name = "row" + (i + 1);
            row.transform.SetParent(table.transform);
            row.transform.localScale = Vector3.one;//设置缩放比例1,1,1，不然默认的比例非常大

            //设置预设实例中的各个子物体的文本内容
            for (int j = 0; j < CellCount; j++)
            {
                //if (Row_Prefab.name == "checkbox_hover") continue;
                string bgname = "cell" + j;
                row.transform.Find(bgname).GetComponent<Text>().text = matl[i, j];
                //Debug.Log("matl" + matl[i, j]);
            }
        }
    }

    private string[,] Read_Csv(string csvName)
    {
        string[,] matl; //从csv中读取的string矩阵
        TextAsset mydata = Resources.Load<TextAsset>(csvName);   //只能读取Resources文件夹的内容
        string[] datalines = mydata.text.Split('\n');   //csv中每一行数据
        int n = datalines[1].Split(',').Length;     //列数
        matl = new string[datalines.Length - 1, n];   //矩阵下面进行赋值
        for (int i = 0; i <= datalines.Length - 2; i++)
        {
            string[] tempdata = datalines[i].Split(',');
            for (int j = 0; j <= n - 1; j++)
            {
                matl[i, j] = tempdata[j];
                //Debug.Log(tempdata[j]);
            }
        }
        return matl;
    }
}
