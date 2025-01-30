using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobStateManagerLib
{
    public class JobStateManager : IJobDependencyManager
    {
        private readonly Dictionary<int, List<int>> _currentJobs;
        public void Init(List<JobInput> jobInputs)
        {
            throw new NotImplementedException();
        }
        public int[] GetNextAvailableJobs()
        {
            throw new NotImplementedException();
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
