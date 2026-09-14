using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script for scene transitions　シーン遷移するためのスクリプト
public class SceneChangeManager : PersistentSingleton<SceneChangeManager>
{
    [Serializable]
    public class SceneData
    {
        public string sceneAsset; // Scene where it is called　呼び出すシーン

        public string sceneName; // Name (for clarity)　名前（分かりやすくするように）
    }

    [Header("Registering a scene「シーンの登録」")]
    public SceneData[] sceneDatas;

    [Header("UI Settings for Blackouts「暗転用UI設定」")]
    public Image texture;   // Image to display　表示する画像
    public float speed = 1.2f;  // speed　速度
    public float alphaMax = 1.0f;   // Maximum transparency (0-1)　最大透明度（0～1） 
    private bool isChanging = false;    // Flag indicating whether a scene transition is in progress　シーン切り替え中かのフラグ

    // シーンの名前
    private Dictionary<string, string> sceneChangeName;

    protected override void Awake()
    {
        // Singleton verification　シングルトン確認
        base.Awake();

        if (SceneChangeManager.Instance != this) return;

        this.sceneChangeName = new Dictionary<string, string>();

        foreach (var sceneData in this.sceneDatas)
        {
            // Register your name　名前を登録する
            this.sceneChangeName[sceneData.sceneName] = sceneData.sceneAsset;
        }

        if (this.texture != null)
        {
            Color color = this.texture.color;
            color.a = 0.0f;
            this.texture.color = color;
        }
    }

    // Function to check if a scene transition is in progress　シーン切り替え中か調べる関数
    public bool IsChanging()
    {
        return this.isChanging;
    }

    // A function that calls a specified scene　指定したシーンを呼び出す関数
    public void OnSceneChange(string changeSceneName)
    {
        if ((this.isChanging) || (!this.sceneChangeName.TryGetValue(changeSceneName, out string sceneName))) return;

        StartCoroutine(SceneChange(sceneName));
    }

    private System.Collections.IEnumerator SceneChange(string sceneName)
    {
        float timer = 0.0f;
        Color color = this.texture.color;
        this.isChanging = true;

        // Blackout 暗転
        while (color.a < this.alphaMax)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0.0f, this.alphaMax, timer * speed);
            this.texture.color = color;
            yield return null;
        }

        color.a = this.alphaMax;
        this.texture.color = color;

        yield return SceneManager.LoadSceneAsync(sceneName);

        // Lights up　明転
        timer = 0.0f;
        while (color.a > 0.0f)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(this.alphaMax, 0.0f, timer * speed);
            this.texture.color = color;
            yield return null;
        }

        color.a = 0.0f;
        this.texture.color = color;

        this.isChanging = false;
    }

}
