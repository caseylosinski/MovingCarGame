using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    [Header("Audio")]
    public AudioSource musicSource; // assign in Inspector

    [Header("Optional UI")]
    public Slider volumeSlider;
    public Toggle muteToggle;

    const string VolumeKey = "MusicVolume";
    const string MuteKey   = "MusicMuted";

    void Awake()
    {
        // Singleton: keep the first one, destroy any future duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // make sure we have the source
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }

        // safety check: do we actually have an AudioSource?
        if (musicSource == null)
        {
            Debug.LogError("[MusicPlayer] No AudioSource found on MusicPlayer.");
            return;
        }

        // load saved settings
        float savedVol = PlayerPrefs.GetFloat(VolumeKey, 0.5f);
        bool savedMute = PlayerPrefs.GetInt(MuteKey, 0) == 1;

        musicSource.volume = savedVol;
        musicSource.mute = savedMute;

        // start playing if not already
        if (!musicSource.isPlaying && musicSource.clip != null)
        {
            musicSource.Play();
        }

        // listen for scene changes so we can keep audio alive/restart if needed
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void Start()
    {
        // hook up UI if you drag them in
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.wholeNumbers = false;
            volumeSlider.value = musicSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (muteToggle != null)
        {
            muteToggle.isOn = musicSource.mute;
            muteToggle.onValueChanged.AddListener(SetMute);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // if some engines pause audio on load, force it back on
        if (musicSource != null &&
            !musicSource.isPlaying &&
            musicSource.clip != null &&
            !musicSource.mute &&
            musicSource.volume > 0f)
        {
            musicSource.Play();
        }
    }

    public void SetVolume(float v)
    {
        if (musicSource == null) return;
        musicSource.volume = v;
        PlayerPrefs.SetFloat(VolumeKey, v);
        PlayerPrefs.Save();
    }

    public void SetMute(bool m)
    {
        if (musicSource == null) return;
        musicSource.mute = m;
        PlayerPrefs.SetInt(MuteKey, m ? 1 : 0);
        PlayerPrefs.Save();
    }
}
