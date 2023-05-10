using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] public AudioSource ASource;
    [SerializeField] public bool DontDestroy;
    [SerializeField] public bool follow;
    [SerializeField] public Transform followTarget;

    private void Awake()
    {
        if (DontDestroy) DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (PauseManager.IsPaused) return;
        if (ASource.isPlaying == false)
        {
            Destroy(gameObject);
        }
        if (follow)
        {
            if (followTarget != null) transform.position = followTarget.position;
        }
    }
}
