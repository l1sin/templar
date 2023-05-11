using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderAudio : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioMixer _audioMixer;

    private void Awake()
    {
        _audioMixer.GetFloat(_audioMixerGroup.name, out float value);
        _slider.value = value;
        _slider.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }

    private void ChangeVolume()
    {
        AudioManager.Instance.SetVolume(_audioMixerGroup.name, _slider.value);
    }
}
