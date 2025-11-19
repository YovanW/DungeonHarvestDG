using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class settingManager : MonoBehaviour
{
    // Display Mode Buttons
    public Button fullscreenBtn;
    public Button windowedBtn;
    public Button borderlessBtn;
    private List<Button> displayButtons;

    // Gameplay settings
    [SerializeField] private TextMeshProUGUI mouseValueText;
    [SerializeField] private Slider mouseSlider;

    // Audio settings
    /*  AUDIO MIXER SETUP

        1. Buat Audio Mixer
           - Project → Create → Audio → Audio Mixer
           - Contoh nama: GameAudio

        2. Buat Mixer Group
           Di dalam mixer, buat tiga group:
             • Master
             • Music
             • VFX

           Struktur akhirnya:
             Master
             ├── Music
             └── VFX

        3. Pasang Mixer Group ke AudioSource
           - Pilih GameObject yang punya AudioSource
           - Pada AudioSource → Output pilih:
               • BGM / lagu          → Music
               • Semua sound effect  → VFX
                 (footstep, UI click, hit, explosion, magic, wind, dll)

           Catatan:
           - Semua SFX masuk ke VFX biar gampang dikontrol satu slider.

        4. Expose Volume Parameters
           - Di Audio Mixer, klik tombol knob Volume pada tiap group
           - Klik kanan → Expose Volume
           - Ganti nama parameter jadi:
               • MasterVol
               • MusicVol
               • VFXVol

        5. Hubungkan Slider ke Audio Mixer (di script)
           - Gunakan mixer.SetFloat("MusicVol", valueDalamDb);
           - Konversi slider (0–1) ke dB memakai:
               Mathf.Log10(sliderValue) * 20

        6. Nilai umum:
           - Volume normal = 0 dB
           - Mute          = -80 dB
    */
    public AudioMixer audioMixer = null; // belum dibuat

    [SerializeField] private TextMeshProUGUI masterValueText;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private TextMeshProUGUI musicValueText;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI vfxValueText;
    [SerializeField] private Slider vfxSlider;

    void Start()
    {
        displayButtons = new List<Button> { fullscreenBtn, windowedBtn, borderlessBtn };
        fullscreenBtn.onClick.AddListener(() => SelectDisplayMode(fullscreenBtn));
        windowedBtn.onClick.AddListener(() => SelectDisplayMode(windowedBtn));
        borderlessBtn.onClick.AddListener(() => SelectDisplayMode(borderlessBtn));

        SelectDisplayMode(fullscreenBtn);   // default display mode

        mouseSlider.onValueChanged.AddListener(SetMouseSensitivity);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        vfxSlider.onValueChanged.AddListener(SetVFXVolume);
    }

    void SelectDisplayMode(Button selected)
    {
        foreach (var btn in displayButtons)
            btn.interactable = true;

        selected.interactable = false;
        Debug.Log("Selected Display Mode: " + selected.name);
        // TODO: Apply display mode setting to the game window

    }

    public void SetMouseSensitivity(float value)
    {
        // TODO: Apply mouse sensitivity setting to the game
        mouseValueText.text = value.ToString("F2");

    }


    public void SetMasterVolume(float value)
    {
        // audioMixer.SetFloat("MasterVol", Mathf.Lerp(-80, 0, value));
        masterValueText.text = Mathf.RoundToInt(value).ToString();
    }

    public void SetMusicVolume(float value)
    {
        // audioMixer.SetFloat("MusicVol", Mathf.Lerp(-80, 0, value));
        musicValueText.text = Mathf.RoundToInt(value).ToString();
    }

    public void SetVFXVolume(float value)
    {
        // audioMixer.SetFloat("VFXVol", Mathf.Lerp(-80, 0, value));
        vfxValueText.text = Mathf.RoundToInt(value).ToString();
    }

}
