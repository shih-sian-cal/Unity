using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Action : MonoBehaviour
{
    public static float Player_Life_Nun; //玩家血量
    public static Rigidbody2D m_Rigidbody2D; //修正角色撞牆不會異常抖動的變數
    private Animator m_Animator; //角色做動作給予動畫的變數
    private SpriteRenderer m_SpriteRenderer; //讓角色左右移動可以轉向的變數
    public float moveSpeed = 3.5f; //可更改跑速的變數
    public float jumpForce = 300.0f; //可更改跳躍力的變數
    private Vector2 moveDir; //修正角色移動後會墜落異常的變數
    public bool isGrounded = false; //判斷角色是否在地面的變數
    private GameObject groundedObject; //判斷角色站在哪種物件的變數
    public static GameObject PlayerHP;
    private Slider UI_Life; //玩家血量條
    private GameObject Fill_Area;
    float time_float = 1f; //受傷時的無敵秒數
    float Dtime_float = 1f; //受傷變色秒數
    float Mtime_float = 60f; //生成怪物時間
    int Lifetime_int = 5; //5秒後不受傷
    bool Life_onoff = false; //偵測血量是否滿血狀態
    public GameObject WB; //偵測工作檯
    public GameObject WB2; //偵測工作檯
    public GameObject WB3; //偵測工作檯
    public GameObject WB4; //偵測工作檯
    public GameObject WBP;
    public static bool WB_onoff = false;
    public GameObject SetUp;
    public static bool isSetUp;
    public GameObject SPWAN;
    public static bool BOSSp_onoff = false;

    private Transform playerTransform;

    public List<GameObject> Monster = new List<GameObject>();
    public static int Monster_Max = 0;

    //程式開始執行後的函式
    void Start()
    {
        Player_Life_Nun = 1;
        //取得Rigidbody2D數據
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        //取得Animator數據
        m_Animator = GetComponent<Animator>();
        //取得SpriteRenderer數據
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        if (GameObject.Find("Player_Life") != null)
        {
            UI_Life = GameObject.Find("Player_Life").GetComponent<Slider>();
            PlayerHP = GameObject.Find("Player_Life");
        }

        if (GameObject.Find("Fill Area") != null)
        {
            Fill_Area = GameObject.Find("Fill Area");
        }

        //查詢名叫橘貓(角色)的物件
        if (GameObject.Find("橘貓(角色)") != null)
        {
            playerTransform = GameObject.Find("橘貓(角色)").transform; //取得玩家的Transform
        }
    }

    //程式開始執行後，會以每幀執行一次的函式
    void Update()
    {
        //查詢名叫橘貓(角色)的物件
        if (GameObject.Find("橘貓(角色)") != null)
        {
            playerTransform = GameObject.Find("橘貓(角色)").transform; //取得玩家的Transform
        }
        else if (GameObject.Find("橘貓(角色)(Clone)") != null)
        {
            playerTransform = GameObject.Find("橘貓(角色)(Clone)").transform; //取得玩家的Transform
        }

        UI_Life.value = Player_Life_Nun;

        //如果血量小於0.0001時
        if (Player_Life_Nun < 0.0001)
        {
            SPWAN.SetActive(true);
            Time.timeScale = 0;
        }
        //如果角色血量不等於1，且血量偵測為關閉時
        else if (Player_Life_Nun <= 1 && Life_onoff == false)
        {
            InvokeRepeating("Life_timer", 1, 1); //一秒後，每秒重複呼叫Life_timer函式。(開始倒數計時)。
            //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)。

            Life_onoff = true; //開啟血量偵測
        }

        //判斷角色是否在地面且給予動畫
        //m_Animator.SetBool ("isGrounded", isGrounded);

        //按住D或方向右鍵可以讓角色往右移動
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDir.x = moveSpeed;
            m_Animator.SetFloat ("play_move", 1);
            m_SpriteRenderer.flipX = false;
        }
        //按住A或方向左鍵可以讓角色往左移動
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir.x = -moveSpeed;
            m_Animator.SetFloat("play_move", 1);
            m_SpriteRenderer.flipX = true;
        }
        //當同時按住A鍵和D鍵或方向鍵左鍵和方向鍵右鍵時角色會直接凍住
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            moveDir.x = 0;
        }
        //放開A鍵、D鍵、方向左鍵或方向右鍵可以停止移動
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            moveDir = Vector2.zero;
            m_Animator.SetFloat("play_move", 0);
        }

        //按下空白鍵可以讓角色做跳躍動作
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            m_Rigidbody2D.AddForce(Vector2.up * jumpForce);
            //m_Animator.SetTrigger ("Jump");
        }

        if (Input.GetKeyDown(KeyCode.Escape) && UseBag.isbag == false)
        {
            isSetUp = !isSetUp;
            SetUp.SetActive(isSetUp);
            if (isSetUp == true)
            {
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
                Time.timeScale = 0;
            }
            else
            {
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                Time.timeScale = 1;
            }
        }

        //取得角色往下掉的數值
        moveDir.y = m_Rigidbody2D.velocity.y;
        m_Rigidbody2D.velocity = moveDir;

        if (Mtime_float == 60f)
        {
            InvokeRepeating("M_timer", 0, 0.5f); //0.5秒後，每0.5秒重複呼叫M_timer函式。(開始倒數計時)
                                                 //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)
        }
    }

    //判斷角色是否踩在Ground標籤物件上的函式
    void OnCollisionStay2D(Collision2D other)
    {
        //角色是否踩在Ground標籤物件上
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("BOSS") || other.gameObject.CompareTag("Workbench"))
        {
            //方便處理陣列用的迴圈
            foreach (ContactPoint2D element in other.contacts)
            {
                //判斷normal值是否讓角色站在物件上
                if (element.normal.y > 0.25f)
                {
                    isGrounded = true; //角色踩的物件為Ground
                    groundedObject = other.gameObject; //取得Ground物件
                    break; //跳出迴圈
                }
            }
        }

        //角色碰到工作檯時
        if (other.gameObject.CompareTag("Workbench"))
        {
            WB_onoff = true;
            if (UseBag.isbag == true)
            {
                WB.SetActive(true); //開啟工作檯介面
                WBP.SetActive(false);
                Workbench.Page = 1; //將工作檯頁數重置
            }
            else
            {
                WB.SetActive(false); //開啟工作檯介面
                WBP.SetActive(false);
            }
        }

        //當角色撞到怪物時且無敵時間倒數完後
        if (other.gameObject.tag == "Monster" && time_float == 1)
        {
            Player_Life_Nun -= 0.05f; //角色血量減少5
            Lifetime_int = 5; //受傷時重置恢復秒數
            Life_onoff = false;

            CancelInvoke("restore_timer"); //取消重複呼叫restore_timer函式。(停止倒數計時)
                                           //CancelInvoke取消重複呼叫(“函式名”)。

            m_SpriteRenderer.color = Color.gray;
            InvokeRepeating("D_timer", 0, 0.25f); //0秒後，每0.5秒重複呼叫D_timer函式。(開始倒數計時)
                                                  //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)

            InvokeRepeating("timer", 0, 0.5f); //每秒重複呼叫timer函式。(開始倒數計時)。
            //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)。
        }

        if (other.gameObject.tag == "BOSS" && time_float == 1)
        {
            Player_Life_Nun -= 0.10f; //角色血量減少10
            Lifetime_int = 5; //受傷時重置恢復秒數
            Life_onoff = false;

            CancelInvoke("restore_timer"); //取消重複呼叫restore_timer函式。(停止倒數計時)
                                           //CancelInvoke取消重複呼叫(“函式名”)。

            m_SpriteRenderer.color = Color.gray;
            InvokeRepeating("D_timer", 0, 0.25f); //0秒後，每0.5秒重複呼叫D_timer函式。(開始倒數計時)
                                                  //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)

            InvokeRepeating("timer", 0, 0.5f); //每秒重複呼叫timer函式。(開始倒數計時)。
            //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)。
        }
    }

    //自訂一個函式叫做timer。
    void timer()
    {
        time_float -= 0.5f; //每次呼叫倒數的時間就扣1。

        //如果倒數時間為0 (也就是時間到!)。
        if (time_float == 0)
        {
            CancelInvoke("timer"); //取消重複呼叫timer函式。(停止倒數計時)
            //CancelInvoke取消重複呼叫(“函式名”)。

            time_float = 1; //重置無敵秒數
        }
    }

    //自訂一個函式叫做Life_timer。
    void Life_timer()
    {
        Lifetime_int -= 1; //每次呼叫倒數的時間就扣1。

        //如果倒數時間為0 (也就是時間到!)。
        if (Lifetime_int <= 0)
        {
            CancelInvoke("Life_timer"); //取消重複呼叫Life_timer函式。(停止倒數計時)
                                        //CancelInvoke取消重複呼叫(“函式名”)。

            InvokeRepeating("restore_timer", 1f, 1f); //一秒後，每秒重複呼叫restore_timer函式。(開始倒數計時)。
                                                      //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)。
        }
    }

    //自訂一個函式叫做restore_timer。
    void restore_timer()
    {
        if (Lifetime_int == 0)
        {
            Player_Life_Nun += 0.01f; //角色血量增加1
        }
        
        //如果角色血量為滿血狀態時
        if (Player_Life_Nun >= 1)
        {
            CancelInvoke("restore_timer"); //取消重複呼叫restore_timer函式。(停止倒數計時)
                                           //CancelInvoke取消重複呼叫(“函式名”)。

            Life_onoff = false; //關閉血量偵測
        }
    }

    //自訂一個函式叫做D_timer(受傷)
    void D_timer()
    {
        Dtime_float -= 0.5f; //每次呼叫倒數的時間就扣0.5

        if (Dtime_float == 0)
        {
            m_SpriteRenderer.color = new Color(255, 255, 255, 255);
            CancelInvoke("D_timer"); //取消重複呼叫timer函式。(停止倒數計時)
            //CancelInvoke取消重複呼叫(“函式名”)
            Dtime_float = 1f;
        }
    }

    //自訂一個函式叫做M_timer(怪物)
    void M_timer()
    {
        Mtime_float -= 1f; //每次呼叫倒數的時間就扣0.5

        if (Mtime_float == 0)
        {
            CancelInvoke("M_timer"); //取消重複呼叫timer函式。(停止倒數計時)
            //CancelInvoke取消重複呼叫(“函式名”)
            if (Monster_Max <= 2)
            {
                Instantiate(Monster[Random.Range(0, 2)], new Vector3(playerTransform.position.x + 10, 19.23556f, playerTransform.transform.localScale.z), transform.rotation);
                Monster_Max++;
            }
            
            Mtime_float = 60f; //重置秒數
        }
    }

    //判斷角色是否離地的函式
    void OnCollisionExit2D(Collision2D other)
    {
        //判斷是否為離地物件
        if (other.gameObject == groundedObject)
        {
            groundedObject = null; // 離地後設為空值
            isGrounded = false; // 角色離地的物件為Ground
        }

        //角色離開工作檯時
        if (other.gameObject.CompareTag("Workbench"))
        {
            WB_onoff = false;
            if (UseBag.isbag == true)
            {
                WB.SetActive(false); //工作檯介面關閉
                WB2.SetActive(false);
                WB3.SetActive(false);
                WB4.SetActive(false);
                WBP.SetActive(true);
            }
            else
            {
                WB.SetActive(false); //工作檯介面關閉
                WB2.SetActive(false);
                WB3.SetActive(false);
                WB4.SetActive(false);
                WBP.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal")
        {
            collision.gameObject.transform.GetComponent<Portal>().ChangeScene();
            if (BOSSp_onoff == true)
            {
                this.transform.position = new Vector3(8, 23, 0);
            }
        }
        //當角色撞到怪物時且無敵時間倒數完後
        else if (collision.gameObject.tag == "Monster" && time_float == 1)
        {
            Player_Life_Nun -= 0.05f; //角色血量減少5
            Lifetime_int = 5; //受傷時重置恢復秒數
            Life_onoff = false;

            CancelInvoke("restore_timer"); //取消重複呼叫restore_timer函式。(停止倒數計時)
                                           //CancelInvoke取消重複呼叫(“函式名”)。

            m_SpriteRenderer.color = Color.gray;
            InvokeRepeating("D_timer", 0, 0.25f); //0秒後，每0.5秒重複呼叫D_timer函式。(開始倒數計時)
                                                  //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)

            InvokeRepeating("timer", 0, 0.5f); //每秒重複呼叫timer函式。(開始倒數計時)。
            //InokeRepeating 重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)。
        }
    }
}