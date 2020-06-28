using System;
using System.Threading.Tasks;
using System.Threading;
using TaskDemo;

namespace MyTaskDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DateTime dateTime = DateTime.Now;
            var dateTimeStr = dateTime.ToString();
            System.Console.WriteLine(dateTimeStr);

            var myTask = MyTask.Run(() => DateTime.Now);
            var myStrTask = MyTask.Run(() => myTask.Result.ToString());
            var print = MyTask.Run(() => Console.WriteLine(myStrTask.Result));


            // Console.WriteLine("Hello World!");

            bool run = true;
            Console.ForegroundColor = ConsoleColor.Green;
            Random r = new Random();
            MyTask task = new MyTask(() =>
            {
                while (run)
                {
                    Console.Write(r.Next(0, 9));
                }
            });

            task.Start();

            task.Wait(500);
            run = false;

            Console.ReadLine();
            

        }
    }
}
