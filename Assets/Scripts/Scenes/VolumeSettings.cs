using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider ambienceSlider;
    [SerializeField] Slider sfxSlider;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        ambienceSlider.value = PlayerPrefs.GetFloat("AmbienceVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        ambienceSlider.onValueChanged.AddListener(AudioManager.Instance.SetAmbienceVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }
}
