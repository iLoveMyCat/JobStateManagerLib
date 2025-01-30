using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobStateManagerLib
{
    public interface IJobDependencyManager
    {
        void Init(List<JobInput> jobInputs);
        int[] GetNextAvailableJobs();
        void SetJobSuccessful(int jobId);
        void SetJobFail(int jobId);
    }
}
