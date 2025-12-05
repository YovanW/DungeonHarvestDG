using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrefs : MonoBehaviour
{
    [SerializeField] private bool canUse = false;
    [SerializeField] private bool defaultAll = false;
    [SerializeField] private settingManager settingsManager;

    [Header("Screen Mode")]
    [SerializeField] private Button fullscreenBtn;
    [SerializeField] private Button windowedBtn;
    [SerializeField] private Button borderlessBtn;
    [SerializeField] private string screenMode;
    [SerializeField] private string defaultScreenMode = "Fullscreen";

    [Header("Mouse Sensitivity")]
    [SerializeField] private TextMeshProUGUI mouseValueText;
    [SerializeField] private Slider mouseSlider;
    [SerializeField] private float defaultMouseSensitivity = 1f;

    [Header("Master Volume")]
    [SerializeField] private TextMeshProUGUI masterValueText;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private float defaultMasterVolume = 100f;

    [Header("Music Volume")]
    [SerializeField] private TextMeshProUGUI musicValueText;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private float defaultMusicVolume = 100f;

    [Header("VFX Volume")]
    [SerializeField] private TextMeshProUGUI vfxValueText;
    [SerializeField] private Slider vfxSlider;
    [SerializeField] private float defaultVFXVolume = 100f;

    public void defaultAllSettings()
    {
        PlayerPrefs.SetString("DisplayMode", defaultScreenMode);
        PlayerPrefs.SetFloat("MouseSensitivity", defaultMouseSensitivity);
        PlayerPrefs.SetFloat("MasterVolume", defaultMasterVolume);
        PlayerPrefs.SetFloat("MusicVolume", defaultMusicVolume);
        PlayerPrefs.SetFloat("VFXVolume", defaultVFXVolume);

        PlayerPrefs.Save();
    }

    private void Awake()
    {
        if (defaultAll)
        {
            defaultAllSettings();
        }
        else if (canUse)
        {
            // Display Mode
            if (PlayerPrefs.HasKey("DisplayMode"))
            {
                screenMode = PlayerPrefs.GetString("DisplayMode");
                if (screenMode == "Fullscreen")
                {
                    Screen.fullScreen = true;
                    settingsManager.SelectDisplayMode(fullscreenBtn);
                }
                else if (screenMode == "Windowed")
                {
                    Screen.fullScreen = false;
                    settingsManager.SelectDisplayMode(windowedBtn);
                }
                else if (screenMode == "Borderless")
                {
                    settingsManager.SelectDisplayMode(borderlessBtn);
                    // TODO: implement borderless mode
                }
            }
            else
            {
                screenMode = defaultScreenMode;
                Screen.fullScreen = true;
            }

            // Mouse Sensitivity
            if (PlayerPrefs.HasKey("MouseSensitivity"))
            {
                float mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");

                // fail-safe check
                if (mouseSensitivity <= 0f || mouseSensitivity > 2f)
                    mouseSensitivity = defaultMouseSensitivity;

                settingsManager.SetMouseSensitivity(mouseSensitivity);
                mouseSlider.value = mouseSensitivity;
            }
            else
            {
                settingsManager.SetMouseSensitivity(defaultMouseSensitivity);
                mouseSlider.value = defaultMouseSensitivity;
            }

            // Master Volume
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                float masterVolume = PlayerPrefs.GetFloat("MasterVolume");
                settingsManager.SetMasterVolume(masterVolume);
                masterSlider.value = masterVolume;
            }
            else
            {
                settingsManager.SetMasterVolume(defaultMasterVolume);
                masterSlider.value = defaultMasterVolume;
            }

            // Music Volume
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
                settingsManager.SetMusicVolume(musicVolume);
                musicSlider.value = musicVolume;
            }
            else
            {
                settingsManager.SetMusicVolume(defaultMusicVolume);
                musicSlider.value = defaultMusicVolume;
            }

            // VFX Volume
            if (PlayerPrefs.HasKey("VFXVolume"))
            {
                float vfxVolume = PlayerPrefs.GetFloat("VFXVolume");
                settingsManager.SetVFXVolume(vfxVolume);
                vfxSlider.value = vfxVolume;
            }
            else
            {
                settingsManager.SetVFXVolume(defaultVFXVolume);
                vfxSlider.value = defaultVFXVolume;
            }
        }
    }
}
