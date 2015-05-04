using PredicateMaps.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.FizzBuzz
{
    public class FizzBuzzRunner
    {
        private readonly IPredicateMap<int, string> fizzBuzzSequenceMap;

        public FizzBuzzRunner()
        {
            fizzBuzzSequenceMap = SetupSequenceMap();
        }

        public List<string> Run(int numberToCountTo)
        {
            var result = new List<string>();
            for (var x = 0; x <= numberToCountTo; x++ )
            {
                var matches = fizzBuzzSequenceMap.GetAllMatches(x);
                if (matches.Any()) 
                {
                    result.Add(string.Join(" ", matches));
                }
                else
                {
                    result.Add(x.ToString());
                }
                
            }
            return result;
        }

        private IPredicateMap<int, string> SetupSequenceMap()
        {
            var functionMap = new Dictionary<Predicate<int>, string>
            {
                { i => i % 3 == 0, "fizz" },
                { i => i % 5 == 0, "buzz" }
            };
            return new PredicateMap<int, string>(functionMap, string.Empty);
        }
    }
}