using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{

    [SerializeField] private AudioClip _mainMenu;
    [SerializeField] private AudioClip _gameplay;
    [SerializeField] private AudioClip _bossBatte;
    [SerializeField] private AudioClip _win;
    [SerializeField] private AudioClip _loss;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _audioSource.outputAudioMixerGroup = _audioMixerGroup;
        if (scene.buildIndex == 0) _audioSource.clip = _mainMenu;
        else _audioSource.clip = _gameplay;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void StartBossMusic()
    {
        _audioSource.clip = _bossBatte;
        _audioSource.Play();
    }

    public void StartLossSound()
    {
        _audioSource.clip = _loss;
        _audioSource.loop = false;
        _audioSource.Play();
    }
    public void StartWinSound()
    {
        _audioSource.clip = _win;
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
