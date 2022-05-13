using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public static Slot weapon_config; //判斷武器圖片
    public int hp = 0; //怪物血量
    public int max_hp = 0; //怪物最大血量
    public GameObject Life; //怪物血量條
    private bool de_onoff = false; //判斷攻擊距離
    private Rigidbody2D m_rigidbody2D; //修正角色撞牆不會異常抖動的變數
    private Animator m_Animator; //動畫
    private SpriteRenderer spriterenderer;
    private Vector2 moveDir;
    float time_float = 1f;

    public enum Status { idle, walk }; //宣告一個狀態變數
    public bool IsFaceRight; //讓怪物水平翻轉
    public Status status; //判斷用
    public enum Face { Right, Left }; //宣告一個狀態變數
    public Face face; //判斷用

    public float speed;
    public float jumpForce = 250.0f; //可更改跳躍力的變數
    private Transform myTransform;

    private Transform playerTransform;

    public bool walk_TF = false;

    public Color color = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        spriterenderer = this.GetComponent<SpriteRenderer>(); //取得SpriteRenderer                                                   
        m_rigidbody2D = GetComponent<Rigidbody2D>(); //取得Rigidbody2D
        m_Animator = GetComponent<Animator>(); //取得Animator
        max_hp = 10; //怪物最大血量為10
        hp = max_hp; //怪物血量為最大血量
        status = Status.idle; //怪物狀態為等待狀態

        //判斷怪物面向
        if (spriterenderer.flipX)
        {
            face = Face.Right; //怪物面向右邊
        }
        else
        {
            face = Face.Left; //怪物面向左邊
        }

        myTransform = this.transform; //取得怪物的Transform

        //查詢名叫橘貓(角色)的物件
        if (GameObject.Find("橘貓(角色)") != null)
        {
            playerTransform = GameObject.Find("橘貓(角色)").transform; //取得玩家的Transform
        }
        color.a = 0.5f;
    }

    //每幀執行函式
    void Update()
    {
        //如果怪物血量小於等於0時
        if (hp <= 0)
        {
            Destroy(this.gameObject); //刪除怪物物件
            Player_Action.Monster_Max--;
        }

        //計算血量條
        float _percent = ((float)hp / (float)max_hp); 
        Life.transform.localScale = new Vector3(_percent, Life.transform.localScale.y, Life.transform.localScale.z);

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
                    hp -= 8; //怪物血量扣7

                    Mondamage();
                }
                else if (weapon_name == "鐵劍" || weapon_name == "斧頭")
                {
                    hp -= 5; //怪物血量扣5

                    Mondamage();
                }
                else if (weapon_name == "石劍" || weapon_name == "鐵斧")
                {
                    hp -= 3; //怪物血量扣3

                    Mondamage();
                }
                else if (weapon_name == "木劍" || weapon_name == "石斧" || weapon_name == "十字鎬")
                {
                    hp -= 2; //怪物血量扣2

                    Mondamage();
                }
                else if (weapon_name == null)
                {
                    return;
                }
                else
                {
                    hp -= 1; //怪物血量扣1

                    Mondamage();
                }
            }
        }

        float deltaTime = Time.deltaTime;

        //更新判斷怪物狀態
        switch (status)
        {
            //怪物狀態為等待
            case Status.idle:
                //判斷玩家Transform
                if (playerTransform)
                {
                    //如果玩家目前距離減怪物目前距離小於絕對值5
                    if(Mathf.Abs(myTransform.position.x - playerTransform.position.x) < 5f)
                    {
                        //怪物狀態轉為移動狀態
                        status = Status.walk;
                    }
                }
                break;
            //怪物狀態為移動
            case Status.walk:
                //判斷玩家Transform
                if (playerTransform)
                {
                    //如果怪物目前距離大於等於玩家目前距離
                    if (myTransform.position.x >= playerTransform.position.x)
                    {
                        //怪物面向左邊
                        spriterenderer.flipX = false;
                        face = Face.Left;
                    }
                    //否則怪物目前距離大於等於玩家目前距離
                    else
                    {
                        //怪物面向右邊
                        spriterenderer.flipX = true;
                        face = Face.Right;
                    }
                }
                //更新判斷怪物面向
                switch (face)
                {
                    //怪物面向為右邊
                    case Face.Right:
                        moveDir.x = speed; //速度往右增加
                        if (this.gameObject.name == "椰子猴(Clone)")
                        {
                            m_Animator.SetFloat("椰子猴(移動)", 1);
                        }
                        else if (this.gameObject.name == "穴蜘蛛(Clone)")
                        {
                            m_Animator.SetFloat("穴蜘蛛(移動)", 1);
                        }
                        else if (this.gameObject.name == "幽靈貓(Clone)" || this.gameObject.name == "幽靈貓")
                        {
                            m_Animator.SetFloat("幽靈貓(移動)", 1);
                            if (myTransform.position.y > playerTransform.position.y){
                                m_rigidbody2D.gravityScale = 1f;
                            }
                            else if (myTransform.position.y == playerTransform.position.y){
                                m_rigidbody2D.gravityScale = 0f;
                            }
                            else if (myTransform.position.y < playerTransform.position.y){
                                m_rigidbody2D.gravityScale = -1f;
                            }
                            walk_TF = false;
                        }
                        //如果怪物速度為0
                        if (m_rigidbody2D.velocity == Vector2.zero)
                        {
                            if (this.gameObject.name == "椰子猴(Clone)")
                            {
                                m_Animator.SetFloat("椰子猴(移動)", 0);
                            }
                            else if (this.gameObject.name == "穴蜘蛛(Clone)")
                            {
                                m_Animator.SetFloat("穴蜘蛛(移動)", 0);
                            }
                            else if (this.gameObject.name == "幽靈貓(Clone)" || this.gameObject.name == "幽靈貓")
                            {
                                m_Animator.SetFloat("幽靈貓(移動)", 0);
                                if (myTransform.position.y == playerTransform.position.y){
                                    m_rigidbody2D.gravityScale = 0f;
                                }
                            }
                            m_rigidbody2D.AddForce(Vector2.up * jumpForce); //怪物進行跳躍動作
                        }
                        break;
                    //怪物面向為左邊
                    case Face.Left:
                        moveDir.x = -speed; //速度往左增加
                        if (this.gameObject.name == "椰子猴(Clone)")
                        {
                            m_Animator.SetFloat("椰子猴(移動)", 1);
                        }
                        else if (this.gameObject.name == "穴蜘蛛(Clone)")
                        {
                            m_Animator.SetFloat("穴蜘蛛(移動)", 1);
                        }
                        else if (this.gameObject.name == "幽靈貓(Clone)" || this.gameObject.name == "幽靈貓")
                        {
                            m_Animator.SetFloat("幽靈貓(移動)", 1);
                            if (myTransform.position.y >= playerTransform.position.y){
                                m_rigidbody2D.gravityScale = 1f;
                            }
                            else if (myTransform.position.y == playerTransform.position.y){
                                m_rigidbody2D.gravityScale = 0f;
                            }
                            else if (myTransform.position.y < playerTransform.position.y){
                                m_rigidbody2D.gravityScale = -1f;
                            }
                            walk_TF = false;
                        }
                        //如果怪物速度為0
                        if (m_rigidbody2D.velocity == Vector2.zero)
                        {
                            if (this.gameObject.name == "椰子猴(Clone)")
                            {
                                m_Animator.SetFloat("椰子猴(移動)", 0);
                            }
                            else if (this.gameObject.name == "穴蜘蛛(Clone)")
                            {
                                m_Animator.SetFloat("穴蜘蛛(移動)", 0);
                            }
                            else if (this.gameObject.name == "幽靈貓(Clone)" || this.gameObject.name == "幽靈貓")
                            {
                                m_Animator.SetFloat("幽靈貓(移動)", 0);
                                if (myTransform.position.y == playerTransform.position.y){
                                    m_rigidbody2D.gravityScale = 0f;
                                }
                            }
                            m_rigidbody2D.AddForce(Vector2.up * jumpForce); //怪物進行跳躍動作
                        }
                        break;
                }
                //判斷玩家Transform
                if (playerTransform)
                {
                    //如果玩家目前距離減怪物目前距離大於等於絕對值5
                    if (Mathf.Abs(myTransform.position.x - playerTransform.position.x) >= 5f)
                    {
                        status = Status.idle; //怪物狀態轉為等待狀態
                        moveDir = Vector2.zero; //怪物移動速度為0
                        m_Animator.SetFloat("椰子猴(移動)", 0);
                        m_Animator.SetFloat("穴蜘蛛(移動)", 0);
                        m_Animator.SetFloat("幽靈貓(移動)", 0);
                        if (this.gameObject.name == "幽靈貓(Clone)" || this.gameObject.name == "幽靈貓"){
                            walk_TF = true;
                            m_rigidbody2D.gravityScale = 0f;
                        }
                    }
                }
                break;
        }

        //取得角色往下掉的數值
        moveDir.y = m_rigidbody2D.velocity.y;
        m_rigidbody2D.velocity = moveDir;
    }

    //怪物受傷特效
    void Mondamage()
    {
        if (this.gameObject.name == "幽靈貓(Clone)" || this.gameObject.name == "幽靈貓"){
            spriterenderer.color = color;
        }
        else{
            spriterenderer.color = Color.gray;
        }

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

    //當觸發器碰撞到怪物時
    void OnTriggerEnter2D(Collider2D other)
    {
        //如果該觸發器的標籤是"Player Trigger"時
        if (other.gameObject.tag == "Player Trigger")
        {
            de_onoff = true; //開啟攻擊距離偵測
        }
                
        if (other.gameObject.tag == "Ground" && walk_TF == true)
        {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //如果該觸發器的標籤是"Player Trigger"時
        if (other.gameObject.tag == "Player Trigger")
        {
            de_onoff = false; //開啟攻擊距離偵測
        }

        if (other.gameObject.tag == "Ground" && walk_TF == false)
        {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
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