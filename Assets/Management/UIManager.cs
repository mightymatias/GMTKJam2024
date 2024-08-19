using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI totalWorkersText;
    public TextMeshProUGUI activeWorkersText;
    private int totalWorkers;
    private int activeWorkers;

    public void SetTotalWorkers(int workers)
    {
        totalWorkers = workers;
        totalWorkersText.text = $"Total Workers: {totalWorkers}";
    }

    public void SetActiveWorkers(int workers)
    {
        activeWorkers = workers;
        activeWorkersText.text = $"Active Workers: {activeWorkers}";
    }

    public int GetActiveWorkers()
    {
        return activeWorkers;
    }
}
