using Examples.FizzBuzz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    class Program
    {
        private const string FizzBuzzKey = "FizzBuzz";

        public static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "?")
            {
                PrintUsage();
            }
            PerformRequiredOperation(args);
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Available calls:");
            Console.WriteLine(String.Join(" ", "FizzBuzz to number:", FizzBuzzKey, "{number to count to}"));
        }

        private static void PerformRequiredOperation(string[] args)
        {
            switch (args[0])
            {
                case FizzBuzzKey:
                    RunFizzBuzz(Int32.Parse(args[1]));
                    break;
                default:
                    PrintUsage();
                    break;
            }
        }

        private static void RunFizzBuzz(int topLimit)
        {
            var runner = new FizzBuzzRunner();
            var results = runner.Run(topLimit);
            //results.ForEach(r => Console.WriteLine(r));
            foreach(var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}
