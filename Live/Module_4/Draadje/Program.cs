



namespace Draadje
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //SynchroonAdd();
            //AsyncAddBasic();
            //ThreadPoolAdd();
            //ApmAdd();
            TplAdd();
            Console.WriteLine("Eind programma");
            Console.ReadLine();
        }

        private static void TplAdd()
        {
            Task<int> t = new Task<int>(() => {
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
        }

        private static void ApmAdd()
        {
            Func<int, int, int> fn = LongAdd;
            IAsyncResult ar = fn.BeginInvoke(4, 5, x =>{
                int nr = (int)x.AsyncState;
                int result = fn.EndInvoke(x);
                Console.WriteLine(result);
            }, 42);          
        }

        private static void ThreadPoolAdd()
        {
            ThreadPool.QueueUserWorkItem((s)=> { 
                int result = LongAdd(2, 3);
                Console.WriteLine(result);
            });
        }

        private static void AsyncAddBasic()
        {
            Thread t = new Thread(() => {
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
    }
}
