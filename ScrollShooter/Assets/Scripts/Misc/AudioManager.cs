using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer _audioMixer;

    private void Awake()
    {
        SetSingleton();
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

    public void MakeSound(Vector3 position, AudioClip audioClip, AudioMixerGroup audioMixerGroup, bool dontDestroyOnLoad = true, string name = "Sound")
    {
        GameObject newSound = new GameObject(name);
        newSound.transform.position = position;
        AudioSource audioSource =  newSound.AddComponent<AudioSource>();
        AudioPlayer audioPlayer = newSound.AddComponent<AudioPlayer>();

        audioPlayer.ASource = audioSource;
        audioPlayer.DontDestroy = dontDestroyOnLoad;

        audioSource.clip = audioClip;
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        audioSource.Play();
    }
}