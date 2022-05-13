using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 場景切換管理類
/// </summary>
public class ScenesMgr : BaseManager<ScenesMgr>
{
    /// <summary>
    /// 場景切換 同步加載（場景加載完之後，再執行其餘相關的方法）
    /// </summary>
    /// <param name="sceneName">加載的場景</param>
    /// <param name="func">切換場景要執行的方法</param>
    public void LoadScene(string sceneName, UnityAction func)
    {
        // 切換場景
        SceneManager.LoadScene(sceneName);
        // 場景切換之後再執行相應的其餘的加載
        func();
    }
    /// <summary>
    /// 場景切換異步加載的封裝
    /// </summary>
    /// <param name="sceneName">加載的場景</param>
    /// <param name="func">切換場景要執行的方法</param>
    public void LoadSceneAsync(string sceneName, UnityAction func)
    {
        // 利用Mono管理器實现沒有繼承MonoBehavior開啟協程
        MonoMgr.GetInstance().StartCoroutine(LoadingSceneAsync(sceneName, func));

    }

    /// <summary>
    /// 場景切換 異步加載（場景加載的過程中，可以同時執行其餘相關的方法）
    /// </summary>
    /// <param name="sceneName">加載的場景</param>
    /// <param name="func">切換場景要執行的方法</param>
    public IEnumerator LoadingSceneAsync(string sceneName, UnityAction func)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        // 如果沒有場景加載完成,通過協程實現進度的更新，可以用於事件分發，供外部監聽
        while (!ao.isDone)
        {
            //EventCenter.GetInstance().EventAddListener("LoadScenePorgress", ao.progress);
            yield return ao.progress;
        }
        yield return ao;

        func();
    }
}