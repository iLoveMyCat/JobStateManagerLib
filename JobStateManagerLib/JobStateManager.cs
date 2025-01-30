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
        private readonly List<int> _nextJobsToExecute = new List<int>();
        public void Init(List<JobInput> jobInputs)
        {
            foreach(JobInput jobInput in jobInputs)
            {
                if (!_currentJobs.ContainsKey(jobInput.Job))
                {
                    _currentJobs.Add(jobInput.Job, new List<int> { jobInput.DependsOn });
                }
                else
                {
                    _currentJobs[jobInput.Job].Add(jobInput.DependsOn);
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
        public void SetJobFail(int jobId)
        {
            throw new NotImplementedException();
        }
        public void SetJobSuccessful(int jobId)
        {
            throw new NotImplementedException();
        }
    }
}
