using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
    [SerializeField] private GameObject[] _minions;
    [SerializeField] private GameObject[] _drops;

    public void Spawn()
    {
        GameObject minion = Instantiate(_minions[Random.Range(0, _minions.Length)], transform.position, Quaternion.identity);
        Spike spike = minion.GetComponent<Spike>();
        spike.Drop = _drops[Random.Range(0, _drops.Length)];
        spike.Spotted = true;
    }
}
