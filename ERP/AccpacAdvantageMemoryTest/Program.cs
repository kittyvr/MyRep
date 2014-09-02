using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccpacAdvantageMemoryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Change in the development branch

            int noOfIterations = 0;
            int disposeObjects = 0;
            int runInParallel = 0;
            int continueTest = 0;

            Process currentProcess = Process.GetCurrentProcess();
            long initialMemory = currentProcess.WorkingSet64;

            Console.WriteLine("Current memory size {0}", initialMemory);

            do
            {
                Console.WriteLine("Continue Test? 0=Stop, 1=Continue");
                continueTest = int.Parse(Console.ReadLine());

                if (continueTest == 0)
                {
                    break;
                }

                Console.WriteLine("How many objects to create?");
                noOfIterations = int.Parse(Console.ReadLine());

                Console.WriteLine("Dispose objects(0/1)?: 0=false, 1=true?");
                disposeObjects = int.Parse(Console.ReadLine());

                Console.WriteLine("Run test sequentially(0/1)?: 0=no, 1=yes?");
                runInParallel = int.Parse(Console.ReadLine());

                if (runInParallel >= 1)
                {
                    Parallel.For(0, noOfIterations, i =>
                   {
                       Stopwatch timeTaken = new Stopwatch();
                       timeTaken.Start();
                       ExecuteCS0001Repository(i, disposeObjects);
                       timeTaken.Stop();
                       Console.WriteLine("Time taken for one pass {0} = {1}", i, timeTaken.ElapsedMilliseconds);
                   });
                }
                else
                {
                    for (int i = 0; i < noOfIterations; i++)
                    {
                        Stopwatch timeTaken = new Stopwatch();
                        timeTaken.Start();
                        ExecuteCS0001Repository(i, disposeObjects);
                        timeTaken.Stop();
                        Console.WriteLine("Time taken for one pass {0} = {1}", i, timeTaken.ElapsedMilliseconds);
                    }
                }

                GC.SuppressFinalize(true);
                GC.Collect();

                Console.WriteLine("Created {0} objects", noOfIterations);
                currentProcess = Process.GetCurrentProcess();
                long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;

                Console.WriteLine("Initial Memory = {0:N0} KB, Current memory size {1:N0} KB", initialMemory / 1024, totalBytesOfMemoryUsed / 1024);
            } while (noOfIterations > 0 && continueTest > 0);

            Console.WriteLine("Press any key to exit the application.");
            Console.ReadLine();
        }

        private static void ExecuteCS0001Repository(int iteration, int disposeObjects)
        {
            try
            {
                if (disposeObjects >= 1)
                {

                    using (CS0001AccpacRepository repository = new CS0001AccpacRepository(false))
                    {
                        repository.InitializeSession();
                        repository.OpenDbLink();
                        repository.ReadLinkData();
                        repository.ReadViewData();
                    }

                }
                else
                {
                    CS0001AccpacRepository repository = new CS0001AccpacRepository(false);
                    repository.InitializeSession();
                    repository.OpenDbLink();
                    repository.ReadLinkData();
                    repository.ReadViewData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Iteration {0} failed: {1}", iteration, ex.Message);

            }
        }
    }
}
