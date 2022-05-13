using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//抽屜數據，池子中的一列容器
public class PoolData
{
    //抽屜中對象掛載的父節點
    public GameObject fatherObj;
    //對象的容器
    public List<GameObject> poolList;

    public PoolData(GameObject obj, GameObject poolObj)
    {
        //給我們的抽屜，創建一個父對象，並且把它作為我們pool(衣櫃)對象的子物體
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>() {};
        PushObj(obj);
    }

    //往抽屜裡面壓東西
    public void PushObj(GameObject obj)
    {
        //失活讓其隱藏
        obj.SetActive(false);
        //存起來
        poolList.Add(obj);
        //設置父對象
        obj.transform.parent = fatherObj.transform;
    }

    //從抽屜裡面取東西
    public GameObject GetObj()
    {
        GameObject obj = null;

        //取出第一個
        obj = poolList[0];
        poolList.RemoveAt(0);

        //激活讓其顯示
        obj.SetActive(true);
        //斷開了父子關係
        obj.transform.parent = null;

        return obj;
    }
}

//1.Dictionary、List
//2.GameObject和Resources兩個共用類中的API
public class Poolmgr : BaseManager<Poolmgr>
{
    //緩存池容器（衣櫃）
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject poolObj;

    //往外拿
    public GameObject GameObj(string name)
    {
        GameObject obj = null;
        //有抽屜，並且抽屜裡有東西
        if ( poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            obj = poolDic[name].GetObj();
        }
        else
        {
            obj = GameObject.Instantiate( Resources.Load<GameObject>(name));
            //把對象名字改的和池子名字一樣
            obj.name = name;
        }
        return obj;
    }

    //還暫時不用的東西給我
    public void PushObj(string name, GameObject obj)
    {
        if (poolObj == null) poolObj = new GameObject("Pool");

        //裡面有抽屜
        if ( poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        //裡面沒有抽屜
        else
        {
            poolDic.Add(name, new PoolData(obj, poolObj));
        }
    }

    //清空緩存池的方法
    //主要用在場景切換時
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
