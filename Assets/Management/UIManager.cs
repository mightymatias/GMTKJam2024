using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI totalWorkersText;
    public TextMeshProUGUI activeWorkersText;
    private int initialTotalWorkers; // This is the new field
    private int activeWorkers;

    public void SetInitialTotalWorkers(int workers)
    {
        initialTotalWorkers = workers;
        totalWorkersText.text = $"{initialTotalWorkers}";
    }

    public void SetActiveWorkers(int workers)
    {
        activeWorkers = workers;
        activeWorkersText.text = $"{activeWorkers}";
    }

    public int GetActiveWorkers()
    {
        return activeWorkers;
    }
}
