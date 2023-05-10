using UnityEngine;
using UnityEngine.Audio;

public class BaseCollectible : MonoBehaviour
{
    private float _speed = 0.25f;
    private float _altitude = 0.1f;
    private Vector2[] _points;
    private bool _goUp;
    [SerializeField] private AudioClip[] _sounds;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;

    private void Awake()
    {
        _goUp = true;
        _points = new Vector2[2];
        _points[0] = (Vector2)transform.position - Vector2.up * _altitude;
        _points[1] = (Vector2)transform.position + Vector2.up * _altitude;
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        MoveUpAndDown();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Collect(collision);
        }
    }

    protected virtual void Collect(Collider2D collision)
    {
        AudioManager.Instance.MakeSound(transform, _sounds, _audioMixerGroup, false, false);
        Destroy(gameObject);
    }

    private void MoveUpAndDown()
    {
        if (_goUp)
        {
            transform.position = Vector2.MoveTowards(transform.position, _points[1], _speed * Time.deltaTime);
            if ((Vector2)transform.position == _points[1])
            {
                _goUp = !_goUp;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _points[0], _speed * Time.deltaTime);
            if ((Vector2)transform.position == _points[0])
            {
                _goUp = !_goUp;
            }
        }
    }
}
