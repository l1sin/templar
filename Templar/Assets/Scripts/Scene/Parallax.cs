using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform _parallaxAnchor;
    [SerializeField] private GameObject[] _parallaxObjects;
    [SerializeField] private float[] _parallaxEffectivenessX;
    [SerializeField] private float[] _parallaxEffectivenessY;
    [SerializeField] private SpriteRenderer[] _renderers;
    [HideInInspector] private Vector3 _deltaXMovement;
    [HideInInspector] private Vector3 _lastAnchorPos;

    private void Start()
    {
        _lastAnchorPos = _parallaxAnchor.position;
        _deltaXMovement = Vector3.zero;
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        _deltaXMovement = Vector3.zero;
        _deltaXMovement = _parallaxAnchor.position - _lastAnchorPos;

        for (int i = 0; i <= _parallaxObjects.Length - 1; i++)
        {
            _parallaxObjects[i].transform.position += new Vector3(_deltaXMovement.x, 0f, 0f) * _parallaxEffectivenessX[i];
            _parallaxObjects[i].transform.position += new Vector3(0f, _deltaXMovement.y, 0f) * _parallaxEffectivenessY[i];
        }

        for (int i = 0; i <= _parallaxObjects.Length - 1; i++)
        {
            if (Mathf.Abs(_parallaxAnchor.position.x - _parallaxObjects[i].transform.position.x) > (_renderers[i].bounds.extents.x / 3 * 2))
            {
                _parallaxObjects[i].transform.position = new Vector3(_parallaxAnchor.position.x, _parallaxObjects[i].transform.position.y, _parallaxObjects[i].transform.position.z);
            }
            if (Mathf.Abs(_parallaxAnchor.position.y - _parallaxObjects[i].transform.position.y) > (_renderers[i].bounds.extents.y / 3 * 2))
            {
                _parallaxObjects[i].transform.position = new Vector3(_parallaxObjects[i].transform.position.x, _parallaxAnchor.position.y, _parallaxObjects[i].transform.position.z);
            }
        }

        _lastAnchorPos = _parallaxAnchor.position;
    }
}
