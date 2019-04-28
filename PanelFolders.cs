using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CourtScript
{
    public class PanelFolders : MonoBehaviour
    {
        DateTime rightTime;
        

        // Start is called before the first frame update
        void Start()
        {
            rightTime = DateTime.Now;
            transform.GetComponent<Button>().onClick.AddListener(Panels);
            
        }   

        // Update is called once per frame
        void Update()
        {
            
        }

        void Panels()
        {
            GameObject parents = transform.parent.gameObject;
            GameObject parentss = parents.transform.parent.gameObject;
            parentss.transform.localPosition = StartB.RcanvasinitPosition;  //当此变量在StartB当中设置为static public，可以直接改变
            StartB.changeCanvas = 0;   
        }
    }
}
