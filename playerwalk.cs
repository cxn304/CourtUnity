using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//注释：     先CTRL+K，然后CTRL+C
//取消注释： 先CTRL+K，然后CTRL+U
public class playerwalk : MonoBehaviour
{
    Animator madmin;            //动画器对象
    private CharacterController controller; //这个才是对角色碰撞体积的定义位置


    // Start is called before the first frame update
    void Start()
    {
        madmin = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        controller.radius = 0.25f;
        controller.height = 1.5f;       //单位：米
        controller.slopeLimit = 80f;     //限制该角色只能爬小于等于该值的斜坡（一般设置该值小于90度）
        controller.center = new Vector3(0, controller.height / 2, 0);    //设置Character Controller 的位置 相当于锚点
        controller.minMoveDistance = 0; //角色最小的移动距离 防止角色抖动 一般设置为0。
    }


    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            madmin.SetInteger("animation_int", 1);  //切换动画状态，0和1是在animation中设置好的
        }
        else if (Input.GetKey(KeyCode.A))
        {
            madmin.SetInteger("animation_int", 1);
            transform.Rotate(Vector3.up, -4.0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            madmin.SetInteger("animation_int", 1);
            transform.Rotate(Vector3.up, 4.0f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            madmin.SetInteger("animation_int", 1);
            transform.Rotate(Vector3.up, 6.0f);
        }
        else
        {
            madmin.SetInteger("animation_int", 0);  //不按按键则停止走动
        }
    }

}
