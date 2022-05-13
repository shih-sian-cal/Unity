using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //導入UI API。

public class FullScreenControl : MonoBehaviour {

    //宣告一個 Toggle UI。
    public Toggle SwitchOfScreen;

    void Update () {

        //若Toggle被勾選就切換成全螢幕狀態。
        if(SwitchOfScreen.isOn){
            Screen.fullScreen=true;
        }
        //若Toggle被取消勾選就切換成視窗狀態。
        else{
            Screen.fullScreen=false;
        }
    }
}