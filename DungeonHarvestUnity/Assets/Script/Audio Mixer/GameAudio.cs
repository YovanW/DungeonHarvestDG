using UnityEngine;

public class GameAudio : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource masterSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip music;
    public AudioClip death;
    public AudioClip takeDamage;
    public AudioClip chop;
    public AudioClip mine;
    public AudioClip rockBreak;
    public AudioClip eat;

    void Start()
    {
        if (music == null) return;

        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
