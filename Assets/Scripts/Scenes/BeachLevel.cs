using UnityEngine;

public class BeachLevel : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip ambience;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        if (fadeIn)
        {
            if (music != null)    AudioManager.Instance.FadeInMusic(music, fadeDuration);
            if (ambience != null) AudioManager.Instance.FadeInAmbience(ambience, fadeDuration);
        }
        else
        {
            if (music != null)    AudioManager.Instance.PlayMusic(music);
            if (ambience != null) AudioManager.Instance.PlayAmbience(ambience);
        }
    }
}
