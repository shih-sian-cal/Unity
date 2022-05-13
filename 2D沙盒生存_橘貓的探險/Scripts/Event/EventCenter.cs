using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{
    //這是一個空接口
}
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}
public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public class EventCenter : BaseManager<EventCenter>
{
    //字典中，key對應著事件的名字，
    //value對應的是監聽這個事件對應的委託方法們（重點圈住：們）
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    //添加事件監聽
    //第一個参數：事件的名字
    //第二個参數：處理事件的方法
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //有沒有對應的事件監聽
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        //沒有的情况
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }

    //對於不需要参數的情况的重載方法
    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }

    //通過事件名字進行事件觸發
    public void EventTrigger<T>(string name, T info)
    {
        //有沒有對應的事件監聽
        //有的情况（有人關心這個事件）
        if (eventDic.ContainsKey(name))
        {
            //調用委託（依次執行委託中的方法）
            //如果是一個C#的簡化操作的存在，則直接調用委託
            (eventDic[name] as EventInfo<T>).actions?.Invoke(info);
        }
    }

    //對於不需要参數的情况的重載方法
    public void EventTrigger(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions?.Invoke();
        }
    }

    //移除對應的事件監聽
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            //移除這個委託
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }

    //對於不需要参數的情况的重載方法
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
    }

    //清空所有事件監聽(主要用在切換場景時)
    public void Clear()
    {
        eventDic.Clear();
    }
}