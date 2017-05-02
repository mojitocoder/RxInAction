using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxInAction
{
    public class PrimeNumber
    {
        public IEnumerable<int> Generate(int amount)
        {
            var primes = new List<int>();
            int seed = 2;
            while (primes.Count < amount)
            {
                if (IsPrime(seed)) primes.Add(seed);
                seed++;
            }
            return primes;
        }

        private bool IsPrime(int number)
        {
            if (number < 2) return false;
            else if (number == 2) return true;
            else
            {
                if (number % 2 == 0) return false;
                else
                {
                    int halfWay = number / 2;
                    for (int i = 2; i <= halfWay; i++)
                        if (number % i == 0) return false;

                    return true;
                }
            }
        }
    }
}
