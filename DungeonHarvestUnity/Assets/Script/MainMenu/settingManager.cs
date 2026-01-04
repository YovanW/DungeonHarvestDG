using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class settingManager : MonoBehaviour
{
    public GameObject player;

    [Header("Display Mode")]
    public Button fullscreenBtn;
    public Button windowedBtn;
    public Button borderlessBtn;
    private List<Button> displayButtons;

    [Header("Mouse")]
    [SerializeField] private TextMeshProUGUI mouseValueText;
    [SerializeField] private Slider mouseSlider;

    [Header("Audio")]
    public AudioMixer audioMixer;

    [SerializeField] private TextMeshProUGUI masterValueText;
    [SerializeField] private Slider masterSlider;

    [SerializeField] private TextMeshProUGUI musicValueText;
    [SerializeField] private Slider musicSlider;

    [SerializeField] private TextMeshProUGUI vfxValueText;
    [SerializeField] private Slider vfxSlider;

    void Awake()
    {
        displayButtons = new List<Button>
        {
            fullscreenBtn,
            windowedBtn,
            borderlessBtn
        };
    }

    void Start()
    {
        fullscreenBtn.onClick.AddListener(() => SelectDisplayMode(fullscreenBtn));
        windowedBtn.onClick.AddListener(() => SelectDisplayMode(windowedBtn));
        borderlessBtn.onClick.AddListener(() => SelectDisplayMode(borderlessBtn));

        mouseSlider.onValueChanged.AddListener(SetMouseSensitivity);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        vfxSlider.onValueChanged.AddListener(SetVFXVolume);
    }

    // ================= DISPLAY MODE =================
    public void SelectDisplayMode(Button selected)
    {
        foreach (var btn in displayButtons)
            btn.interactable = true;

        selected.interactable = false;

        if (selected == fullscreenBtn)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            PlayerPrefs.SetString("DisplayMode", "Fullscreen");
        }
        else if (selected == windowedBtn)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            PlayerPrefs.SetString("DisplayMode", "Windowed");
        }
        else if (selected == borderlessBtn)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerPrefs.SetString("DisplayMode", "Borderless");
        }

        PlayerPrefs.Save();
    }

    // ================= MOUSE =================
    public void SetMouseSensitivity(float value)
    {
        mouseValueText.text = value.ToString("F2");

        if (player != null)
            player.GetComponent<FirstPersonController>().mouseSensitivity = value * 300f;

        PlayerPrefs.SetFloat("MouseSensitivity", value);
    }

    // ================= AUDIO =================
    public void SetMasterVolume(float value)
    {
        SetVolume("MasterVol", value);
        masterValueText.text = value.ToString("0");
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        SetVolume("MusicVol", value);
        musicValueText.text = value.ToString("0");
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetVFXVolume(float value)
    {
        SetVolume("VFXVol", value);
        vfxValueText.text = value.ToString("0");
        PlayerPrefs.SetFloat("VFXVolume", value);
    }

    private void SetVolume(string param, float sliderValue)
    {
        float normalized = sliderValue / 100f;
        float db = normalized <= 0f ? -80f : Mathf.Log10(normalized) * 20f;
        audioMixer.SetFloat(param, db);
    }
}
