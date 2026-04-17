using UnityEngine;
using UnityEngine.Audio; 

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixerMusic; 
    public void SetVolumeMusic (float volume)
    {
        audioMixerMusic.SetFloat("volumeMusic", volume); 
    }

    public void SetVolumeSFX (float volume)
    {
        audioMixerMusic.SetFloat("volumeSFX", volume); 
    }

    public void SetVolumeGlobal (float volume)
    {
        audioMixerMusic.SetFloat("volumeGlobal", volume); 
    }
}
