using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class SoundSettingsData
{
    public float bgmVolume = 0.5f;
    public float seVolume = 0.5f;
}

// Script for managing audio　オーディオを管理するスクリプト
public class AudioManager : PersistentSingleton<AudioManager>
{
    [Serializable]
    public class AudioData
    {
        public string name; // Name (for clarity)　名前（分かりやすくするように）
        public AudioClip clip; // mp3 format　mp3形式
    }

    [Header("Text")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI bgmVolume;
    [SerializeField] private TextMeshProUGUI seVolume;

    [Header("BGM")]
    public  AudioData[] bgms;

    [Header("SE")]
    public AudioData[] ses;

    private Dictionary<string, AudioClip> bgmDictionary;
    private Dictionary<string, AudioClip> seDictionary;

    private AudioSource bgmSource;  // AudioSource dedicated to BGM　BGM専用AudioSource
    private AudioSource seSource;   // SE-exclusive AudioSource　SE専用AudioSource

    private GameObject mainCamera;  // Object (Main Camera)　オブジェクト(Main Camera)

    private string saveFilePath;

    protected override void Awake()
    {
        // Singleton verification　シングルトン確認
        base.Awake();

        if (AudioManager.Instance != this) return;

        this.saveFilePath = Path.Combine(Application.persistentDataPath, "AudioVolume.json");

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

        LoadSoundSettings();
    }

    void Start()
    {
        
    }
    void Update()
    {
        if (this.mainCamera == null)
        {
            this.mainCamera = GameObject.Find("Main Camera");
        }
        this.transform.position = this.mainCamera.transform.position;

        if (this.canvas.enabled)
        {
            this.bgmVolume.text = bgmSource.volume.ToString();
            this.seVolume.text = seSource.volume.ToString();
        }

        if ((Mouse.current != null) && (Mouse.current.middleButton.wasPressedThisFrame))
        {
            if (this.canvas.enabled) this.canvas.enabled = false;
            else this.canvas.enabled = true;
        }
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

    public void SetBGMVolume(bool isAdd)
    {
        if (isAdd) this.bgmSource.volume += 0.1f;
        else this.bgmSource.volume -= 0.1f;

        this.bgmSource.volume = Mathf.Round(this.bgmSource.volume * 10.0f) / 10.0f;

        this.bgmSource.volume = Mathf.Clamp(this.bgmSource.volume, 0.0f, 1.0f);

        SaveSoundSettings();
    }

    // SE
    public void PlaySE(string name)
    {
        if (!this.seDictionary.TryGetValue(name, out AudioClip clip)) return;

        this.seSource.PlayOneShot(clip);
    }

    public void SetSEVolume(bool isAdd)
    {
        if (isAdd) this.seSource.volume += 0.1f;
        else this.seSource.volume -= 0.1f;

        this.seSource.volume = Mathf.Round(this.seSource.volume * 10.0f) / 10.0f;

        this.seSource.volume = Mathf.Clamp(this.seSource.volume, 0.0f, 1.0f);

        SaveSoundSettings();
    }

    private void SaveSoundSettings()
    {
        SoundSettingsData data = new SoundSettingsData();
        data.bgmVolume = this.bgmSource.volume;
        data.seVolume = this.seSource.volume;

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(this.saveFilePath, json);
    }

    private void LoadSoundSettings()
    {
        if (File.Exists(this.saveFilePath))
        {
            string json = File.ReadAllText(this.saveFilePath);
            SoundSettingsData data = JsonUtility.FromJson<SoundSettingsData>(json);

            this.bgmSource.volume = data.bgmVolume;
            this.seSource.volume = data.seVolume;
        }
        else
        {
            this.bgmSource.volume = 0.5f;
            this.seSource.volume = 0.5f;
            SaveSoundSettings();
        }
    }
}
