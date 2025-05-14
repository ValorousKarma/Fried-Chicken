using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music Tracks")]
    public AudioClip[] backgroundMusic;
    private AudioClip backgroundSong;
    public AudioClip winMusic;
    public AudioClip deathMusic;

    [Header("UI Sounds")]
    public AudioClip pauseSound;      // Single "pause" sound effect
    public AudioClip unpauseSound;    // Single "unpause" sound effect

    private bool wasMusicPlaying;
    
    [Header("Player Sounds")]
    public AudioClip jumpSound;
    public AudioClip playerAttackSound;

    [Header("Enemy Sounds")]
    public AudioClip ratAttackSound; // Separate sound for Rat NPC
    public AudioClip chickenAttackSound; // Separate sound for Chicken NPC
   
    
    private AudioSource musicSource;
    private AudioSource sfxSource;

    private Dictionary<string, int> musicKeys = new Dictionary<string, int>();

    
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

        // set scene names to be associated with background music array indexes
        musicKeys.Add("StartScene", 0);
        musicKeys.Add("Level_One_Forrest", 0);
        musicKeys.Add("Level_Two_Town", 1);
        musicKeys.Add("Level_Three_Cave", 2);

        // default song to play if UpdateBackgroundMusic not called
        backgroundSong = backgroundMusic[0];
    }
    void Start()
    {
        PlayBackgroundMusic();
    }

    // === Music Controls ===
    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundSong;
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

    public void OnGamePaused()
    {
    // Pause background music (but remember its state)
    wasMusicPlaying = musicSource.isPlaying;
    musicSource.Pause();
    
    // Play single UI pause sound (non-looping)
    sfxSource.PlayOneShot(pauseSound);
    }

// Call this when unpausing
    public void OnGameResumed()
    {
        // Play UI unpause sound
        sfxSource.PlayOneShot(unpauseSound);
        
        // Restore music if it was playing
        if (wasMusicPlaying)
        {
            musicSource.UnPause();
        }
    }

    public void UpdateBackgroundMusic(string sceneName)
    {
        backgroundSong = backgroundMusic[musicKeys[sceneName]];
        PlayBackgroundMusic();
    }

    // === Player Sounds ===
    public void PlayJumpSound() => sfxSource.PlayOneShot(jumpSound);
    public void PlayPlayerAttackSound() => sfxSource.PlayOneShot(playerAttackSound);
    

    // === Enemy Sounds ===
    public void PlayRatAttackSound() => sfxSource.PlayOneShot(ratAttackSound);

    public void PlayChickenAttackSound() => sfxSource.PlayOneShot(chickenAttackSound);
}
