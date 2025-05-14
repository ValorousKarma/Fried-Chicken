using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music Tracks")]
    public AudioClip backgroundMusic;
    public AudioClip winMusic;
    public AudioClip deathMusic;

    public AudioClip pauseMusic;
    
    [Header("Player Sounds")]
    public AudioClip jumpSound;
    public AudioClip playerAttackSound;

    [Header("Enemy Sounds")]
    public AudioClip ratAttackSound; // Separate sound for Rat NPC
    
    private AudioSource musicSource;
    private AudioSource sfxSource;

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
            
            // Configure music source
            musicSource.loop = true;
            musicSource.spatialBlend = 0; // 2D sound
            musicSource.volume = 0.7f; // Adjust as needed
            
            // Configure SFX source
            sfxSource.spatialBlend = 0;
            sfxSource.volume = 1.0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PlayBackgroundMusic();
    }

    // === Music Controls ===
    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlayWinMusic()
    {
        musicSource.clip = winMusic;
        musicSource.Play();
    }

    public void PlayDeathMusic()
    {
        musicSource.clip = deathMusic;
        musicSource.Play();
    }

    public void PlayPauseMusic()
    {
        musicSource.clip = pauseMusic;
        musicSource.Play();
    }

    // === Player Sounds ===
    public void PlayJumpSound() => sfxSource.PlayOneShot(jumpSound);
    public void PlayPlayerAttackSound() => sfxSource.PlayOneShot(playerAttackSound);
    

    // === Enemy Sounds ===
    public void PlayRatAttackSound() => sfxSource.PlayOneShot(ratAttackSound);
}
