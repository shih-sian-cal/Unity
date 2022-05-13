using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int slotID; //空格ID等於道具ID
    public Item slotItem; //列表裡的道具類別
    public Image slotImage; //列表裡的道具圖片
    public Text slotNum; //列表裡的道具數量
    public string slotInfo; //列表裡的道具描述
    public int sloteqbip; //列表裡的道具是否是工具
    public GameObject itemInSlot;

    //列表中的道具按鈕被點擊時
    public void ItemOnClicked()
    {
        //顯示該道具的描述
        InvertoryManager.UpdateItemInfo(slotInfo);
    }

    public void SetupSlot(Item item)
    {
        if (item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }

        slotImage.sprite = item.itemImage;
        slotNum.text = item.itemHeld.ToString();
        slotInfo = item.itemInfo;
        sloteqbip = item.itemeqbip;
    }
}