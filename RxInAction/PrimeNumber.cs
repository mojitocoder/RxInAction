using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
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

        public IEnumerable<int> GenerateYield(int amount)
        {
            int count = 0;
            int seed = 1;
            while (count < amount)
            {
                seed++;
                if (IsPrime(seed))
                {
                    count++;
                    yield return seed;
                }
            }
        }

        public async Task<IList<int>> GenerateAsync(int amount)
        {
            var primes = await Task.Run<IEnumerable<int>>(() =>
            {
                return Generate(amount);
            });

            return primes.ToList();
        }

        public IObservable<int> GeneratePrimeObservable_SameThread(int amount)
        {
            return Observable.Create<int>(observer =>
            {
                foreach (var prime in GenerateYield(amount))
                {
                    observer.OnNext(prime);
                }
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        public IObservable<int> GeneratePrimeObservable(int amount)
        {
            var cts = new CancellationTokenSource();
            return Observable.Create<int>(observer =>
            {
                //Task.Run();

                return new CancellationDisposable(cts);
            });
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
