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
        private const string FIZZ_BUZZ_KEY = "FizzBuzz";
        private const string EXCEPTION_FILTER_KEY = "ExceptionFilter";
        private static IDictionary<string, Action<string[]>> operations = new Dictionary<string, Action<string[]>>
        {
            { FIZZ_BUZZ_KEY, RunFizzBuzz  },
            { EXCEPTION_FILTER_KEY, RunExceptionFiltering }
        };

        public static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "?")
            {
                PrintUsage();
            }
            else
            {
                PerformRequiredOperation(args);
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Available calls:");
            Console.WriteLine(String.Join(" ", "FizzBuzz to number:", FIZZ_BUZZ_KEY, "{number to count to}"));
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(String.Join(" ", "Throw exception to demonstrate catching:", EXCEPTION_FILTER_KEY, "{Name of excpetion type to throw}", "{message to include in exception}"));
            Console.WriteLine(Environment.NewLine);
            foreach (var line in ExceptionFilter.GetHandledTypesDescriptions())
            {
                Console.WriteLine(line);
            }
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
            var thrower = new ExceptionThrower();
            try 
            { 
                thrower.ThrowExceptionForFilter(args[1], args[2]);
            }
            catch (Exception e)
            {
                Console.WriteLine(filter.RespondToThrownException(e));
            }
        }
    }
}