using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseBag : MonoBehaviour
{
    public GameObject bag;
    public GameObject WB; //偵測工作檯
    public GameObject WB2; //偵測工作檯
    public GameObject WB3; //偵測工作檯
    public GameObject WB4; //偵測工作檯
    public GameObject WBP;
    public static bool isbag;

    // Update is called once per frame
    void Update()
    {
        //按住E鍵打開背包
        if (Input.GetKeyDown(KeyCode.E))
        {
            isbag = !isbag;
            bag.SetActive(isbag);
            InvertoryManager.UpdateItemInfo("");
            if (Player_Action.WB_onoff == true)
            {
                WB.SetActive(isbag);
                WB2.SetActive(false);
                WB3.SetActive(false);
                WB4.SetActive(false);
                Workbench.Page = 1; //將工作檯頁數重置
            }
            else
            {
                WBP.SetActive(isbag);
                Workbench.Page = 1; //將工作檯頁數重置
            }
        }
    }

    public void Bag_onoff()
    {
        isbag = false;
        if (Player_Action.WB_onoff == true)
        {
            WB.SetActive(false);
            WB2.SetActive(false);
            WB3.SetActive(false);
            WB4.SetActive(false);
            Workbench.Page = 1; //將工作檯頁數重置
        }
        else
        {
            WBP.SetActive(false);
            Workbench.Page = 1; //將工作檯頁數重置
        }
    }
}
