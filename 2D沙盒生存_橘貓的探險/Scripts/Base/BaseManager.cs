using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//C#中的泛型知識點
//設計模式、單例模式的知識點
public class BaseManager<T> where T:new()
{
    private static T instance;

    public static T GetInstance()
    {
        if (instance == null) instance = new T();
        return instance;
    }
}
