using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    public int totalNPCWorkers = 10;
    public DynamicTimerController dynamicTimerController;
    public UIManager uiManager;
    public CrumbSpawning crumbSpawner;

    private int availableWorkers;
    private Dictionary<string, Job> jobs = new Dictionary<string, Job>();
    private HashSet<string> activeConnections = new HashSet<string>();

    private class Job
    {
        public int workersAssigned;
        public float timePerWorker;
        public Timer timer;
        public bool isProcessed;
        public string jobName;
    }

    void Start()
    {
        availableWorkers = totalNPCWorkers;
        uiManager.SetInitialTotalWorkers(totalNPCWorkers);
        uiManager.SetActiveWorkers(0);
    }

    public void AssignWorkersToJob(
        string jobName,
        int numberOfWorkers,
        float timePerWorker,
        Sprite jobSymbol
    )
    {
        if (numberOfWorkers > availableWorkers || activeConnections.Contains(jobName))
        {
            Debug.LogError(
                $"Cannot assign workers to {jobName}. Not enough workers or job is already ongoing."
            );
            return;
        }

        availableWorkers -= numberOfWorkers;

        // Make sure the UI is updated AFTER modifying availableWorkers
        uiManager.SetActiveWorkers(uiManager.GetActiveWorkers() + numberOfWorkers);

        float totalTime = timePerWorker / numberOfWorkers;
        Timer jobTimer = dynamicTimerController.CreateAndConfigureTimer(
            totalTime,
            jobSymbol,
            () => OnJobCompleted(jobName)
        );

        Job newJob = new Job
        {
            workersAssigned = numberOfWorkers,
            timePerWorker = timePerWorker,
            timer = jobTimer,
            isProcessed = false,
            jobName = jobName,
        };

        jobs[jobName] = newJob;
        activeConnections.Add(jobName);

        Debug.Log(
            $"Job '{jobName}' started with {numberOfWorkers} workers for {totalTime} seconds."
        );
    }

    private void OnJobCompleted(string jobName)
    {
        if (jobs.TryGetValue(jobName, out Job job) && !job.isProcessed)
        {
            job.isProcessed = true;

            // Add workers back to the pool
            availableWorkers += job.workersAssigned;

            // Make sure the UI is updated AFTER modifying availableWorkers
            uiManager.SetActiveWorkers(uiManager.GetActiveWorkers() - job.workersAssigned);
            uiManager.UpdateActiveWorkersBasedOnInk(totalNPCWorkers, availableWorkers);

            Debug.Log(
                $"[JobManager] Workers available after completing {jobName}: {availableWorkers}"
            );
            Debug.Log(
                $"[JobManager] Workers in UI after completing {jobName}: {uiManager.GetActiveWorkers()}"
            );

            crumbSpawner?.SpawnCrumb(jobName);

            if (IsLineStillActiveForJob(jobName))
            {
                Debug.Log(
                    $"[JobManager] Job '{jobName}' is restarting because the line is still active."
                );

                // Clear the job state
                jobs.Remove(jobName);
                activeConnections.Remove(jobName);

                // Reassign the job
                if (availableWorkers >= job.workersAssigned)
                {
                    Debug.Log(
                        $"[JobManager] Reassigning job '{jobName}' with {job.workersAssigned} workers."
                    );
                    AssignWorkersToJob(
                        job.jobName,
                        job.workersAssigned,
                        job.timePerWorker,
                        job.timer.GetJobSymbol()
                    );

                    // Add job back to active connections
                    activeConnections.Add(jobName);
                }
                else
                {
                    Debug.LogError(
                        $"[JobManager] Cannot reassign workers to '{jobName}'. Not enough workers."
                    );
                }
            }
            else
            {
                // Clean up job if the line is no longer active
                Debug.Log(
                    $"[JobManager] Job '{jobName}' completed and will not restart as the line is not active."
                );
                jobs.Remove(jobName);
                activeConnections.Remove(jobName);
            }
        }
        else
        {
            Debug.LogWarning(
                $"[JobManager] Job '{jobName}' has already been processed or does not exist."
            );
        }
    }

    private bool IsLineStillActiveForJob(string jobName)
    {
        return activeConnections.Contains(jobName);
    }

    public void RemoveConnection(string jobName)
    {
        if (activeConnections.Contains(jobName))
        {
            activeConnections.Remove(jobName);
            Debug.Log($"Connection to {jobName} removed.");
        }
    }

    public bool TryAssignWorkersToJob(
        string jobName,
        int numberOfWorkers,
        float timePerWorker,
        Sprite jobSymbol,
        string lineId
    )
    {
        if (numberOfWorkers > availableWorkers || activeConnections.Contains(jobName))
        {
            Debug.LogError(
                $"Cannot assign workers to {jobName}. Not enough workers or job is already ongoing."
            );
            return false;
        }

        AssignWorkersToJob(jobName, numberOfWorkers, timePerWorker, jobSymbol);
        return true;
    }

    public void FlagJobForStopAfterCompletion(string jobName)
    {
        if (jobs.ContainsKey(jobName))
        {
            // Set a flag or take action to stop the job after it completes
            Debug.Log($"{jobName} will be stopped after completion.");
        }
        else
        {
            Debug.LogWarning($"Job '{jobName}' does not exist or is not currently active.");
        }
    }
}
