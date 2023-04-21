using System.Collections.Generic;
using UnityEngine;

public class RefreshPolygonCollider : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;

    private void Start()
    {
        _sprite = _spriteRenderer.sprite;
    }

    private void Update()
    {
        RefreshCollider();
    }
    private void RefreshCollider()
    {
        _collider.pathCount = 0;
        _collider.pathCount = _sprite.GetPhysicsShapeCount();

        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < _collider.pathCount; i++)
        {
            path.Clear();
            _sprite.GetPhysicsShape(i, path);
            _collider.SetPath(i, path.ToArray());
        }
    }
}
