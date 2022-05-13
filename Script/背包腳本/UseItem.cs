using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public static int Itemeqbip; //判斷道具是否為工具
    private static SpriteRenderer sprite_renderer; //角色手上的道具圖片
    private Animator m_Animator; //工具動畫
    private bool cut = false;
    public static Sprite temp;
    private bool sound = false;
    public static int sound_type = 0;

    public GameObject cat; //角色物件

    //程式開始執行時
    void Start()
    {
        sprite_renderer = this.GetComponent<SpriteRenderer>(); //取得SpriteRenderer
        m_Animator = GetComponent<Animator>(); //取得Animator
    }

    //每幀執行時
    void Update()
    {
        //按住D或方向右鍵可以讓道具隨著角色位置來更改座標
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            move_right();
            Cut_left();
        }
        //按住A或方向左鍵可以讓道具隨著角色位置來更改座標
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            move_left();
            Cut_right();
        }
        //當同時按住A鍵和D鍵或方向鍵左鍵和方向鍵右鍵時會根據當下角色方向來更改道具座標
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            //如果當下角色是面向左邊時
            if (sprite_renderer.flipX == true)
            {
                move_left();
            }
            //如果當下角色是面向右邊時
            else
            {
                move_right();
            }
        }
        //當按住滑鼠左鍵且手上道具為工具，同時道具方向是在左邊時
        if (Input.GetKey(KeyCode.Mouse0) && sprite_renderer.flipX == true && cut == false)
        {
            if (sprite_renderer.sprite != null){
                sound_type = 0;    //播放音效
            }
            m_Animator.SetFloat("item_cut_left", 1); //播放item_cut_left動畫
            cut = true;
        }
        //當按住滑鼠左鍵且手上道具為工具，同時道具方向是在右邊時
        else if (Input.GetKey(KeyCode.Mouse0) && sprite_renderer.flipX == false && cut == false)
        {
            if (sprite_renderer.sprite != null){
                sound_type = 0;    //播放音效
            }
            m_Animator.SetFloat("item_cut_right", 1); //播放item_cut_right動畫
            cut = true;
        }
        //當放開滑鼠左鍵且手上道具為工具，同時道具方向是在左邊時
        if (Input.GetKeyUp(KeyCode.Mouse0) && sprite_renderer.flipX == true)
        {
            Invoke("Cut_left", 0.1f); //0.1秒後呼叫Cut_left函式
            sound_type = 1;    //停止播放音效
        }
        //當放開滑鼠左鍵且手上道具為工具，同時道具方向是在右邊時
        else if (Input.GetKeyUp(KeyCode.Mouse0) && sprite_renderer.flipX == false)
        {
            Invoke("Cut_right", 0.1f); //0.1秒後呼叫Cut_right函式
            sound_type = 1;    //停止播放音效
        }
    }

    //結束item_move_right動畫函式
    void move_left()
    {
        m_Animator.SetFloat("item_move_right", 0);
        m_Animator.SetFloat("item_move_left", 1);
        sprite_renderer.flipX = true;
    }

    //結束item_move_left動畫函式
    void move_right()
    {
        m_Animator.SetFloat("item_move_right", 1);
        m_Animator.SetFloat("item_move_left", 0);
        sprite_renderer.flipX = false;
    }

    //結束item_cut_left動畫函式
    void Cut_left()
    {
        m_Animator.SetFloat("item_cut_left", 0); //結束item_cut_left動畫
        cut = false;
    }

    //結束item_cut_right動畫函式
    void Cut_right()
    {
        m_Animator.SetFloat("item_cut_right", 0); //結束item_cut_right動畫
        cut = false;
    }

    //取得背包裡的圖片並顯示在角色手上
    public static void ShowItemIM(Slot x)
    {
        //如果載入的Slot不為null時
        if (x != null)
        {
            if (x.slotImage.sprite != null)
            {
                if (ItemOnDrag.BP_onoff == true)
                {
                    sprite_renderer.sprite = x.slotImage.sprite; //把Slot裡的道具圖片載入到角色手上
                    temp = sprite_renderer.sprite;
                    Itemeqbip = x.sloteqbip; //取得道具是否是工具
                    Monster.SMslot(x);
                    BOSS.SMslot(x);
                    Object_Destruction.SMslot(x);
                }
            }
            else
            {
                NullItem();
            }
        }
        //如果載入的Slot為null時
        else
        {
            NullItem();
        }
    }

    //讓使用的道具為空值之函式
    public static void NullItem()
    {
        sprite_renderer.sprite = null; //角色手上為空
        Itemeqbip = 0; //道具為空
        CreateBlock.SetMessgeslot(null); //順便去告知CreateBlock為空
    }
}