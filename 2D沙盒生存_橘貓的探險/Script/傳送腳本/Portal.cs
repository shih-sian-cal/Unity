using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneName;
    public static bool key_onoff = false;
    public GameObject tip;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.tag = "Portal";
        if (GameObject.Find("提醒字幕") != null)
        {
            tip = GameObject.Find("提醒字幕");
            tip.SetActive(false);
        }
    }

    public void ChangeScene()
    {
        if (key_onoff == true && sceneName == "BOSS")
        {
            SceneManager.LoadScene(sceneName);
            Player_Action.BOSSp_onoff = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (key_onoff == false)
        {
            tip.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        tip.SetActive(false);
    }
}
