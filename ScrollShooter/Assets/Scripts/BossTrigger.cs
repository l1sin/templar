using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _boss;
    [SerializeField] private Transform _spawnPositon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            AudioManager.Instance.StartBossMusic();
            Instantiate(_boss, _spawnPositon.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
