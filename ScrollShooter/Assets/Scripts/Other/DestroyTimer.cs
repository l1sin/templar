using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] private float _timer;

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        _timer -= Time.deltaTime;
        if (_timer <= 0) Destroy(gameObject);
    }
}
