using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] public AudioSource ASource;
    [SerializeField] public bool DontDestroy;

    private void Awake()
    {
        if (DontDestroy) DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (ASource.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}