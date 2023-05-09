using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private float _destroyTime;
    [HideInInspector] private bool _destroy;

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        if (_destroy)
        {
            _destroyTime -= Time.deltaTime;
            if (_destroyTime <= 0) Destroy(gameObject);
        } 
    }
    public void SeparateParent()
    {
        transform.SetParent(null);
        _destroy = true;
    }
}
