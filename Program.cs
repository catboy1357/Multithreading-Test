using System.Diagnostics;

namespace MyApp
{
    public class Program
    {
        public static void Main()
        {
            Stopwatch timer = new Stopwatch();

            // Gets the amount of task to run

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter number of tasks: ");

            int tasks = ifIsNumber(Console.ReadLine());
            bool[] jobs = new bool[tasks];


            // Starts all the tasks
            timer.Start();
            for (int i = 0; i < jobs.Length; i++)
            {
                ThreadPool.QueueUserWorkItem((object state) =>
                {
                    Tasks(state, jobs);
                }, i);
            }


            //Job for the main thread todo
            // MainThreadJob(1000);


            // progresses of all the tasks
            while (true)
            {
                bool allDone = true;
                for (int i = 0; i < jobs.Length; i++)
                {
                    if (!jobs[i])
                    {
                        allDone = false;
                        break;
                    }
                }
                // presentages of the jobs done
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($" \rProgress: {jobs.Count(x => x)}/{jobs.Length} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" {(jobs.Length - jobs.Count(x => !x)) * 100 / jobs.Length}% ");

                if (allDone)
                {
                    break;
                }
            }


            // check if tasks are still running before exiting
            while (!jobs.All(x => x))
                Thread.Sleep(100);


            // Prints the time it took to complete the tasks
            timer.Stop();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" All tasks finished, total time: {timer.Elapsed}'s ");
        }


        private static void Tasks(object state, bool[] jobs)
        {
            // proses the task
            Stopwatch timer = new Stopwatch();
            Random rnd = new Random();
            int min = 1000, max = 5000, delay = rnd.Next(min, max);
            timer.Start();


            int job = (int)state;
            // Console.ForegroundColor = ConsoleColor.Blue;
            // Console.WriteLine("  Job {0} started on thread {1}", job, Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(delay);


            timer.Stop();
            jobs[job] = true;
            // Console.ForegroundColor = ConsoleColor.Green;
            // Console.WriteLine("  Job {0} finished on tread {1}, time: {2}", job, Thread.CurrentThread.ManagedThreadId, timer.Elapsed);
        }


        private static void MainThreadJob(int time)
        {
            // proses the task on main thread
            // Console.ForegroundColor = ConsoleColor.Magenta;
            // Console.WriteLine("Main thread started");

            Thread.Sleep(time);

            // Console.ForegroundColor = ConsoleColor.DarkMagenta;
            // Console.WriteLine("Main thread done.");
        }

        private static int ifIsNumber(string input)
        {
            //checks if input is a number using tryparse
            int number;
            if (int.TryParse(input.ToString(), out number))
            {
                if (number > 0)
                {
                    return number;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[Error]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Please enter a number greater than 0: ");
                    return ifIsNumber(Console.ReadLine());
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("[Error] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Invalid input, please enter a number: ");
                return ifIsNumber(Console.ReadLine());
            }
        }
    }
}
