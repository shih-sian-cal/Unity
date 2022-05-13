using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleSceneControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite Ssp;
    private Sprite Stsp;
    public Sprite Qsp;
    private Sprite Qtsp;
    public AudioClip shootSound;
    public AudioSource m_MyAudioSource;
    private int tf;

    void Start(){
        m_MyAudioSource = GetComponent<AudioSource>();    //獲取音訊源元件
        m_MyAudioSource.clip = shootSound;
    }

    public void GameStart()
    {
        //同Application.LoadLevel("game"); 載入場景
        SceneManager.LoadScene("Init");
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    //滑鼠移入事件
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "開始遊戲")
        {
            m_MyAudioSource.Play();    //播放音效
            Stsp = this.GetComponent<Image>().sprite;
            this.GetComponent<Image>().sprite = Ssp;
            tf = 1;
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "結束遊戲")
        {
            m_MyAudioSource.Play();    //播放音效
            Qtsp = this.GetComponent<Image>().sprite;
            this.GetComponent<Image>().sprite = Qsp;
            tf = 2;
        }
    }

    //滑鼠移出事件
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tf == 1)
        {
            Ssp = this.GetComponent<Image>().sprite;
            this.GetComponent<Image>().sprite = Stsp;
        }
        else if (tf == 2)
        {
            Qsp = this.GetComponent<Image>().sprite;
            this.GetComponent<Image>().sprite = Qtsp;
        }
    }
}