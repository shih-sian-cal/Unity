using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : BaseManager<InputMgr>
{
    private bool isStart = false;

    //構造方法中，添加Update監聽
    public InputMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(MyUpdate);
    }
    //檢測是否需要開啟输入檢測
    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
    }
    private void MyUpdate()
    {
        //沒有開啟输入檢測，就不去檢測
        if (!isStart)
            return;
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.D);
        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.S);
    }
    private void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            //事件中心模塊，分發按下抬起事件（把哪個按鍵也發送出去）
            EventCenter.GetInstance().EventTrigger("KeyisDown", key);
        }
        if (Input.GetKeyUp(key))
        {
            EventCenter.GetInstance().EventTrigger("KeyisUp", key);
        }
    }
}