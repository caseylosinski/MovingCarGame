using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Hook your music AudioSource here")]
    public AudioSource musicSource;

    [Header("Optional: hook the slider to sync on start")]
    public Slider slider;

const string PrefKey = "MusicVolume";

    void Awake()
    {
        // Load saved volume or default to 0.5
        float v = PlayerPrefs.GetFloat(PrefKey, 0.5f);
        if (musicSource) musicSource.volume = v;
        if (slider) slider.value = v;
    }

    public void SetMusicVolume(float v)
    {
        if (musicSource) musicSource.volume = v;
        PlayerPrefs.SetFloat(PrefKey, v);  // remember across sessions
    }
}
