using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public static bool BP_onoff;
    public Transform originalParent;
    public Slot slot;
    public Inventory mybag;
    public static Slot temp_slot;
    private int currentItemID; //當前道具ID
    public string str;

    private void Start()
    {
        temp_slot = slot;
    }

    //滑鼠點擊事件
    public void OnPointerDown(PointerEventData eventData)
    {
        //當滑鼠右鍵時，可以取得背包裡的圖片並傳送到UseItem類別中
        if (eventData.pointerId == -2 )
        {
            BP_onoff = true;
            temp_slot = slot;
            UseItem.ShowItemIM(temp_slot);
            CreateBlock.SetMessgeslot(temp_slot); //把Slot資料丟去CreateBlock函式
            CreateBlock.SetBag(mybag);
            BP_onoff = false;
        }
    }

    //滑鼠開始拖曳事件
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)//當按滑鼠左鍵時
        {
            originalParent = transform.parent;
            currentItemID = originalParent.GetComponent<Slot>().slotID;
            transform.SetParent(transform.parent.parent);
            transform.position = eventData.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;//設限阻攔關閉
        }
    }

    //滑鼠拖曳事件
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)//當按滑鼠左鍵時
        {
            transform.position = eventData.position;
            //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);//輸出鼠標當前位置下到第一個碰到道具名字
            str = eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotImage.sprite.name;
        }
    }

    //滑鼠結束拖曳事件
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)//當按滑鼠左鍵時
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                //判斷下面道具名字是：item_Image 或 number 就互換位置
                if (eventData.pointerCurrentRaycast.gameObject.name == "item_Image" || eventData.pointerCurrentRaycast.gameObject.name == "number")
                {
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                    transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
                    //itemList的道具存儲位置改變
                    var temp = mybag.itemList[currentItemID];
                    mybag.itemList[currentItemID] = mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                    mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;

                    eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
                    CreateBlock.SetMessgeslot(temp_slot); //把Slot資料丟去CreateBlock函式
                    CreateBlock.SetBag(mybag);
                    GetComponent<CanvasGroup>().blocksRaycasts = true;//設限阻攔開啟，不然無法再次選中移動的道具
                    return;
                }
                //判斷格子裡有沒有道具
                if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)" && mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] == null)
                {
                    //否則直接掛在檢測到Slot下面
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                    transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                    //itemList的道具存儲位置改變
                    mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = mybag.itemList[currentItemID];
                    //解決道具放在自己位置的問題
                    if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID != currentItemID) mybag.itemList[currentItemID] = null;
                    CreateBlock.SetMessgeslot(temp_slot); //把Slot資料丟去CreateBlock函式
                    CreateBlock.SetBag(mybag);
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }
            }
            //當丢出背包外時
            else
            {
                if (str != "key")
                {
                    Destroy(transform.gameObject);
                    mybag.itemList[slot.slotID] = null;//銷毀背包列表中的道具
                    UseItem.ShowItemIM(null);
                    InvertoryManager.UpdateItemInfo("");
                }
                else
                {
                    transform.SetParent(originalParent);
                    transform.position = originalParent.position;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }
            //其他任何位置都歸位道具
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}