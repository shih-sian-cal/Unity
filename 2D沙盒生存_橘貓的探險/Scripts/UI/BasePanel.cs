using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 面板基類
// 幫助我們通過代碼快速的找到所有的子控件
// 方便我們在子類中處理邏輯
// 節省找控件的工作量
public class BasePanel : MonoBehaviour
{    
    //UGUI的基礎组件都最終繼承自UIBehaviour
    //通過裡式轉換原則，來存儲所有的UI控件
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();
    private void Awake()
    {
        FindChildControl<Button>();
        FindChildControl<Image>();
        FindChildControl<Text>();
        FindChildControl<Toggle>();
        FindChildControl<ScrollRect>();
        FindChildControl<Slider>();
    }
    //得到對應名字的對應控件腳本
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        //用字典ContainsKey找裡面有沒有這個鍵
        if (controlDic.ContainsKey(controlName))
        {
            for (int i = 0; i < controlDic[controlName].Count; i++)
            {
                //對應字典的值（是個集合）中，符合要求的類型的
                //則返回出去，這樣外部就可以獲取到了
                if (controlDic[controlName][i] is T)
                {
                    return controlDic[controlName][i] as T;
                }
            }
        }
        return null;
    }
    //找到相對應的UI控件並記錄到字典中
    private void FindChildControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
        string objname;
        for (int i = 0; i < controls.Length; i++)
        {
            objname = controls[i].gameObject.name;
            if (controlDic.ContainsKey(objname))
            {
                controlDic[objname].Add(controls[i]);
            }
            else
            {
                controlDic.Add(objname, new List<UIBehaviour>() { controls[i] });
            }
        }
    }

    //讓子類重寫（覆蓋）此方法，來實現UI的隱藏與出現
    public virtual void ShowMe()
    {

    }
    public virtual void HideMe()
    {

    }
}