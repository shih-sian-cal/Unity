using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Destruction : MonoBehaviour
{
    public GameObject GlassBreak; //道具
    public bool Trigger_onoff = false; //觸發器開關
    private SpriteRenderer spriterenderer; //存物件原圖
    public GameObject nulBlock; //存空物件
    public Sprite spr; //存要更換的圖
    private Sprite temp; //暫存圖的變數
    public static bool CBbagonoff = UseBag.isbag; //判斷背包開關
    public static bool CBsetuponoff = Player_Action.isSetUp;
    public bool obj_mouse = false; //偵測滑鼠是否移入該物件
    float time_float = 1f; //破壞時間
    public static Slot stone_onoff;
    float Ctime_float = 5f;

    //程式開始執行時
    void Start()
    {
        spriterenderer = this.GetComponent<SpriteRenderer>(); //取得SpriteRenderer
        temp = spriterenderer.sprite; //把原圖存進另一個變數中
    }

    //每幀執行時
    void Update()
    {
        CBbagonoff = UseBag.isbag; //判斷背包開關
        CBsetuponoff = Player_Action.isSetUp;
        //按下E鍵打開背包
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            spriterenderer.sprite = temp; //把換過的圖換成原本的圖
            obj_mouse = false;
        }
        //按住滑鼠左鍵且鼠標移動到該物件上，並且該物件的判斷觸發器是否開啟
        if (Input.GetKey(KeyCode.Mouse0) && obj_mouse == true && Trigger_onoff == true)
        {
            spriterenderer.color = Color.gray;

            InvokeRepeating("timer", 0.5f, 0.5f); //0.5秒後，每0.5秒重複呼叫timer函式。(開始倒數計時)
            //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)

            //判斷背包是否關閉
            if (CBbagonoff == false && CBsetuponoff == false)
            {
                if (this.gameObject.name == "stone(Clone)")
                {
                    string pick_name;
                    if (stone_onoff != null)
                    {
                        pick_name = stone_onoff.slotImage.sprite.name;
                    }
                    else
                    {
                        pick_name = null;
                    }

                    if (pick_name == "木鎬")
                    {
                        //判斷按住鼠標是否維持在1秒鐘
                        if (time_float == 0)
                        {
                            DestroyObj();
                        }
                    }
                    else if (pick_name == "石鎬" || pick_name == "鐵鎬" || pick_name == "十字鎬")
                    {
                        DestroyObj();
                    }
                    else
                    {
                        return;
                    }
                }
                else if (this.gameObject.name == "stone_browniron(Clone)")
                {
                    string pick_name;
                    if (stone_onoff != null)
                    {
                        pick_name = stone_onoff.slotImage.sprite.name;
                    }
                    else
                    {
                        pick_name = null;
                    }

                    if (pick_name == "石鎬")
                    {
                        //判斷按住鼠標是否維持在1秒鐘
                        if (time_float == 0)
                        {
                            DestroyObj();
                        }
                    }
                    else if (pick_name == "鐵鎬" || pick_name == "十字鎬")
                    {
                        DestroyObj();
                    }
                    else
                    {
                        return;
                    }
                }
                else if (this.gameObject.name == "tree_trunk_mid(Clone)" ||
                         this.gameObject.name == "tree_trunk_bottom(Clone)" ||
                         this.gameObject.name == "trunk_side(Clone)")
                {
                    string axe_name;

                    if (stone_onoff != null)
                    {
                        axe_name = stone_onoff.slotImage.sprite.name;
                    }
                    else
                    {
                        axe_name = null;
                    }

                    if (axe_name == null)
                    {
                        //判斷按住鼠標是否維持在1秒鐘
                        if (time_float == 0)
                        {
                            DestroyObj();
                        }
                    }
                    else if (axe_name == "木斧" || axe_name == "石斧" || axe_name == "鐵斧" || axe_name == "斧頭")
                    {
                        DestroyObj();
                    }
                    else
                    {
                        //判斷按住鼠標是否維持在1秒鐘
                        if (time_float == 0)
                        {
                            DestroyObj();
                        }
                    }
                }
                else
                {
                    //判斷按住鼠標是否維持在1秒鐘
                    if (time_float == 0)
                    {
                        DestroyObj();
                    }
                }
            }
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            spriterenderer.color = new Color(255, 255, 255, 255);
        }
    }

    //破壞物件
    void DestroyObj()
    {
        Destroy(this.gameObject); //刪除物件

        //如果該物件不會掉道具時
        if (GlassBreak == null)
        {
            Instantiate(nulBlock, transform.position, transform.rotation); //生成空物件
            return;
        }
        //否則該物件會掉道具時
        else
        {
            Instantiate(GlassBreak, transform.position, transform.rotation); //生成道具 
            Instantiate(nulBlock, transform.position, transform.rotation); //生成空物件
        }
    }

    //自訂一個函式叫做timer
    void timer()
    {
        time_float -= 0.5f; //每次呼叫倒數的時間就扣0.5

        if (time_float == 0)
        {
            CancelInvoke("timer"); //取消重複呼叫timer函式。(停止倒數計時)
            //CancelInvoke取消重複呼叫(“函式名”)
        }
    }

    //當滑鼠游標移動到該物件時
    private void OnMouseEnter()
    {
        //如果背包是關閉狀態時
        if (CBbagonoff == false && CBsetuponoff == false)
        {
            //圖片為石頭
            if (this.gameObject.name == "stone(Clone)")
            {
                string pick_name;
                if (stone_onoff != null)
                {
                    pick_name = stone_onoff.slotImage.sprite.name;
                }
                else
                {
                    pick_name = null;
                }

                if (pick_name == "木鎬" || pick_name == "石鎬" || pick_name == "鐵鎬" || pick_name == "十字鎬")
                {
                    OSMS(1); //重複程序交給OSMS工具執行
                    obj_mouse = true;
                }
                else
                {
                    return;
                }
            }
            //圖片為鐵礦
            else if (this.gameObject.name == "stone_browniron(Clone)")
            {
                string pick_name;
                if (stone_onoff != null)
                {
                    pick_name = stone_onoff.slotImage.sprite.name;
                }
                else
                {
                    pick_name = null;
                }

                if (pick_name == "石鎬" || pick_name == "鐵鎬" || pick_name == "十字鎬")
                {
                    OSMS(1); //重複程序交給OSMS工具執行
                    obj_mouse = true;
                }
                else
                {
                    return;
                }
            }
            //圖片為樹根、樹幹、木頭
            else if (this.gameObject.name == "tree_trunk_mid(Clone)" ||
                     this.gameObject.name == "tree_trunk_bottom(Clone)" ||
                     this.gameObject.name == "trunk_side(Clone)")
            {
                string axe_name;

                if (stone_onoff != null)
                {
                    axe_name = stone_onoff.slotImage.sprite.name;
                }
                else
                {
                    axe_name = null;
                }
                
                if (axe_name == null || axe_name == "木斧" || axe_name == "石斧" || axe_name == "鐵斧" || axe_name == "斧頭")
                {
                    OSMS(1); //重複程序交給OSMS工具執行
                    obj_mouse = true;
                }
                else
                {
                    OSMS(1); //重複程序交給OSMS工具執行
                    obj_mouse = true;
                }
            }
            else
            {
                OSMS(1); //重複程序交給OSMS工具執行
                obj_mouse = true;
            }
        }
    }

    //當滑鼠游標離開該物件時
    private void OnMouseExit()
    {
        //如果背包是關閉狀態時
        if (CBbagonoff == false && CBsetuponoff == false)
        {
            OSMS(2); //重複程序交給OSMS工具執行
            obj_mouse = false;
            spriterenderer.color = new Color(255, 255, 255, 255);
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

    //觸發方塊偵測到物件進來時
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //判斷玩家的距離是否觸碰到物件
        if (collision.gameObject.tag == "Player Trigger" || collision.gameObject.tag == "Player")
        {
            Trigger_onoff = true; //開啟觸發器
        }
        //否則判斷玩家的距離是否沒碰到物件
        else if (collision.gameObject.tag != "Player Trigger" || collision.gameObject.tag == "Player")
        {
            //修正玩家沒辦法拆掉已讓道具掉到該物件上會關閉觸發器
            for (int i = 0; i < 1; i++)
            {
                //判斷玩家的距離是否觸碰到物件
                if (collision.gameObject.tag == "Player Trigger" || collision.gameObject.tag == "Player")
                {
                    break;
                }
                else
                {
                    Trigger_onoff = false;  //關閉觸發器
                }
            }
            //開啟觸發器
            Trigger_onoff = true;
        }
    }

    //觸發方塊偵測到物件離開時
    private void OnTriggerExit2D(Collider2D collision)
    {
        Trigger_onoff = false; //關閉觸發器
    }

    //從UseItem取得資料
    public static void SMslot(Slot y)
    {
        if (y != null)
        {
            if (y.slotImage.sprite != null)
            {
                stone_onoff = y;
            }
            else
            {
                stone_onoff = null;
            }
        }
        else
        {
            stone_onoff = null;
        }
    }
}