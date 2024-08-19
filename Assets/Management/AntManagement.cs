using System.Collections.Generic;
using UnityEngine;

public class AntManagement : MonoBehaviour
{
    public int totalNPCWorkers = 10;
    public DynamicTimerController dynamicTimerController;
    public UIManager uiManager; // Reference to UIManager
    private int availableWorkers; // Field to track available workers

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
        // Set the initial total workers and the available workers
        uiManager.SetInitialTotalWorkers(totalNPCWorkers);
        availableWorkers = totalNPCWorkers;

        uiManager.SetActiveWorkers(0); // No workers are active initially
    }

    public void AssignWorkersToJob(
        string jobName,
        int numberOfWorkers,
        float timePerWorker,
        Sprite symbol
    )
    {
        if (numberOfWorkers > availableWorkers)
        {
            Debug.LogError("Not enough workers to assign to the job.");
            return;
        }

        availableWorkers -= numberOfWorkers;
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
        int workersToRelease = 0;
        List<string> completedJobs = new List<string>();

        // First pass: Identify completed jobs and calculate total workers to release
        foreach (var jobEntry in jobs)
        {
            string jobName = jobEntry.Key;
            Job job = jobEntry.Value;

            if (!job.timer.IsRunning)
            {
                completedJobs.Add(jobName);
                workersToRelease += job.workersAssigned; // Accumulate the workers to release
            }
        }

        // Second pass: Remove completed jobs and update the UI
        foreach (var jobName in completedJobs)
        {
            jobs.Remove(jobName);
            Debug.Log($"{jobName} is completed!");
        }

        // Update the available workers and the active workers in the UI
        availableWorkers += workersToRelease;
        uiManager.SetActiveWorkers(uiManager.GetActiveWorkers() - workersToRelease);

        // Ensure active workers count cannot go below zero
        if (uiManager.GetActiveWorkers() < 0)
        {
            uiManager.SetActiveWorkers(0);
        }
    }
}
