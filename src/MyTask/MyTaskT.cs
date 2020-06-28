using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskDemo
{

    public class MyTask<T>
    {
        private readonly Func<T> _func;
        private ManualResetEvent _resetEvent;

        public MyTask(Func<T> func)
        {
            _func = func;
        }

        private T _result;

        public T Result
        {
            get
            {
                _resetEvent.WaitOne();
                return _result;
            }
            private set
            {
                _result = value;
            }
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
                _result = _func();
                _resetEvent.Set();
            });

            //thread.Start();
        }

        public void Wait(int ms = -1)
        {
            _resetEvent.WaitOne(ms);
        }

      


    }

}