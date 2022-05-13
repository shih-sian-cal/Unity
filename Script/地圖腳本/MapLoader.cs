using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorToPrefab
{
    public Color32 color;
    public GameObject prefab;
}

public class MapLoader : MonoBehaviour
{
    public string LevelFileName;

    //public Texture2D LevelMap;

    public ColorToPrefab[] colorToPrefab;

    // Use this for initialization
    // 使用它進行初始化
    void Start()
    {
        // 進行地圖加載與生成
        LoadMap();
    }

    // Get the raw pixels from the level imagemap
    // 從關卡圖像貼圖獲取原始像素
    void EmptyMap()
    {
        // Find all of our children and...eliminate them.
        // 找到我們所有的孩子並...消除他們。

        while (transform.childCount > 0)
        {
            Transform c = transform.GetChild(0);
            c.SetParent(null);// become Batman
            Destroy(c.gameObject);// become The Joker
        }
    }
    void LoadAllLevelNames()
    {
        // Read the list of files from streamingAssets/Levels/*.png
        // 從中讀取文件列表streamingAssets/Levels/*.png
        // The player will progess through the levels alphabetically
        // 玩家將按字母順序進行遊戲
    }

    private void LoadMap()
    {
        // 清空已存在的地圖對象
        EmptyMap();

        // Read the image data from the file in /2D沙盒生存/Resources/Map/
        // 從/2D沙盒生存/Resources/Map/中的文件中讀取圖像數據
        string filePath = Application.dataPath + "/Resources/Map/" + LevelFileName;
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
        Texture2D LevelMap = new Texture2D(2, 2);
        LevelMap.LoadImage(bytes);

        // Get the raw pixels from the level imagemap
        // 從關卡圖像貼圖獲取原始像素
        Color32[] allPixels = LevelMap.GetPixels32();
        int width = LevelMap.width;
        int height = LevelMap.height;

        // 遍歷所有像素值
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                SpawnTileAt( allPixels[(j * width) + i], i, j);
            }
        }
    }

    void SpawnTileAt( Color32 c, int x, int y)
    {
        // If this is a transparent pixel, then it's meant to just be empty
        // 如果這是一個透明像素，則意味著它是空的
        if (c.a <= 0)
        {
            return;
        }

        // Find the right color in our map
        // 在我們的地圖上找到合適的顏色

        // NOTE: This isn't optimized. You should have a dictionary lookup for max speed
        // 注意：這不是最佳的。您應該對最大速度進行字典查找
        foreach (ColorToPrefab ctp in colorToPrefab)
        {
            if (c.Equals(ctp.color))
            {
                // Spawn the prefab at the right location
                // 在正確的位置生成預製件

                GameObject go = (GameObject)Instantiate(ctp.prefab, new Vector3(x, y, 0), Quaternion.identity);
                go.transform.SetParent(this.transform);
                // maybe do more stuff to the gamebject here?
                // 也許這裡有更多東西要玩遊戲？
                return;
            }
        }

        // If we got to this point, it means we did not find a matching color in our array.
        // 如果到了這一點，這意味著我們在數組中找不到匹配的顏色。
        Debug.LogError("No color to prefab found for: " + c.ToString());
    }
}