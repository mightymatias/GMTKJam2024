using System.Collections.Generic;
using UnityEngine;

public class AntManagement : MonoBehaviour
{
    public int totalNPCWorkers = 10;
    public DynamicTimerController dynamicTimerController;
    public UIManager uiManager;
    public CrumbSpawning crumbSpawner; // Reference to CrumbSpawner

    private int availableWorkers;

    private Dictionary<string, Job> jobs = new Dictionary<string, Job>();
    private HashSet<string> ongoingJobs = new HashSet<string>(); // Track ongoing jobs

    private class Job
    {
        public int workersAssigned;
        public float timePerWorker;
        public Timer timer;
        public bool isProcessed; // Flag to ensure workers are only released once
        public string jobName; // Job name directly associated with the crumb type
    }

    void Start()
    {
        uiManager.SetInitialTotalWorkers(totalNPCWorkers);
        availableWorkers = totalNPCWorkers;
        uiManager.SetActiveWorkers(0);
    }

    public void AssignWorkersToJob(
        string jobName,
        int numberOfWorkers,
        float timePerWorker,
        Sprite jobSymbol
    )
    {
        if (numberOfWorkers > availableWorkers)
        {
            Debug.LogError("Not enough workers to assign to the job.");
            return;
        }

        availableWorkers -= numberOfWorkers;
        uiManager.SetActiveWorkers(uiManager.GetActiveWorkers() + numberOfWorkers);

        float totalTime = timePerWorker / numberOfWorkers;

        // Pass the OnJobCompleted method as a callback to the timer
        Timer jobTimer = dynamicTimerController.CreateAndConfigureTimer(
            totalTime,
            jobSymbol,
            () => OnJobCompleted(jobName) // Callback when timer completes
        );

        Job newJob = new Job
        {
            workersAssigned = numberOfWorkers,
            timePerWorker = timePerWorker,
            timer = jobTimer,
            isProcessed = false, // Initialize as unprocessed
            jobName =
                jobName // Store job name
            ,
        };

        jobs[jobName] = newJob;
        ongoingJobs.Add(jobName);

        Debug.Log(
            $"Job '{jobName}' started with {numberOfWorkers} workers for {totalTime} seconds."
        );
    }

    public void FlagJobForStopAfterCompletion(string jobName)
    {
        if (jobs.ContainsKey(jobName))
        {
            Job job = jobs[jobName];

            if (ongoingJobs.Contains(jobName))
            {
                ongoingJobs.Remove(jobName); // Prevent further looping
                Debug.Log($"{jobName} job flagged for stop and removed from ongoingJobs.");
            }

            // Ensure the job is correctly flagged to stop after completion
            if (!job.isProcessed)
            {
                job.isProcessed = true;
                Debug.Log($"{jobName} job is flagged to stop after the current iteration.");
            }
            else
            {
                Debug.LogWarning($"{jobName} job was already processed, flagging skipped.");
            }
        }
        else
        {
            Debug.LogWarning(
                $"FlagJobForStopAfterCompletion called with non-existent job: {jobName}"
            );
        }
    }

    private void OnJobCompleted(string jobName)
    {
        Debug.Log($"OnJobCompleted called for job: {jobName}");

        if (jobs.ContainsKey(jobName))
        {
            Job job = jobs[jobName];

            if (!job.isProcessed)
            {
                job.isProcessed = true; // Mark as processed to prevent reprocessing

                availableWorkers += job.workersAssigned;
                uiManager.SetActiveWorkers(uiManager.GetActiveWorkers() - job.workersAssigned);

                Debug.Log($"Job '{jobName}' completed. Checking if it was flagged for stop...");

                // Trigger crumb spawn
                crumbSpawner?.SpawnCrumb(jobName);

                // Ensure job cleanup occurs if flagged for stop
                if (!ongoingJobs.Contains(jobName))
                {
                    jobs.Remove(jobName);
                    Debug.Log($"{jobName} job has been completed and cleaned up.");

                    uiManager.UpdateActiveWorkersBasedOnInk(totalNPCWorkers, availableWorkers);
                }
                else
                {
                    Debug.Log($"Job '{jobName}' is looping again.");
                    AssignWorkersToJob(
                        jobName,
                        job.workersAssigned,
                        job.timePerWorker,
                        job.timer.GetJobSymbol()
                    );
                }
            }
            else
            {
                Debug.LogWarning($"Job '{jobName}' has already been processed.");
            }
        }
        else
        {
            Debug.LogError($"OnJobCompleted called with non-existent job: '{jobName}'");
        }
    }

    public void StopJob(string jobName)
    {
        if (ongoingJobs.Contains(jobName))
        {
            ongoingJobs.Remove(jobName);

            if (jobs.ContainsKey(jobName))
            {
                Job job = jobs[jobName];

                // Instead of releasing workers immediately, mark the job to stop after the current iteration
                job.isProcessed = true;
                Debug.Log($"{jobName} job is marked for stopping after completion.");

                // Ensure job is not removed from jobs dictionary prematurely
                // It will be removed in OnJobCompleted when it finishes
            }
        }
    }
}
