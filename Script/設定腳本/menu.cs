using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite sp;
    private Sprite tsp;
    public GameObject SPWAN;
    private Vector3 SPWAN_pos = new Vector3(8, 23);
    public GameObject WB; //偵測工作檯
    public GameObject WB2; //偵測工作檯
    public GameObject WB3; //偵測工作檯
    public GameObject WB4; //偵測工作檯
    public GameObject WBP;
    public GameObject SetUp;
    public GameObject[] DG = new GameObject[5];
    public AudioClip shootSound;
    public AudioSource m_MyAudioSource;

     // Start is called before the first frame update
    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();    //獲取音訊源元件
        m_MyAudioSource.clip = shootSound;  
    }

    public void QuitGame()
    {
        for (int i = 0; i < 3; i++)
        {
            Destroy(DG[i]);
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("title");
        for (int i = 3; i < 8; i++)
        {
            Destroy(DG[i]);
        }
    }

    public void SPAWN_Player()
    {
        for (int i = 0; i < 3; i++)
        {
            Destroy(DG[i]);
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("Init");
        for (int i = 3; i < 8; i++)
        {
            Destroy(DG[i]);
        }
    }

    //滑鼠移入事件
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_MyAudioSource.Play();    //播放音效
        tsp = this.GetComponent<Image>().sprite;
        this.GetComponent<Image>().sprite = sp;
    }

    //滑鼠移出事件
    public void OnPointerExit(PointerEventData eventData)
    {
        sp = this.GetComponent<Image>().sprite;
        this.GetComponent<Image>().sprite = tsp;
    }
}