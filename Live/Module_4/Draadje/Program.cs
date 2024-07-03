



using System.Globalization;
using System.Threading.Tasks.Dataflow;

namespace Draadje
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            //{
            //    Console.WriteLine("Foutje");
            //    Console.WriteLine(e.ExceptionObject);
            //};
            //SynchroonAdd();
            //AsyncAddBasic();
            //ThreadPoolAdd();
            //ApmAdd();
            //TplAdd();
            //IetwatSlimmeAdd();
            //FoutenInTask();
            //CancellenTask();
            //ParellelTasks();
            // Hakken();
            //DeGarage();
            //AsyncRekenenAsync();
            //CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            //Console.WriteLine(ci.);
            //Console.WriteLine();

            Console.WriteLine("Eind programma");
            Console.ReadLine();
        }

        private static async Task AsyncRekenenAsync()
        {
            int a = 0;
            int b = 0;

            //Mutex m = new Mutex()
            AutoResetEvent zl1 = new AutoResetEvent(false);
            AutoResetEvent zl2 = new AutoResetEvent(false); 

            var t1 = Task.Run(() =>
            {
                Task.Delay(2000).Wait();
                a = 100;
                //zl1.Set();
            });
            var t2 = Task.Run(() =>
            {
                Task.Delay(3000).Wait();
                b = 200;
                //zl2.Set();
            });

            //WaitHandle.WaitAll([zl1, zl2]);
            await Task.WhenAll(t1, t2);

            Console.WriteLine(a + b) ;
        }

        private static void DeGarage()
        {
            Semaphore garage = new Semaphore(10, 10);
            Barrier barrier = new Barrier(20);
            Parallel.For(0, 50, idx =>
            {
                barrier.SignalAndWait();

                Console.WriteLine($"Auto{Thread.CurrentThread.ManagedThreadId} komt bij de garage");
                garage.WaitOne();
                Console.WriteLine($"Auto{Thread.CurrentThread.ManagedThreadId} gaat shoppen");
                Task.Delay(10000 + Random.Shared.Next(3000, 6000)).Wait();
                garage.Release();
                Console.WriteLine($"Auto{Thread.CurrentThread.ManagedThreadId} rijdt de garage uit");
            });
        }

        static object stokje = new object();

        private static void Hakken()
        {
            int counter = 0;
            Barrier barrier = new Barrier(20);
            Parallel.For(0, 20, idx =>
            {
                //Monitor.Enter(stokje);
                barrier.SignalAndWait();
                lock (stokje)
                {
                    int tmp = counter;
                    Task.Delay(100).Wait();
                    counter = ++tmp;
                }
                ///Monitor.Exit(stokje);
                Console.WriteLine(counter);
            });
        }

        private static void ParellelTasks()
        {
            ThreadPool.SetMinThreads(20, 20);
            Console.WriteLine($"Nr of Threads {ThreadPool.ThreadCount}");
            var options = new ParallelOptions { MaxDegreeOfParallelism = 40 };
            Parallel.For(0, 40, options, idx =>
            {
                Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}");
                Task.Delay(Random.Shared.Next(3000, 6000)).Wait();
            });
            Console.WriteLine($"Nr of Threads {ThreadPool.ThreadCount}");
            Console.WriteLine("Tweede run");
            Parallel.For(0, 40, async idx =>
            {
                Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}");
                await Task.Delay(Random.Shared.Next(3000, 6000));
            });
        }

        private static void CancellenTask()
        {
            CancellationTokenSource nikko = new CancellationTokenSource();

            CancellationToken bommetje = nikko.Token;


            //Task.Run(() => {
            //    int teller = 0;
            //    do
            //    {
            //        if (bommetje.IsCancellationRequested)
            //        {
            //            Console.WriteLine("Boooom!");
            //            return;
            //        }
            //        Task.Delay(1000).Wait();
            //        Console.WriteLine(++teller  );
            //    }
            //    while(true);
            //});

            DoeIetsAsync(bommetje);

            Console.WriteLine("We gaaan verder");
            nikko.CancelAfter(5000);
        }

        static async void DoeIetsAsync(CancellationToken t = default)
        {
            await Task.Run(() =>
            {
                int teller = 0;
                do
                {
                    if (t.IsCancellationRequested)
                    {
                        Console.WriteLine("Boooom!");
                        return;
                    }
                    Task.Delay(1000).Wait();
                    Console.WriteLine(++teller);
                }
                while (true);
            });


        }

        private static async void FoutenInTask()
        {
            try
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Taak is gestart");
                    throw new Exception("Ooops");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Task.Run(() =>
            {
                Console.WriteLine("Taak is gestart");
                throw new Exception("Ooops");
            }).ContinueWith(pt =>
            {
                Console.WriteLine(pt.Status);
                if (pt.Exception != null)
                {
                    Console.WriteLine(pt.Exception.InnerException.Message);
                }
            });
        }

        private static async void IetwatSlimmeAdd()
        {
            Task<int> t1 = Task.Run(() => LongAdd(1, 2));
            int result = await t1;
            Console.WriteLine($"result: {result}");
            int x = 10;
            Console.WriteLine(x);
            result = await Task.Run(() => LongAdd(x, 2));

            Console.WriteLine($"result: {result}");

            result = await LongAddAsync(20, 30);
            Console.WriteLine(result);

        }

        private static void TplAdd()
        {
            Console.WriteLine($"Nr of threads: {ThreadPool.ThreadCount}");
            Task<int> t = new Task<int>(() =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                int result = LongAdd(2, 3);
                return result;
            });

            t.ContinueWith(pt =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(pt.Result);
            }).ContinueWith(pt =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Laatste");
            });
            t.Start();
            //Console.WriteLine(t.Result);

            Task.Run<int>(() => LongAdd(70, 80)).ContinueWith(pt =>
            {
                //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(pt.Result);
            });

            Console.WriteLine($"Nr of threads: {ThreadPool.ThreadCount}");
        }

        private static void ApmAdd()
        {
            Func<int, int, int> fn = LongAdd;
            IAsyncResult ar = fn.BeginInvoke(4, 5, x =>
            {
                int nr = (int)x.AsyncState;
                int result = fn.EndInvoke(x);
                Console.WriteLine(result);
            }, 42);
        }

        private static void ThreadPoolAdd()
        {
            ThreadPool.QueueUserWorkItem((s) =>
            {
                int result = LongAdd(2, 3);
                Console.WriteLine(result);
            });
        }

        private static void AsyncAddBasic()
        {
            Thread t = new Thread(() =>
            {
                int result = LongAdd(2, 3);
                Console.WriteLine(result);
            });
            t.Start();
        }

        private static void SynchroonAdd()
        {
            int result = LongAdd(2, 3);
            Console.WriteLine(result);
        }

        static int LongAdd(int a, int b)
        {
            Task.Delay(5000).Wait();
            return a + b;
        }
        static Task<int> LongAddAsync(int a, int b)
        {
            return Task.Run(() => LongAdd(a, b));

        }
    }
}
