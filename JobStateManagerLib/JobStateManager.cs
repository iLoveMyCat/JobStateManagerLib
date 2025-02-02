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
    }
}
