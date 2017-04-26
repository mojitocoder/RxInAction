using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reactive.Linq;

namespace RxInAction
{
    class Program
    {
        static void Main(string[] args)
        {
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
            throwsError.SubscribleConsole();



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
