using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvertoryManager : MonoBehaviour
{
    //背包管理員:方便在程式碼中訪問的值
    static InvertoryManager instance;

    public Inventory myBag; //讀取背包的屬性變數
    public GameObject slotGrid; //讀取網格的屬性變數
    public GameObject emtpySlot; //讀取列表的屬性變數
    public Text itemInfromation; //讀取道具描述的屬性變數
    public List<GameObject> slots = new List<GameObject>(); //管理生成的18個Slots
    public static List<GameObject> tempSlot = new List<GameObject>();

    //用於初始化任何變量或遊戲狀態
    //在腳本檔被創建並載入場景的時候呼叫
    private void Awake()
    {
        //判斷instance的值是否是空值，如果是的話就把InvertoryManager類別名稱改為instance
        if (instance != null) Destroy(this);
        instance = this;
    }

    //顯示背包裡的東西
    private void OnEnable()
    {
        RefreshItem();
        //顯示道具描述
        instance.itemInfromation.text = "";
    }

    //判斷哪時顯示道具描述
    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInfromation.text = "道具描述：" + itemDescription;
    }

    //偵測背包中的道具是否有相同種類的道具
    public static void RefreshItem()
    {
        tempSlot = instance.slots;

        //循環刪除slotGrid下的子集道具
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            //判斷背包裡的道具個數是否是0，如果是0就跳出迴圈
            if (instance.slotGrid.transform.childCount == 0) break;
            //刪除所有道具
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }

        //重新生成對應myBag裡面的道具的slot
        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            //建立新的道具在背包中
            instance.slots.Add(Instantiate(instance.emtpySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<Slot>().slotID = i;
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.myBag.itemList[i]);
            //解決撿起道具會偵測不到背包裡的道具
            if (Item_Detection.Item_D == true)
            {
                if (UseItem.temp == null)
                {
                    ItemOnDrag.BP_onoff = true;
                    CreateBlock.SetBag(instance.myBag); //更新CreateBlock的背包數據
                    Item_Detection.Item_D = false; //關閉item_Detection撿道具偵測
                    ItemOnDrag.BP_onoff = false;
                }
               //查詢新建立的道具是否與角色手上的道具一樣
                else if (tempSlot[i].GetComponent<Slot>().slotImage.sprite == UseItem.temp)
                {
                    ItemOnDrag.BP_onoff = true;
                    UseItem.ShowItemIM(tempSlot[i].GetComponent<Slot>()); //傳遞查詢到的道具給UseItem
                    CreateBlock.SetMessgeslot(tempSlot[i].GetComponent<Slot>());
                    CreateBlock.SetBag(instance.myBag); //更新CreateBlock的背包數據
                    Item_Detection.Item_D = false; //關閉item_Detection撿道具偵測
                    ItemOnDrag.BP_onoff = false;
                }
                //查詢新建立的道具是否與從背包使用的道具一樣
                else if (tempSlot[i].GetComponent<Slot>().slotImage.sprite == ItemOnDrag.temp_slot.slotImage.sprite)
                {
                    ItemOnDrag.BP_onoff = true;
                    UseItem.ShowItemIM(instance.slots[i].GetComponent<Slot>()); //傳遞查詢到的道具給UseItem
                    CreateBlock.SetMessgeslot(instance.slots[i].GetComponent<Slot>());
                    CreateBlock.SetBag(instance.myBag); //更新CreateBlock的背包數據
                    Item_Detection.Item_D = false; //關閉item_Detection撿道具偵測
                    ItemOnDrag.BP_onoff = false;
                }
            }
        }
    }
}