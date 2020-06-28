using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskDemo
{

    public class MyTask
    {
        private readonly Action _action;
        private ManualResetEvent _resetEvent; 

        public MyTask(Action action)
        {
            _action = action;
        }

        public void Start()
        {
            _resetEvent = new ManualResetEvent(false);
            //Thread thread = new Thread(() =>
            //{
            //    _action();
            //    _resetEvent.Set();
            //});

            ThreadPool.QueueUserWorkItem((_) =>
            {
                _action();
                _resetEvent.Set();
            });

            //thread.Start();
        }

        public void Wait(int ms = -1)
        {
            _resetEvent.WaitOne(ms);
        }

        public static MyTask Run(Action action)
        {
            var task = new MyTask(action);
            task.Start();
            return task;
        }

        public static MyTask<T> Run<T>(Func<T> func)
        {
            var task = new MyTask<T>(func);
            task.Start();
            return task;
        }
    }

}