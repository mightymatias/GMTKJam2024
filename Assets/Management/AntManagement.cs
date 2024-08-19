using System.Collections.Generic;
using UnityEngine;

public class AntManagement : MonoBehaviour
{
    public int totalNPCWorkers = 10;
    public DynamicTimerController dynamicTimerController;
    public UIManager uiManager; // Reference to UIManager

    private Dictionary<string, Job> jobs = new Dictionary<string, Job>();

    // Structure to hold the job details
    private class Job
    {
        public int workersAssigned;
        public float timePerWorker;
        public float timeRemaining;
        public Timer timer;
    }

    void Start()
    {
        // Initialize the UI with the total number of workers
        uiManager.SetTotalWorkers(totalNPCWorkers);
        uiManager.SetActiveWorkers(0); // No workers are active initially
    }

    public void AssignWorkersToJob(
        string jobName,
        int numberOfWorkers,
        float timePerWorker,
        Sprite symbol
    )
    {
        if (numberOfWorkers > totalNPCWorkers)
        {
            Debug.LogError("Not enough workers to assign to the job.");
            return;
        }

        totalNPCWorkers -= numberOfWorkers;
        uiManager.SetTotalWorkers(totalNPCWorkers); // Update the total workers in the UI
        uiManager.SetActiveWorkers(uiManager.GetActiveWorkers() + numberOfWorkers); // Update the active workers in the UI

        // Calculate total time based on number of workers
        float totalTime = timePerWorker / numberOfWorkers;

        // Create and configure the timer using CreateAndConfigureTimer from DynamicTimerController
        Timer jobTimer = dynamicTimerController.CreateAndConfigureTimer(totalTime, symbol);

        // Create and store the job in the dictionary
        Job newJob = new Job
        {
            workersAssigned = numberOfWorkers,
            timePerWorker = timePerWorker,
            timeRemaining = totalTime,
            timer = jobTimer,
        };
        jobs[jobName] = newJob;
    }

    void Update()
    {
        List<string> completedJobs = new List<string>();

        foreach (var jobEntry in jobs)
        {
            string jobName = jobEntry.Key;
            Job job = jobEntry.Value;

            // Check if the job timer is still running
            if (!job.timer.IsRunning)
            {
                completedJobs.Add(jobName);
                totalNPCWorkers += job.workersAssigned; // Return workers to the pool
                uiManager.SetTotalWorkers(totalNPCWorkers); // Update total workers in the UI
                uiManager.SetActiveWorkers(uiManager.GetActiveWorkers() - job.workersAssigned); // Update active workers in the UI
            }
        }

        // Remove completed jobs from the dictionary
        foreach (var jobName in completedJobs)
        {
            jobs.Remove(jobName);
            Debug.Log($"{jobName} is completed!");
        }
    }
}
