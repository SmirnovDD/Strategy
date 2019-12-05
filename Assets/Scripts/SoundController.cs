using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SoundController : MonoBehaviour
{
    public AudioMixer mainAudioMixer;
    public Image soundsVolumeImage;
    public Sprite soundOffSprite, soundOnSprite;
    public Slider soundsVolume;

    private void Start()
    {
        soundsVolume.value = PlayerPrefs.GetFloat("soundsVolume", 1);
    }
    public void VolumeChange()
    {
        float soundsVolumeLerped = Mathf.Lerp(-80, 0, soundsVolume.value);

        if (soundsVolume.value == 0)
            soundsVolumeImage.sprite = soundOffSprite;
        else
            soundsVolumeImage.sprite = soundOnSprite;

        mainAudioMixer.SetFloat("Volume", soundsVolumeLerped);

        PlayerPrefs.SetFloat("soundsVolume", soundsVolume.value);
    }
}
