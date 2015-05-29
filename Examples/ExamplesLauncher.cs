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
        private static IDictionary<string, Action<string[]>> operations = new Dictionary<string, Action<string[]>>
        {
            { FizzBuzzKey, RunExceptionFiltering },
            { ExceptionFilter, RunFizzBuzz }
        };

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
            if (operations.ContainsKey(args[0]))
            {
                operations[args[0]].Invoke(args);
            }
            else
            {
                PrintUsage();
            }
        }

        private static void RunFizzBuzz(string[] args)
        {
            var runner = new FizzBuzzRunner();
            var results = runner.Run(Int32.Parse(args[1]));
            results.ForEach(r => Console.WriteLine(r));
        }

        private static void RunExceptionFiltering(string[] args)
        {
            var filter = new ExceptionFilter();
            var thrower = new ExceptionThrower(filter);
            thrower.ThrowExceptionForFilter(args[1], args[2]);
        }
    }
}
