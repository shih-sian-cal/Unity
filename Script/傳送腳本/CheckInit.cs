using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckInit : MonoBehaviour
{
    public static string debugSceneName;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameObject.Find("橘貓(角色)"))
        {
            SceneManager.LoadScene("Init");
            debugSceneName = SceneManager.GetActiveScene().name;
        }
    }
}
