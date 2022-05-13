using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Detection : MonoBehaviour
{
    public Item thisItem; //道具類別
    public Inventory playerInventory; //背包
    public GameObject Explobion; //道具
    private Rigidbody2D m_Rigidbody2D; //2D剛體
    public static bool Item_D; //判斷道具是否是撿來的

    //程式開始執行時
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    //觸發方塊偵測到物件進來時
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //判斷道具是否有偵測到玩家
        if (collision.gameObject.tag == "Player")
        {
            //呼叫讓道具添加到背包的函式
            AddNewItem();
            //刪除道具
            Destroy(gameObject);
        }
        //判斷道具掉落時是否掉在Ground物件上
        else if (collision.gameObject.tag == "Ground")
        {
            //讓道具不會持續掉落
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }

    //觸發方塊偵測到物件離開時
    private void OnTriggerExit2D(Collider2D collision)
    {
        //判斷道具掉落時是否掉在Ground物件上
        if (collision.gameObject.tag == "Ground")
        {
            //讓道具不會持續掉落
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY - 1;
        }
    }

    //讓道具添加到背包裡
    public void AddNewItem()
    {
        if (thisItem.itemName == "key")
        {
            Portal.key_onoff = true;
        }

        //判斷背包裡面是否沒有這項道具
        if (!playerInventory.itemList.Contains(thisItem))
        {
            //添加到背包裡面
            thisItem.itemHeld = 1;
            for (int i = 0; i < playerInventory.itemList.Count; i++)
            {
                if (playerInventory.itemList[i] == null)
                {
                    playerInventory.itemList[i] = thisItem;
                    break;
                }
            }
        }
        //否則在背包裡面讓這項道具個數+1
        else
        {
            //當道具是為工具時
            if (thisItem.itemeqbip == 1)
            {
                //添加到背包裡面
                thisItem.itemHeld = 1;
                for (int i = 0; i < playerInventory.itemList.Count; i++)
                {
                    if (playerInventory.itemList[i] == null)
                    {
                        playerInventory.itemList[i] = thisItem;
                        break;
                    }
                }
            }
            //否則不是工具時
            else
            {
                //該道具個數+1
                thisItem.itemHeld += 1;
            }
        }

        //開啟撿到具偵測
        Item_D = true;

        //顯示背包裡的東西
        InvertoryManager.RefreshItem();
    }
}