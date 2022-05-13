using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Mono的管理者
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;
    private void Start()
    {
        //此對象不可移除
        //從而方便别的對象找到該物體，從而獲取腳本，從而添加方法
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (updateEvent != null)
        {
            updateEvent();
        }
    }
    //為外部提供的添加幀更新事件的方法
    public void AddUpdateListener(UnityAction func)
    {
        updateEvent += func;
    }
    //為外部提供的移除幀更新事件的方法
    public void RemoveUpdateListener(UnityAction func)
    {
        updateEvent -= func;
    }
}
