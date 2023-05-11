using UnityEngine;

public class WinTimer : MonoBehaviour
{
    [SerializeField] public float Timer;

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Player.Instance.FinishLevel();
            Destroy(gameObject);
        }
    }
}
