using TMPro;
using UnityEngine;

public class DestroyOnReach : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _destroyTime;
    private float _destroyTimer;
    private bool _destroy = false;

    private void Awake()
    {
        _destroy = false;
        _destroyTimer =  _destroyTime;
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        if (_destroy)
        {
            _destroyTimer -= Time.deltaTime;
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _destroyTimer / _destroyTime);
            if (_destroyTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) _destroy = true;
    }
}
