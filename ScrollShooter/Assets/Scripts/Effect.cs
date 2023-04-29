using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private float _destroyTime;
    public void SeparateParent()
    {
        transform.SetParent(null);
        Destroy(gameObject, _destroyTime);
    }
}
