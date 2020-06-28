using System;

namespace async
{

    public delegate void OnAddCompleted(int res);

    class Calc
    {
        public Func<int> Add(Func<int> a, Func<int> b)
        {
            return new Func<int>(() => a() + b());
        }

        public OnAddCompleted OnAddCompletedEvent;

        public void AddAsync(int a, int b)
        {
            OnAddCompletedEvent(a + b);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Calc c = new Calc();
            var res = c.Add(() => 3, () => 4)();
            System.Console.WriteLine(res);

            c.OnAddCompletedEvent = new OnAddCompleted(target);
            c.OnAddCompletedEvent += new OnAddCompleted(target);
            c.OnAddCompletedEvent -= new OnAddCompleted(target);
            c.OnAddCompletedEvent = target;

            c.OnAddCompletedEvent += delegate (int res)
            {
                System.Console.WriteLine(res);
            };

            c.OnAddCompletedEvent += (int res) =>
            {
                System.Console.WriteLine(res);
            };


            c.OnAddCompletedEvent += res =>
            {
                System.Console.WriteLine(res);
            };

            c.OnAddCompletedEvent += System.Console.WriteLine;


            c.AddAsync(4, 5);



        }

        public static void target(int res)
        {
            System.Console.WriteLine(res);
        }
    }
}
