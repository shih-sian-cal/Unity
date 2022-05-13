using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//右鍵選單新增Item選項，設定預設選項名稱、預設清單路徑名稱
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    //宣告一個能儲存Item的列表
    public List<Item> itemList = new List<Item>();
}
