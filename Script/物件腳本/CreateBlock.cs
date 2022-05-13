using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateBlock : MonoBehaviour
{
    public static int Itemeqbip; //判斷道具是否為工具
    public static Slot slot_texturee; //列表
    private SpriteRenderer spriterenderer; //存物件原圖
    public Sprite spr; //存要更換的圖
    private Sprite temp; //暫存圖的變數
    public static bool CBbagonoff = UseBag.isbag; //判斷背包開關
    public static bool CBsetuponoff = Player_Action.isSetUp;
    public static Inventory CBmybag; //背包
    public bool obj_mouse = false; //偵測滑鼠是否移入該物件

    //程式開始執行時
    void Start()
    {
        spriterenderer = this.GetComponent<SpriteRenderer>(); //取得SpriteRenderer
        temp = spriterenderer.sprite; //把原圖存進另一個變數中
    }

    //每幀執行時
    void Update()
    {
        CBbagonoff = UseBag.isbag;
        CBsetuponoff = Player_Action.isSetUp;
        //按住E鍵打開背包
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            spriterenderer.sprite = temp; //把換過的圖換成原本的圖
            obj_mouse = false;
        }
        //如果按下滑鼠左鍵且滑鼠移入到空物件上時
        if (Input.GetKey(KeyCode.Mouse1) && obj_mouse == true)
        {
            //如果Slot為空則跳出函式
            if (slot_texturee == null) return;

            //如果玩家手上的道具不是工具類道具且沒有開啟背包的狀態時
            if (slot_texturee.sloteqbip == 0 && CBbagonoff == false && CBsetuponoff == false)
            {
                GameObject obj = Resources.Load<GameObject>("obj/" + slot_texturee.slotImage.sprite.name); //將字串變成物件，路徑"Resources/obj/圖檔"
                print(slot_texturee.slotImage.sprite.name);
                print(obj);

                //判斷該物件的名稱是否為空值
                if (slot_texturee.slotImage.sprite.name != null)
                {
                    if (CBmybag.itemList[slot_texturee.slotID] == null && UseItem.temp == null)
                    {
                        return;
                    }
                    else if (CBmybag.itemList[slot_texturee.slotID] == null && UseItem.temp != null)
                    {
                        for (int i = 0; i < CBmybag.itemList.Count; i++)
                        {
                            if (CBmybag.itemList != null)
                            {
                                if (CBmybag.itemList[i] != null)
                                {
                                    if (UseItem.temp.name == CBmybag.itemList[i].itemName)
                                    {
                                        Destroy(this.gameObject); //刪除該空物件
                                        Instantiate(obj, transform.position, transform.rotation); //在空物件座標上生成指定物件

                                        if ((int.Parse(slot_texturee.slotNum.text) > 1))
                                        {
                                            slot_texturee.slotNum.text = (int.Parse(slot_texturee.slotNum.text) - 1).ToString(); //道具數量減1
                                            CBmybag.itemList[i].itemHeld = int.Parse(slot_texturee.slotNum.text); //將減少後的值傳遞给背包列表中的值
                                        }
                                        //否則數量只剩1時
                                        else
                                        {       
                                            //開啟撿到具偵測
                                            Item_Detection.Item_D = true;

                                            //顯示背包裡的東西
                                            InvertoryManager.RefreshItem();

                                            GameObject go = slot_texturee.transform.GetChild(0).gameObject; //指定該道具列表中的子物件
                                            Destroy(go); //刪除該子物件
                                            CBmybag.itemList[i] = null; //刪除背包列表中的道具
                                            UseItem.ShowItemIM(null); //刪除玩家手上拿著的道具
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Destroy(this.gameObject); //刪除該空物件
                        Instantiate(obj, transform.position, transform.rotation); //在空物件座標上生成指定物件

                        //如果道具數量大於1時
                        if ((int.Parse(slot_texturee.slotNum.text) > 1))
                        {
                            slot_texturee.slotNum.text = (int.Parse(slot_texturee.slotNum.text) - 1).ToString(); //道具數量減1
                            CBmybag.itemList[slot_texturee.slotID].itemHeld = int.Parse(slot_texturee.slotNum.text); //將減少後的值傳遞给背包列表中的值
                        }
                        //否則數量只剩1時
                        else
                        {
                            GameObject go = slot_texturee.transform.GetChild(0).gameObject; //指定該道具列表中的子物件
                            Destroy(go); //刪除該子物件
                            CBmybag.itemList[slot_texturee.slotID] = null; //刪除背包列表中的道具
                            UseItem.ShowItemIM(null); //刪除玩家手上拿著的道具
                        }
                    }
                }
            }
        }
    }

    //當滑鼠游標移動到該物件時
    private void OnMouseEnter()
    {
        if (slot_texturee != null && CBbagonoff == false && slot_texturee.sloteqbip == 0 )
        {
            OSMS(1); //重複程序交給OSMS工具執行
            obj_mouse = true;
        }
    }

    //當滑鼠游標離開該物件時
    private void OnMouseExit()
    {
        if (slot_texturee != null && CBbagonoff == false && slot_texturee.sloteqbip == 0 )
        {
            OSMS(2); //重複程序交給OSMS工具執行
            obj_mouse = false;
        }
    }

    //針對重複程序做處理
    private void OSMS(int x)
    {
        //如果值等於1則做該物件的換圖程序
        if (x == 1)
        {
            temp = spriterenderer.sprite; //把原圖存進另一個變數中
            spriterenderer.sprite = spr; //把要換的圖換上去
        }
        //如果值等於2則換回原本的圖之程序
        else if (x == 2)
        {
            spriterenderer.sprite = temp; //把換過的圖換成原本的圖
        }
    }

    //從UseItem取得資料
    public static void SetMessgeslot(Slot y)
    {
        if (y != null)
        {
            if (y.slotImage.sprite != null)
            {
                if (ItemOnDrag.BP_onoff == true)
                {
                    slot_texturee = y;
                }
            }
            else
            {
                slot_texturee = null;
            }
        }
        else
        {
            slot_texturee = null;
        }
    }

    //從ItemOnDrag或InvertoryManager取得資料
    public static void SetBag(Inventory x)
    {
        CBmybag = x;
    }
}