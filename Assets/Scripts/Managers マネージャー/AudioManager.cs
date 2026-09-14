using System;
using System.Collections.Generic;
using UnityEngine;

// Script for managing audio　オーディオを管理するスクリプト
public class AudioManager : PersistentSingleton<AudioManager>
{
    [Serializable]
    public class AudioData
    {
        public string name; // Name (for clarity)　名前（分かりやすくするように）
        public AudioClip clip; // mp3 format　mp3形式
    }

    [Header("BGM")]
    public  AudioData[] bgms;

    [Header("SE")]
    public AudioData[] ses;

    private Dictionary<string, AudioClip> bgmDictionary;
    private Dictionary<string, AudioClip> seDictionary;

    private AudioSource bgmSource;  // AudioSource dedicated to BGM　BGM専用AudioSource
    private AudioSource seSource;   // SE-exclusive AudioSource　SE専用AudioSource

    private GameObject mainCamera;  // Object (Main Camera)　オブジェクト(Main Camera)

    protected override void Awake()
    {
        // Singleton verification　シングルトン確認
        base.Awake();

        if (AudioManager.Instance != this) return;

        this.bgmDictionary = new Dictionary<string, AudioClip>();

        foreach (var bgm in this.bgms)
        {
            this.bgmDictionary[bgm.name] = bgm.clip;
        }

        this.seDictionary = new Dictionary<string, AudioClip>();

        foreach (var se in this.ses)
        {
            this.seDictionary[se.name] = se.clip;
        }

        AudioSource[] sources = GetComponents<AudioSource>();

        this.bgmSource = sources[0];
        this.seSource = sources[1];
    }


    void Update()
    {
        if (this.mainCamera == null)
        {
            this.mainCamera = GameObject.Find("Main Camera");
        }
        this.transform.position = this.mainCamera.transform.position;
    }
    // BGM
    public void PlayBGM(string name)
    {
        if (!this.bgmDictionary.TryGetValue(name, out AudioClip clip)) return;

        this.bgmSource.clip = clip;
        this.bgmSource.loop = true;
        this.bgmSource.Play();
    }

    public void StopBGM()
    {
        this.bgmSource.Stop();
    }
    // SE
    public void PlaySE(string name)
    {
        if (!this.seDictionary.TryGetValue(name, out AudioClip clip)) return;

        this.seSource.PlayOneShot(clip);
    }
}
