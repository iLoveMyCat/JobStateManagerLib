namespace JobStateManagerLib
{
    public class JobStateManager : IJobDependencyManager
    {
        private readonly Dictionary<int, List<int>> _jobToDependencies = new Dictionary<int, List<int>>();
        private readonly Dictionary<int, List<int>> _jobToDependants = new Dictionary<int, List<int>>();
        private readonly HashSet<int> _nextJobsToExecute = new HashSet<int>();
        private readonly Dictionary<int, bool> _jobSuccess = new Dictionary<int, bool>();
        public void Init(List<JobInput> jobInputs)
        {
            ClearState();

            foreach (JobInput jobInput in jobInputs)
            {
                if (!_jobToDependencies.ContainsKey(jobInput.Job))
                {
                    _jobToDependencies.Add(jobInput.Job, new List<int> { jobInput.DependsOn });
                }
                else
                {
                    _jobToDependencies[jobInput.Job].Add(jobInput.DependsOn);
                }

                if (!_jobToDependants.ContainsKey(jobInput.DependsOn))
                {
                    _jobToDependants.Add(jobInput.DependsOn, new List<int> { jobInput.Job });
                }
                else
                {
                    _jobToDependants[jobInput.DependsOn].Add(jobInput.Job);
                }

                if (!_jobToDependencies.ContainsKey(jobInput.DependsOn))
                {
                    _nextJobsToExecute.Add(jobInput.DependsOn);
                }
            }

            if (IsCircular())
            {
                ClearState();
                Console.WriteLine("Failed To initialize, circular structure detected.");
                //throw new InvalidOperationException("Failed To initialize, circular structure detected.");
            }
        }
        public int[] GetNextAvailableJobs()
        {
            return _nextJobsToExecute.ToArray();
        }
        public void SetJobSuccessful(int successJob)
        {
            _jobSuccess[successJob] = true;
            UpdateNextJobs(successJob, true);
        }
        public void SetJobFail(int faildJob)
        {
            _jobSuccess[faildJob] = false;
            UpdateNextJobs(faildJob, false);

        }
        private void UpdateNextJobs(int completedJob, bool success)
        {
            // We assume, only available jobs are executed
            if (_nextJobsToExecute.Contains(completedJob))
            {
                _nextJobsToExecute.Remove(completedJob);

                if (success)
                {
                    if (_jobToDependants.ContainsKey(completedJob))
                    {
                        foreach (int dependantJob in _jobToDependants[completedJob])
                        {
                            if (!HasUnfullfilledDependency(dependantJob))
                            {
                                _nextJobsToExecute.Add(dependantJob);
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Operation, Job is not available");
                //throw new InvalidOperationException("Invalid Operation, Job is not available");
            }
        }
        private bool HasUnfullfilledDependency(int targetJob)
        {
            foreach (int dependencyJob in _jobToDependencies[targetJob])
            {
                //failed depnedency or unhandled yet
                if (!_jobSuccess.ContainsKey(dependencyJob) || !_jobSuccess[dependencyJob])
                {
                    return true;
                }
            }
            return false;
        }
        private void ClearState()
        {
            _jobToDependencies.Clear();
            _nextJobsToExecute.Clear();
            _jobToDependants.Clear();
            _jobSuccess.Clear();
        }
        private bool IsCircular()
        {
            HashSet<int> checkedJobs = new HashSet<int>();
            HashSet<int> jobsInProgress = new HashSet<int>();

            foreach (int job in _jobToDependencies.Keys)
            {
                if (CheckForLoop(job, checkedJobs, jobsInProgress))
                {
                    return true;  
                }
            }

            return false; 
        }
        private bool CheckForLoop(int currentJob, HashSet<int> checkedJobs, HashSet<int> jobsInProgress)
        {
            if (checkedJobs.Contains(currentJob) == false)
            {
                checkedJobs.Add(currentJob); 
                jobsInProgress.Add(currentJob); 

                if (_jobToDependencies.ContainsKey(currentJob))
                {
                    List<int> jobDependencies = _jobToDependencies[currentJob];

                    foreach (int dependency in jobDependencies) 
                    {
                        if (checkedJobs.Contains(dependency) == false)
                        {
                            if (CheckForLoop(dependency, checkedJobs, jobsInProgress))  
                            {
                                return true;  
                            }
                        }
                        else if (jobsInProgress.Contains(dependency))  
                        {
                            return true; 
                        }
                    }
                }
            }

            if (jobsInProgress.Contains(currentJob))  
            {
                jobsInProgress.Remove(currentJob);
            }

            return false;  
        }
    }
}
