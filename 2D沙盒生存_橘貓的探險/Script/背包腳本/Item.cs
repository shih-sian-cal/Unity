using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//右鍵選單新增Item選項，設定預設選項名稱、預設清單路徑名稱
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public int itemKey; //道具數據主鍵
    public string itemName; //道具名稱
    public Sprite itemImage; //道具圖片
    public int itemType; //道具種類
    public int itemHeld; //道具數量
    [TextArea] //讓道具描述框變大
    public string itemInfo; //道具描述
    public int itemeqbip; //是否是工具
}