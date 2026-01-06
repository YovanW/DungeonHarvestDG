using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource masterSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip music;

    public AudioClip death;
    public AudioClip swing;
    public AudioClip takeDamage;

    public AudioClip chop;
    public AudioClip mine;
    public AudioClip rockBreak;

    public AudioClip eat;
    public AudioClip rake;
    public AudioClip plant;
    public AudioClip harvest;

    public AudioClip walk;
    public AudioClip run;
    public AudioClip dash;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        if (music == null) return;

        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        float originalPitch = SFXSource.pitch;

        SFXSource.pitch = Random.Range(0.90f, 1.10f);
        SFXSource.PlayOneShot(clip);
        SFXSource.pitch = originalPitch;
    }

    // Tambahin ini kalo mau play sfx
    // if (GameAudio.Instance != null)
    //     GameAudio.Instance.PlaySFX(GameAudio.Instance.namaSound);
}
