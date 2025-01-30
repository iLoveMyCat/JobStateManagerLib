using JobStateManagerLib;

namespace JobExecutor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            JobStateManager jobStateManager = new JobStateManager();

            //jobStateManager.GetNextAvailableJobs();
            //jobStateManager.SetJobFail(1);
            //jobStateManager.SetJobSuccessful(1);
            jobStateManager.Init(new List<JobInput>
            {
                    new JobInput { Job = 1, DependsOn = 2 }
            });
            jobStateManager.GetNextAvailableJobs();
            jobStateManager.SetJobFail(1);
            jobStateManager.SetJobSuccessful(2);
            jobStateManager.GetNextAvailableJobs();

        }
    }
}