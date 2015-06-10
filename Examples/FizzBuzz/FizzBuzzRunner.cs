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
            //Setup the predicate map to use when writing the fizz buzz sequence
            fizzBuzzSequenceMap = SetupSequenceMap();
        }

        public List<string> Run(int numberToCountTo)
        {
            var result = new List<string>();
            for (var x = 0; x <= numberToCountTo; x++ )
            {
                //Check the setup of the map and you can see the conditions for numbers matching fizz and buzz are separate 
                //and there is no predicate to match both. In cases where both predicates match, GetAllMatches finds both and
                //the string.Join concatenate the two matches.
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

        //In this method the map is set up and here you can see the predicates which match the incoming interger values
        // and the sting messages which go with the predicate conditions.
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