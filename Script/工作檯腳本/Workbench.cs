using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Workbench : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Slot Workslot; //工作檯道具列表
    public Inventory mybag; //背包
    public GameObject material; //道具素材顯示
    public Item thisItem; //道具類別
    public GameObject WB; //工作檯
    public GameObject WB2;
    public GameObject WB3;
    public GameObject WB4;
    public static int Page = 1; //頁數

    private bool mat_onoff = false;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && mat_onoff == true)
        {
            material.SetActive(false);
        }
    }

    //上一頁
    public void PageUp()
    {
        if (Player_Action.WB_onoff == true && UseBag.isbag == true)
        {
            if (Page == 2)
            {
                Page = 1;
                WB.SetActive(true);
                WB2.SetActive(false);
            }
            else if (Page == 3)
            {
                Page = 2;
                WB2.SetActive(true);
                WB3.SetActive(false);
            }
            else if (Page == 4)
            {
                Page = 3;
                WB3.SetActive(true);
                WB4.SetActive(false);
            }
        }
    }

    //下一頁
    public void PageDown()
    {
        if (Player_Action.WB_onoff == true && UseBag.isbag == true)
        {
            if (Page == 1)
            {
                Page = 2;
                WB2.SetActive(true);
                WB.SetActive(false);
            }
            else if (Page == 2)
            {
                Page = 3;
                WB3.SetActive(true);
                WB2.SetActive(false);
            }
            else if (Page == 3)
            {
                Page = 4;
                WB4.SetActive(true);
                WB3.SetActive(false);
            }
        }
    }

    //滑鼠點擊事件
    public void OnPointerDown(PointerEventData eventData)
    {
        //若滑鼠左鍵
        if (eventData.pointerId == -1)
        {
            //道具名稱配對
            if (eventData.pointerCurrentRaycast.gameObject.name == "工作檯")
            {
                //呼叫專門處理素材數量和種類之函式(素材只有1種的狀況, 素材種類名稱1, 素材種類名稱2, 素材種類數量1, 素材數種類量2)
                weapon("A", "trunk_side", "", 2, 0);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "木劍")
            {
                weapon("A", "trunk_side", "", 3, 0);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "木鎬")
            {
                weapon("A", "trunk_side", "", 4, 0);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "木斧")
            {
                weapon("A", "trunk_side", "", 4, 0);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "石劍")
            {
                //呼叫專門處理素材數量和種類之函式(素材只有2種的狀況, 素材種類名稱1, 素材種類名稱2, 素材種類數量1, 素材數種類量2)
                weapon("B", "trunk_side", "stone", 1, 2);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "石鎬")
            {
                weapon("B", "trunk_side", "stone", 1, 3);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "石斧")
            {
                weapon("B", "trunk_side", "stone", 1, 3);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "鐵劍")
            {
                weapon("B", "trunk_side", "stone_browniron", 1, 2);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "鐵鎬")
            {
                weapon("B", "trunk_side", "stone_browniron", 1, 3);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "鐵斧")
            {
                weapon("B", "trunk_side", "stone_browniron", 1, 3);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "劍")
            {
                weapon("B", "trunk_side", "stone_browniron", 3, 6);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "十字鎬")
            {
                weapon("B", "trunk_side", "stone_browniron", 3, 9);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "斧頭")
            {
                weapon("B", "trunk_side", "stone_browniron", 3, 9);
            }
        }
    }

    //處理素材數量和種類之函式
    public void weapon(string index, string str1, string str2, int nu1, int nu2)
    {
        bool nu_onoff = false; //判斷合成道具

        //判斷素材種類狀況
        switch (index)
        {
            //如果該合成道具之素材只有一種時
            case "A":
                //根據背包格子數量做迴圈
                for (int i = 0; i < mybag.itemList.Count; i++)
                {
                    //如果該格子上有道具時
                    if (mybag.itemList[i] != null)
                    {
                        //如果該格子的道具名稱與素材種類名稱相符且素材要求的數量有剛好或超過時
                        if (mybag.itemList[i].itemName == str1 && mybag.itemList[i].itemHeld >= nu1)
                        {
                            //該格子的道具數量減去要求素材數量
                            mybag.itemList[i].itemHeld = mybag.itemList[i].itemHeld - nu1;
                            //如果該格子的道具數量是0
                            if (mybag.itemList[i].itemHeld == 0)
                            {
                                mybag.itemList[i] = null; //刪除背包列表中的道具
                            }

                            //素材要求的數量為2時，不減去
                            if (nu1 == 2)
                            {
                                
                                nu1 = 2;
                            }
                            //否則素材要求的數量為0
                            else
                            {
                                nu1 = 0;
                            }
                        }

                        //如果合成偵測為關閉且素材要求的數量是0
                        if (nu_onoff == false && nu1 == 0)
                        {
                            //
                            nu_onoff = true;
                            //添加到背包裡面
                            thisItem.itemHeld = 1;
                            for (int j = 0; j < mybag.itemList.Count; j++)
                            {
                                if (mybag.itemList[j] == null)
                                {
                                    mybag.itemList[j] = thisItem;
                                    break;
                                }
                            }
                        }
                        else if (nu_onoff == false && nu1 == 2)
                        {
                            nu_onoff = true;

                            //判斷背包裡面是否沒有這項道具
                            if (!mybag.itemList.Contains(thisItem))
                            {
                                //添加到背包裡面
                                thisItem.itemHeld = 1;
                                for (int j = 0; j < mybag.itemList.Count; j++)
                                {
                                    if (mybag.itemList[j] == null)
                                    {
                                        mybag.itemList[j] = thisItem;
                                        break;
                                    }
                                }
                            }
                            //否則在背包裡面讓這項道具個數+1
                            else
                            {
                                //該道具個數+1
                                thisItem.itemHeld += 1;
                            }
                        }
                        //開啟撿到具偵測
                        Item_Detection.Item_D = true;

                        //顯示背包裡的東西
                        InvertoryManager.RefreshItem();
                    }
                }
                break;
            //如果該合成道具之素材只有兩種
            case "B":
                for (int i = 0; i < mybag.itemList.Count; i++)
                {
                    if (mybag.itemList[i] != null)
                    {
                        if (mybag.itemList[i].itemName == str1 && mybag.itemList[i].itemHeld >= nu1)
                        {
                            mybag.itemList[i].itemHeld = mybag.itemList[i].itemHeld - nu1;
                            if (mybag.itemList[i].itemHeld == 0)
                            {
                                mybag.itemList[i] = null; //刪除背包列表中的道具
                            }
                            nu1 = 0;
                        }
                        else if (mybag.itemList[i].itemName == str2 && mybag.itemList[i].itemHeld >= nu2)
                        {
                            mybag.itemList[i].itemHeld = mybag.itemList[i].itemHeld - nu2;
                            if (mybag.itemList[i].itemHeld == 0)
                            {
                                mybag.itemList[i] = null; //刪除背包列表中的道具
                            }
                            nu2 = 0;
                        }

                        if (nu_onoff == false && nu1 == 0 && nu2 == 0)
                        {
                            nu_onoff = true;
                            //添加到背包裡面
                            thisItem.itemHeld = 1;
                            for (int j = 0; j < mybag.itemList.Count; j++)
                            {
                                if (mybag.itemList[j] == null)
                                {
                                    mybag.itemList[j] = thisItem;
                                    break;
                                }
                            }

                            //開啟撿到具偵測
                            Item_Detection.Item_D = true;

                            //顯示背包裡的東西
                            InvertoryManager.RefreshItem();
                        }
                    }
                }
                break;
        }
    }
        
    //滑鼠移入事件
    public void OnPointerEnter(PointerEventData eventData)
    {
        material.GetComponent<Transform>().position = new Vector3(eventData.position.x + 300, eventData.position.y);
        material.SetActive(true);
        mat_onoff = true;
    }

    //滑鼠移出事件
    public void OnPointerExit(PointerEventData eventData)
    {
        material.SetActive(false);
    }
}
