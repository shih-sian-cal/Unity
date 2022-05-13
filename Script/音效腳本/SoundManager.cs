using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour
{
    public AudioSource m_MyAudioSource;
    public AudioClip shootSound;
    public GameObject sound_value;
    public GameObject[] sound = new GameObject[10];
    private GameObject gt;
    int temp = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();    //獲取音訊源元件
    }

    void Update()
    {
        if (UseItem.sound_type != 0)
        {
            if (GameObject.Find("pcut_Sound(Clone)") != null)
            {
                gt = GameObject.Find("pcut_Sound(Clone)");
                Destroy(gt);
                sound[UseItem.sound_type].GetComponent<AudioSource>().volume = sound_value.GetComponent<AudioSource>().volume;
                Instantiate(sound[UseItem.sound_type], transform.position, transform.rotation); //在空物件座標上生成指定物件
            }
            else{
                sound[UseItem.sound_type].GetComponent<AudioSource>().volume = sound_value.GetComponent<AudioSource>().volume;
                Instantiate(sound[UseItem.sound_type], transform.position, transform.rotation); //在空物件座標上生成指定物件
            }
        }
    }
}