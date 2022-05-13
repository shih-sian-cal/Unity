using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//ProjectBase中新建一个目錄Res，再裡面新建一個腳本——ResMgr.cs
//資源加載模塊
public class ResMgr : BaseManager<ResMgr>
{
    //同步加载資源
    public T Load<T>(string name) where T : Object
    {

        T res = Resources.Load<T>(name);
        //resources.Load-加載資源

        //如果對象是一個GameObject類型的，我把它實例化後，再返回出去直接使用。
        //is檢查一個對象是否兼容於指定的類型
        if (res is GameObject)
            return GameObject.Instantiate(res);//實例化
        else //else情况示例：TextAsset、AudioClip
            return res;
    }

    //異步加載資源
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        //開啟異步加載的協程
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync<T>(name, callback));
    }

    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        {
            //Resources.LoadAsync 異步加載Resources文件夾中的資源。
            //ResourceRequest從資源包異步加載請求。
            ResourceRequest r = Resources.LoadAsync<T>(name);
            yield return r;

            if (r.asset is GameObject)
            {
                //實例化一下再傳给方法
                callback(GameObject.Instantiate(r.asset) as T);
            }
            else
            {
                //直接傳给方法
                callback(r.asset as T);
            }
        }
    }
}