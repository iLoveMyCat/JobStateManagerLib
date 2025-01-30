using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobStateManagerLib
{
    public class JobStateManager : IJobDependencyManager
    {
        private readonly Dictionary<int, List<int>> _currentJobs = new Dictionary<int, List<int>>();
        private readonly Dictionary<int, List<int>> _jobDependants = new Dictionary<int, List<int>>();
        private readonly HashSet<int> _nextJobsToExecute = new HashSet<int>();
        private readonly Dictionary<int, bool?> _jobState = new Dictionary<int, bool?>();
        public void Init(List<JobInput> jobInputs)
        {
            _currentJobs.Clear();
            _nextJobsToExecute.Clear();
            _jobDependants.Clear();
            _jobState.Clear();

            foreach (JobInput jobInput in jobInputs)
            {
                if (!_currentJobs.ContainsKey(jobInput.Job))
                {
                    _currentJobs.Add(jobInput.Job, new List<int> { jobInput.DependsOn });
                }
                else
                {
                    _currentJobs[jobInput.Job].Add(jobInput.DependsOn);
                }

                if (!_jobDependants.ContainsKey(jobInput.DependsOn))
                {
                    _jobDependants.Add(jobInput.DependsOn, new List<int> { jobInput.Job });
                }
                else
                {
                    _jobDependants[jobInput.DependsOn].Add(jobInput.Job);
                }

                if (!_currentJobs.ContainsKey(jobInput.DependsOn))
                {
                    _nextJobsToExecute.Add(jobInput.DependsOn);
                }
        
            }
        }
        public int[] GetNextAvailableJobs()
        {
            return _nextJobsToExecute.ToArray();
        }
        public void SetJobSuccessful(int jobId)
        {
            if (!_jobState.ContainsKey(jobId))
            {
                _jobState.Add(jobId, true);

                if (_nextJobsToExecute.Contains(jobId))
                {
                    if (_jobDependants.ContainsKey(jobId))
                    {
                        foreach(int job in _jobDependants[jobId])
                        {
                            _nextJobsToExecute.Add(job);
                        }
                    }
                }
            }
            else
            {
                _jobState[jobId] = true;
            }

            _nextJobsToExecute.Remove(jobId);
        }
        public void SetJobFail(int jobId)
        {
            if (!_jobState.ContainsKey(jobId))
            {
                _jobState.Add(jobId, false);
            }
            else
            {
                _jobState[jobId] = false;
            }

            _nextJobsToExecute.Remove(jobId);
        }
    }
}
