﻿using JobStateManagerLib;

namespace JobExecutor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            JobStateManager jobStateManager = new JobStateManager();
            List<JobInput> jobInputs = new List<JobInput>
            {
                    new JobInput { Job = 2, DependsOn = 1 },
                    new JobInput { Job = 3, DependsOn = 1 },
                    new JobInput { Job = 4, DependsOn = 2 },
                    new JobInput { Job = 4, DependsOn = 3 },
                    new JobInput { Job = 8, DependsOn = 4 },
                    new JobInput { Job = 9, DependsOn = 8 },
                    new JobInput { Job = 10, DependsOn = 8 },
                    new JobInput { Job = 7, DependsOn = 5 },
                    new JobInput { Job = 7, DependsOn = 6 }
            };

          

            // Example #1
            jobStateManager.Init(jobInputs);
            Console.WriteLine(string.Join(",", jobStateManager.GetNextAvailableJobs()));

            // Example #2
            jobStateManager.Init(jobInputs);
            jobStateManager.SetJobSuccessful(1);
            Console.WriteLine(string.Join(",", jobStateManager.GetNextAvailableJobs()));

            // Example #3
            jobStateManager.Init(jobInputs);
            jobStateManager.SetJobFail(1);
            Console.WriteLine(string.Join(",", jobStateManager.GetNextAvailableJobs()));

          

        }
    }
}