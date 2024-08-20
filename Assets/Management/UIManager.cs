using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI totalWorkersText;
    public TextMeshProUGUI activeWorkersText;

    private int initialTotalWorkers;
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

    public void UpdateActiveWorkersBasedOnInk(float maxInk, float currentInk)
    {
        int activeWorkers = (int)(maxInk - currentInk); // Calculate active workers
        SetActiveWorkers(activeWorkers); // Update the active workers display
    }
}
