using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//UI層級枚舉
public enum E_UI_Layer
{
    Bot,
    Mit,
    Top
}

//UI管理器（管理面板）
//管理所有顯示的面板
//提供给外部顯示和隱藏
public class UIManager : BaseManager<UIManager>
{
    public Dictionary<string, BasePanel> panelDic　= new Dictionary<string, BasePanel>();

    //這是幾個UI面板
    private Transform bot;
    private Transform mid;
    private Transform top;

    public UIManager()
    {
        //去找Canvas（做成了預設體在Resources/UI下面）
        GameObject obj = ResMgr.GetInstance().Load<GameObject>("UI/Canvas");
        Transform canvas = obj.transform;
        //創建Canvas，讓其過場景的時候不被移除
        GameObject.DontDestroyOnLoad(obj);

        //找到各層
        bot = canvas.Find("bot");
        mid = canvas.Find("mid");
        top = canvas.Find("top");

        //加載EventSystem，有了它，按鈕等组件才能響應
        obj = ResMgr.GetInstance().Load<GameObject>("UI/EventSystem");

        //創建Canvas，讓其過場景的時候不被移除
        GameObject.DontDestroyOnLoad(obj);
    }

    public void ShowPanel<T>(string panelName,
    E_UI_Layer layer = E_UI_Layer.Top,
    UnityAction<T> callback = null) where T : BasePanel
    {
        //已經顯示了此面板
        if (panelDic.ContainsKey(panelName))
        {
            //調用重寫方法，具體内容自己添加
            panelDic[panelName].ShowMe();
            if (callback != null)
                callback(panelDic[panelName] as T);
            return;
        }
        ResMgr.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) => {
            //把它作為Canvas的子對象
            //並且設置它的相對位置
            //找到父對象
            Transform father = bot;
            switch (layer)
            {
                case E_UI_Layer.Mit:
                    father = mid;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
            }
            //設置父對象
            obj.transform.SetParent(father);

            //設置相對位置和大小
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //得到預設体身上的腳本（繼承自BasePanel）
            T panel = obj.GetComponent<T>();

            //執行外面想要做的事情  
            if (callback != null)
            {
                callback(panel);
            }

            //在字典中添加此面板  
            panelDic.Add(panelName, panel);
        });
    }
    //隱藏面板  
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            //調用重寫方法，具體内容自己添加  
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }
}