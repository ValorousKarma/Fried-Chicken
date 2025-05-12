using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer audioMixer; 
    public void SetVolume(float volume)
    {
        Debug.Log("Volume set to: " + volume);
        audioMixer.SetFloat("volume", volume);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
