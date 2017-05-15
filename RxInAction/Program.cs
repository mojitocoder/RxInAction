using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace RxInAction
{
    class Program
    {
        public static void CreateObservers()
        {
            //Creating observer with Cancellation Token
            //var cts = new canc 

            //Creating observer on the pipe
            //  using lambda expressions
            var sub = Observable.Range(1, 10)
                .Select(foo => foo / (foo - 3))
                .Timestamp()
                .Subscribe(foo =>
                {
                    Console.WriteLine($"OnNext with: {foo}");
                },
                exception =>
                {
                    Console.WriteLine($"OnError with: {exception.Message};\n{exception.StackTrace}");
                },
                () =>
                {
                    Console.WriteLine($"OnCompleted!");
                });
        }

        static void Main(string[] args)
        {
            CreateObservers();

            Console.ReadKey();

            // **************************************
            // Create: Create an observable
            //  by writing into the observer
            // **************************************
            var countries = Observable.Create<string>(observer =>
            {
                var list = new List<string> { "Britain", "US", "France", "Germany" };
                foreach (var item in list)
                {
                    observer.OnNext(item);
                }

                observer.OnCompleted();
                return Disposable.Empty;
            });
            //countries.SubscribleConsole();

            // **************************************
            // Using: Read file into an observable
            // **************************************
            var lines = "rate.txt".ReadFileToObservable();
            //lines.SubscribleConsole("read file");

            // **************************************
            // Range: Create an observable of integers
            // **************************************
            var ints = Observable.Range(0, 100);
            //ints.SubscribleConsole();

            // **************************************
            // Return: Create an observable with only one item
            // **************************************
            var singleChild = Observable.Return("I am the only OnNext");
            //singleChild.SubscribleConsole();

            // **************************************
            // Return: Create an observable that never ends
            // **************************************
            var neverEnding = Observable.Never<string>();
            //neverEnding.SubscribleConsole();

            // **************************************
            // Return: Create an empty observable, 
            //  i.e. immediately OnComplete
            // **************************************
            var empty = Observable.Empty("Empty");
            //empty.SubscribleConsole();

            // **************************************
            // Return: Create an observable that throws an exception
            // **************************************
            var throwsError = Observable.Throw<ApplicationException>(new ApplicationException("This error shall throw up"));
            //throwsError.SubscribleConsole();

            var deferred = Observable.Defer(() =>
            {
                return Enumerable.Range(1, 10).ToObservable();
            });
            deferred.SubscribleConsole();

            Console.ReadLine();
        }
    }

    public static class ObservableExtensions
    {
        public static IObservable<string> ReadFileToObservable(this string fileName)
        {
            //This crazy-looking statement reads a file into an observable
            IObservable<string> lines = Observable.Using(() => File.OpenText(fileName), stream => Observable.Generate(stream, s => !s.EndOfStream, s => s, s => s.ReadLine()));
            return lines;
        }
    }

    public class ConsoleObserver<T> : IObserver<T>
    {
        private readonly string name;

        public ConsoleObserver(string name = "")
        {
            this.name = name;
        }

        public void OnCompleted()
        {
            Console.WriteLine($"{name} - OnCompleted");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"{name} - OnError");
            Console.WriteLine($"\t {error}");
        }

        public void OnNext(T value)
        {
            Console.WriteLine($"{name} - OnNext({value})");
        }
    }

    public static class ObserverExtensions
    {
        public static IDisposable SubscribleConsole<T>(this IObservable<T> observable, string name = "")
        {
            return observable.Subscribe(new ConsoleObserver<T>(name));
        }
    }

}
