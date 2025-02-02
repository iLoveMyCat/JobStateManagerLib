using JobStateManagerLib;

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





            Console.WriteLine("Expected: Error or detection of cycle");
            Console.WriteLine(string.Join(",", jobStateManager.GetNextAvailableJobs()));
            // Example #1
            Console.WriteLine("---Example 1");
            jobStateManager.Init(jobInputs);
            Console.WriteLine(string.Join(",", jobStateManager.GetNextAvailableJobs()));

            // Example #2
            Console.WriteLine("---Example 2");
            jobStateManager.Init(jobInputs);
            jobStateManager.SetJobSuccessful(1);
            Console.WriteLine(string.Join(",", jobStateManager.GetNextAvailableJobs()));

            // Example #3
            Console.WriteLine("---Example 3");
            jobStateManager.Init(jobInputs);
            jobStateManager.SetJobFail(1);
            Console.WriteLine(string.Join(",", jobStateManager.GetNextAvailableJobs()));
            jobStateManager.Init(jobInputs);

            jobStateManager.SetJobFail(1);
            Console.WriteLine("Excpected: 5,6");
            Console.WriteLine("Actual:" + string.Join(",", jobStateManager.GetNextAvailableJobs()));

            jobStateManager.SetJobSuccessful(6);
            Console.WriteLine("Excpected: 5");
            Console.WriteLine("Actual:" + string.Join(",", jobStateManager.GetNextAvailableJobs()));

            jobStateManager.SetJobSuccessful(5);
            Console.WriteLine("Excpected: 7");
            Console.WriteLine("Actual:" + string.Join(",", jobStateManager.GetNextAvailableJobs()));

            jobStateManager.Init(jobInputs);

         


            jobStateManager.Init(jobInputs);

            Console.WriteLine("Expected: 1,5,6");
            Console.WriteLine("Actual: " + string.Join(",", jobStateManager.GetNextAvailableJobs()));

            jobStateManager.Init(jobInputs);
            jobStateManager.SetJobSuccessful(1);
            Console.WriteLine("Expected: 2,3,5,6");
            Console.WriteLine("Actual: " + string.Join(",", jobStateManager.GetNextAvailableJobs()));

            jobStateManager.Init(jobInputs);
            jobStateManager.SetJobSuccessful(1);
            jobStateManager.SetJobSuccessful(2);
            jobStateManager.SetJobFail(3); 
            Console.WriteLine("Expected: 5,6"); 
            Console.WriteLine("Actual: " + string.Join(",", jobStateManager.GetNextAvailableJobs()));

            jobStateManager.Init(jobInputs);
            jobStateManager.SetJobSuccessful(1);  
            jobStateManager.SetJobSuccessful(2);
            jobStateManager.SetJobSuccessful(3);
            jobStateManager.SetJobSuccessful(5); 
            Console.WriteLine("Expected: 4,6");  
            Console.WriteLine("Actual: " + string.Join(",", jobStateManager.GetNextAvailableJobs()));

            jobStateManager.SetJobSuccessful(4);
            jobStateManager.SetJobSuccessful(6);
            Console.WriteLine("Expected: 7,8"); 
            Console.WriteLine("Actual: " + string.Join(",", jobStateManager.GetNextAvailableJobs()));

            
            jobStateManager.Init(jobInputs);
            jobStateManager.SetJobSuccessful(1);
            jobStateManager.SetJobSuccessful(2);
            jobStateManager.SetJobSuccessful(3);
            jobStateManager.SetJobSuccessful(4);
            jobStateManager.SetJobSuccessful(5);
            jobStateManager.SetJobSuccessful(6);
            jobStateManager.SetJobSuccessful(7);
            jobStateManager.SetJobSuccessful(8);
            jobStateManager.SetJobSuccessful(9);
            jobStateManager.SetJobSuccessful(10);
            Console.WriteLine("Expectd: ");
            Console.WriteLine("Actual: " + string.Join(",", jobStateManager.GetNextAvailableJobs()));
        }
    }
}