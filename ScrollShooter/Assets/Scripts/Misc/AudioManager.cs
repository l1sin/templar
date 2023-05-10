using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private MusicPlayer _musicPlayer;

    private void Awake()
    {
        SetSingleton();
        SetSFXSpeed(1);
    }

    public void SetVolume(string mixerGroup, float newVolume)
    {
        _audioMixer.SetFloat(mixerGroup, newVolume);
    }

    private void SetSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public GameObject MakeSound(Transform transform, AudioClip[] audioClips, AudioMixerGroup audioMixerGroup, bool follow = true, bool spatialblend = true, bool dontDestroyOnLoad = true, string name = "Sound")
    {
        GameObject newSound = new GameObject(name);
        newSound.transform.position = transform.position;
        AudioSource audioSource =  newSound.AddComponent<AudioSource>();
        AudioPlayer audioPlayer = newSound.AddComponent<AudioPlayer>();

        audioPlayer.ASource = audioSource;
        audioPlayer.DontDestroy = dontDestroyOnLoad;

        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        if (spatialblend)
        {
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = 5;
            audioSource.maxDistance = 10;
        }
        if (follow)
        {
            audioPlayer.follow = follow;
            audioPlayer.followTarget = transform;
        }

        audioSource.Play();
        return newSound;
    }

    public void SetSFXSpeed(float speed)
    {
        _audioMixer.SetFloat(GlobalStrings.SFXSpeed, speed);
    }

    public void StartBossMusic()
    {
        _musicPlayer.StartBossMusic();
    }
    public void StartLossSound()
    {
        _musicPlayer.StartLossSound();
    }
    public void StartWinSound()
    {
        _musicPlayer.StartWinSound();
    }
}
