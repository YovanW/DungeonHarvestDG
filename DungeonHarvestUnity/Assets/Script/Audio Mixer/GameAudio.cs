using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource masterSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource footstepSource; // ONLY walk/run
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource deathSource;


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
    public AudioClip dash;
    public AudioClip jump;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        musicSource.ignoreListenerPause = true;
        deathSource.ignoreListenerPause = true;

        footstepSource.ignoreListenerPause = false;
        SFXSource.ignoreListenerPause = false;
    }

    void Start()
    {
        if (music == null) return;

        musicSource.clip = music;
        musicSource.Play();
    }

    void Update()
    {
        bool paused = Time.timeScale == 0;
        AudioListener.pause = paused;

        if (paused && footstepSource.isPlaying)
            footstepSource.Pause();

        if (!paused && footstepSource.clip != null && !footstepSource.isPlaying)
            footstepSource.UnPause();
    }
    public void StartWalk(bool isRunning)
    {
        if (walk == null) return;

        if (!footstepSource.isPlaying || footstepSource.clip != walk)
        {
            footstepSource.clip = walk;
            footstepSource.loop = true;
            footstepSource.Play();
        }

        footstepSource.pitch = isRunning ? 1.5f : 1f;
    }

    public void StopWalk()
    {
        if (!footstepSource.isPlaying) return;

        footstepSource.Stop();
        footstepSource.clip = null;
        footstepSource.loop = false;
        footstepSource.pitch = 1f;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        float originalPitch = SFXSource.pitch;

        SFXSource.pitch = Random.Range(0.90f, 1.10f);
        SFXSource.PlayOneShot(clip);
        SFXSource.pitch = originalPitch;
    }

    public void PlayDeathSFX(AudioClip clip)
    {
        if (clip == null) return;

        float originalPitch = deathSource.pitch;

        deathSource.pitch = Random.Range(0.90f, 1.10f);
        deathSource.PlayOneShot(clip);
        deathSource.pitch = originalPitch;
    }

    // Tambahin ini kalo mau play sfx
    // if (GameAudio.Instance != null)
    //     GameAudio.Instance.PlaySFX(GameAudio.Instance.namaSound);

    // Untuk wa
    // if (isMoving)
    // {
    //     if (GameAudio.Instance != null)
    //         GameAudio.Instance.StartWalk(isRunning);
    // }
    // else
    // {
    //     if (GameAudio.Instance != null)
    //         GameAudio.Instance.StopWalk();
    // }
}
