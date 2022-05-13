using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOSS : MonoBehaviour
{
    public static float BOSS_Life_Nun; //BOSS血量
    public static Slot weapon_config; //判斷武器圖片
    private Slider UI_Life; //BOSS血量條
    private GameObject Fill_Area;
    public GameObject BOSS_HP;
    private Animator m_Animator; //BOSS做動作給予動畫的變數
    private bool de_onoff = false; //判斷攻擊距離
    private Rigidbody2D m_rigidbody2D; //修正角色撞牆不會異常抖動的變數
    private SpriteRenderer spriterenderer;
    private Vector2 moveDir;
    float time_float = 1f;
    float time_End = 0f;
    public GameObject BOSS_Life;

    public enum Status { idle, walk, attack }; //宣告一個狀態變數
    public bool IsFaceRight; //讓BOSS水平翻轉
    public Status status; //判斷用
    public enum Face { Right, Left }; //宣告一個狀態變數
    public Face face; //判斷用

    public float speed;
    public float jumpForce = 250.0f; //可更改跳躍力的變數
    private Transform myTransform;

    private Transform playerTransform;
    public GameObject End;

    // Start is called before the first frame update
    void Start()
    {
        BOSS_Life_Nun = 1;
        spriterenderer = this.GetComponent<SpriteRenderer>(); //取得SpriteRenderer                                                   
        m_rigidbody2D = GetComponent<Rigidbody2D>(); //取得Rigidbody2D
        m_Animator = GetComponent<Animator>(); //取得Animator
        status = Status.idle; //BOSS狀態為等待狀態

        //判斷BOSS面向
        if (spriterenderer.flipX)
        {
            face = Face.Right; //BOSS面向右邊
        }
        else
        {
            face = Face.Left; //BOSS面向左邊
        }

        myTransform = this.transform; //取得BOSS的Transform

        //查詢名叫橘貓(角色)的物件
        if (GameObject.Find("橘貓(角色)") != null)
        {
            playerTransform = GameObject.Find("橘貓(角色)").transform; //取得玩家的Transform
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("BOSS_Life") != null)
        {
            UI_Life = GameObject.Find("BOSS_Life").GetComponent<Slider>();
            BOSS_HP = GameObject.Find("BOSS_Life");
        }

        if (GameObject.Find("BOSS_Fill Area") != null)
        {
            Fill_Area = GameObject.Find("BOSS_Fill Area");
        }
        //查詢名叫橘貓(角色)的物件
        if (GameObject.Find("橘貓(角色)") != null)
        {
            playerTransform = GameObject.Find("橘貓(角色)").transform; //取得玩家的Transform
        }
        else if (GameObject.Find("橘貓(角色)(Clone)") != null)
        {
            playerTransform = GameObject.Find("橘貓(角色)(Clone)").transform; //取得玩家的Transform
        }

        UI_Life.value = BOSS_Life_Nun;

        //如果血量小於0.0001時
        if (BOSS_Life_Nun < 0.0001)
        {
            Fill_Area.SetActive(false); //刪除血量條
            Destroy(gameObject); //刪除角色物件
            Time.timeScale = 0;
            InvokeRepeating("E_timer", 0, 0.01f); //0秒後，每0.01秒重複呼叫timer函式。(開始倒數計時)
                                                //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)
        }

        //如果玩家按下滑鼠左鍵且攻擊距離偵測開始時
        if (Input.GetKeyDown(KeyCode.Mouse0) && de_onoff == true)
        {
            //如果weapon_config為空值
            if (weapon_config == null)
            {
                return;
            }
            else
            {
                string weapon_name = weapon_config.slotImage.sprite.name;

                GameObject obj = Resources.Load<GameObject>("obj/" + weapon_name); //將字串變成物件

                //如果圖片名稱為"劍"
                if (weapon_name == "劍")
                {
                    BOSS_Life_Nun -= 0.07f; //BOSS血量扣7

                    Mondamage();
                }
                else if (weapon_name == "鐵劍" || weapon_name == "斧頭")
                {
                    BOSS_Life_Nun -= 0.05f; //BOSS血量扣5

                    Mondamage();
                }
                else if (weapon_name == "石劍" || weapon_name == "鐵斧")
                {
                    BOSS_Life_Nun -= 0.03f; //BOSS血量扣3

                    Mondamage();
                }
                else if (weapon_name == "木劍" || weapon_name == "石斧" || weapon_name == "十字鎬")
                {
                    BOSS_Life_Nun -= 0.02f; //BOSS血量扣2

                    Mondamage();
                }
                else if (weapon_name == null)
                {
                    return;
                }
                else
                {
                    BOSS_Life_Nun -= 0.01f; //BOSS血量扣1

                    Mondamage();
                }
            }
        }
        float deltaTime = Time.deltaTime;

        //更新判斷BOSS狀態
        switch (status)
        {
            //BOSS狀態為等待
            case Status.idle:
                //判斷玩家Transform
                if (playerTransform)
                {
                    //如果玩家目前距離減BOSS目前距離小於絕對值100
                    if (Mathf.Abs(myTransform.position.x - playerTransform.position.x) < 100f)
                    {
                        //BOSS狀態轉為移動狀態
                        status = Status.walk;
                    }
                }
                break;
            //BOSS狀態為移動
            case Status.walk:
                //判斷玩家Transform
                if (playerTransform)
                {
                    //如果BOSS目前距離大於等於玩家目前距離
                    if (myTransform.position.x >= playerTransform.position.x)
                    {
                        //BOSS面向左邊
                        spriterenderer.flipX = false;
                        face = Face.Left;
                    }
                    //否則BOSS目前距離大於等於玩家目前距離
                    else
                    {
                        //BOSS面向右邊
                        spriterenderer.flipX = true;
                        face = Face.Right;
                    }
                }
                //更新判斷BOSS面向
                switch (face)
                {
                    //BOSS面向為右邊
                    case Face.Right:
                        moveDir.x = speed; //速度往右增加
                    break;
                    //BOSS面向為左邊
                    case Face.Left:
                        moveDir.x = -speed; //速度往左增加
                    break;
                }

                m_Animator.SetFloat("豬頭人(移動)", 1);
                //如果BOSS速度為0
                if (m_rigidbody2D.velocity == Vector2.zero)
                {
                    m_Animator.SetFloat("豬頭人(移動)", 0);
                    m_rigidbody2D.AddForce(Vector2.up * jumpForce); //BOSS進行跳躍動作
                }
                //判斷玩家Transform
                if (playerTransform)
                {
                    //如果玩家目前距離減BOSS目前距離大於等於絕對值100
                    if (Mathf.Abs(myTransform.position.x - playerTransform.position.x) >= 100f)
                    {
                        status = Status.idle; //BOSS狀態轉為等待狀態
                        moveDir = Vector2.zero; //BOSS移動速度為0
                        m_Animator.SetFloat("豬頭人(移動)", 0);
                    }
                    //如果玩家目前距離減BOSS目前距離小於絕對值1.5
                    else if (Mathf.Abs(myTransform.position.x - playerTransform.position.x) <= 1.5f)
                    {
                        //BOSS狀態轉為攻擊狀態
                        status = Status.attack;
                    }
                }
            break;
            case Status.attack:
                //判斷玩家Transform
                if (playerTransform)
                {
                    //如果BOSS目前距離大於等於玩家目前距離
                    if (myTransform.position.x >= playerTransform.position.x)
                    {
                        //BOSS面向左邊
                        spriterenderer.flipX = false;
                        face = Face.Left;
                    }
                    //否則BOSS目前距離大於等於玩家目前距離
                    else
                    {
                        //BOSS面向右邊
                        spriterenderer.flipX = true;
                        face = Face.Right;
                    }

                    //如果玩家目前距離減BOSS目前距離小於絕對值1.5
                    if (Mathf.Abs(myTransform.position.x - playerTransform.position.x) > 1.5f)
                    {
                        //BOSS狀態轉為移動狀態
                        status = Status.walk;
                        m_Animator.SetFloat("豬頭人(攻擊)", 0);
                    }
                    else
                    {
                        m_Animator.SetFloat("豬頭人(攻擊)", 1);
                    }
                }
            break;
        }

        //取得角色往下掉的數值
        moveDir.y = m_rigidbody2D.velocity.y;
        m_rigidbody2D.velocity = moveDir;
    }

    //BOSS受傷特效
    void Mondamage()
    {
        spriterenderer.color = Color.gray;

        InvokeRepeating("timer", 0, 0.25f); //0.5秒後，每0.5秒重複呼叫timer函式。(開始倒數計時)
                                            //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)
    }

    //自訂一個函式叫做timer
    void timer()
    {
        time_float -= 0.5f; //每次呼叫倒數的時間就扣0.5

        if (time_float == 0)
        {
            spriterenderer.color = new Color(255, 255, 255, 255);
            CancelInvoke("timer"); //取消重複呼叫timer函式。(停止倒數計時)
            //CancelInvoke取消重複呼叫(“函式名”)
            time_float = 1f;
        }
    }

    //自訂一個函式叫做timer
    void E_timer()
    {
        time_End += 1f; //每次呼叫倒數的時間就扣1
        End.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, time_End);

        if (time_End == 255)
        {
            CancelInvoke("E_timer"); //取消重複呼叫timer函式。(停止倒數計時)
            //CancelInvoke取消重複呼叫(“函式名”)
        }
    }

    //當觸發器碰撞到BOSS時
    void OnTriggerEnter2D(Collider2D other)
    {
        //如果該觸發器的標籤是"Player Trigger"時
        if (other.gameObject.tag == "Player Trigger")
        {
            de_onoff = true; //開啟攻擊距離偵測
        }
    }

    //當觸發器離開BOSS時
    void OnTriggerExit2D(Collider2D other)
    {
        //如果該觸發器的標籤是"Player Trigger"時
        if (other.gameObject.tag == "Player Trigger")
        {
            de_onoff = false; //開啟攻擊距離偵測
        }
    }

    //從UseItem取得資料
    public static void SMslot(Slot y)
    {
        if (y != null)
        {
            if (y.slotImage.sprite != null)
            {
                weapon_config = y;
            }
            else
            {
                weapon_config = null;
            }
        }
        else
        {
            weapon_config = null;
        }
    }
}