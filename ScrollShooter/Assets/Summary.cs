using TMPro;
using UnityEngine;

public class Summary : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeResult;
    [SerializeField] private TextMeshProUGUI _moneyResult;

    private void Awake()
    {
        ResultTime();
        _moneyResult.text = Stats.Instance.Money.ToString();
    }

    private void ResultTime()
    {
        int totalSeconds = (int)Stats.Instance.TimeInSeconds;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        string timeResult;
        if (seconds < 10) timeResult = $"{minutes}:0{seconds}";
        else timeResult = $"{minutes}:{seconds}";

        _timeResult.text = timeResult;
    }
}
