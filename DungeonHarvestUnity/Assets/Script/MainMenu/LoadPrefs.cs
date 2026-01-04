using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrefs : MonoBehaviour
{
    [SerializeField] private bool defaultAll = false;

    [SerializeField] private settingManager settingsManager;

    [Header("Display Mode")]
    [SerializeField] private Button fullscreenBtn;
    [SerializeField] private Button windowedBtn;
    [SerializeField] private Button borderlessBtn;
    [SerializeField] private string defaultScreenMode = "Fullscreen";

    [Header("Mouse")]
    [SerializeField] private Slider mouseSlider;
    [SerializeField] private float defaultMouseSensitivity = 1f;

    [Header("Audio")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private float defaultMasterVolume = 100f;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private float defaultMusicVolume = 100f;

    [SerializeField] private Slider vfxSlider;
    [SerializeField] private float defaultVFXVolume = 100f;

    private void Start()
    {
        if (defaultAll)
            ResetToDefault();

        // Display Mode
        string mode = PlayerPrefs.GetString("DisplayMode", defaultScreenMode);

        if (mode == "Fullscreen")
            settingsManager.SelectDisplayMode(fullscreenBtn);
        else if (mode == "Windowed")
            settingsManager.SelectDisplayMode(windowedBtn);
        else if (mode == "Borderless")
            settingsManager.SelectDisplayMode(borderlessBtn);

        // Mouse
        mouseSlider.value = PlayerPrefs.GetFloat(
            "MouseSensitivity",
            defaultMouseSensitivity
        );

        // Audio
        masterSlider.value = PlayerPrefs.GetFloat(
            "MasterVolume",
            defaultMasterVolume
        );

        musicSlider.value = PlayerPrefs.GetFloat(
            "MusicVolume",
            defaultMusicVolume
        );

        vfxSlider.value = PlayerPrefs.GetFloat(
            "VFXVolume",
            defaultVFXVolume
        );
    }

    private void ResetToDefault()
    {
        PlayerPrefs.SetString("DisplayMode", defaultScreenMode);
        PlayerPrefs.SetFloat("MouseSensitivity", defaultMouseSensitivity);
        PlayerPrefs.SetFloat("MasterVolume", defaultMasterVolume);
        PlayerPrefs.SetFloat("MusicVolume", defaultMusicVolume);
        PlayerPrefs.SetFloat("VFXVolume", defaultVFXVolume);
        PlayerPrefs.Save();
    }
}
