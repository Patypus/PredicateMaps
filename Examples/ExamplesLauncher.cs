using Examples.ExceptionFiltering;
using Examples.FizzBuzz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    public class ExamplesLauncher
    {
        private const string FizzBuzzKey = "FizzBuzz";
        private const string ExceptionFilter = "ExceptionFilter";

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
            Console.WriteLine(String.Join(" ", "Throw exception to demonstrate catching", ExceptionFilter, "{Name of excpetion type to throw}", "{message to include in exception}"));
        }

        private static void PerformRequiredOperation(string[] args)
        {
            switch (args[0])
            {
                case FizzBuzzKey:
                    RunFizzBuzz(Int32.Parse(args[1]));
                    break;
                case ExceptionFilter:

                default:
                    PrintUsage();
                    break;
            }
        }

        private static void RunFizzBuzz(int topLimit)
        {
            var runner = new FizzBuzzRunner();
            var results = runner.Run(topLimit);
            results.ForEach(r => Console.WriteLine(r));
        }

        private static void RunExceptionFiltering(string exceptionType, string message)
        {
            var filter = new ExceptionFilter();
            Console.WriteLine(filter.ThrowAndCatchException(exceptionType, message));
        }
    }
}
