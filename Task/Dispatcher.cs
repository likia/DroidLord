using System;
using System.Collections;
using System.Threading;

namespace DroidLord.Task
{
    public interface Dispatchable
    {
        void Execute(object sender, int ThreadIndex, object Parameter);
    }

    public enum DispatcherState
    {
        IDLE = 0,
        INIT = 1,
        BUSY = 2,
        HALTING = 3
    }

    class JobItem
    {
        public int Repeats;
        public bool Wait;
        public Dispatchable Job;
        public object Parameter;
    }

    public delegate void DispatchFinishedHandler(object sender);

    class ThreadProxyArgs
    {
        public JobItem Job;
        public int ThreadIndex;
    }

    class WorkerArgs
    {
        public int ThreadIndex;
    }

    public class Dispatcher
    {
        private Queue DispatchQueue;
        public DispatcherState State;
        private Thread[] WorkingThread;
        private int Living;
        private object LivingLock;
        private int ExecuteInterval;
        public object Tag;
        public event DispatchFinishedHandler DispatchFinished;
        public bool AutoStop = false;

        private object limitLock = new object();
        private int limit = -1;
        private int limitAvailale = 0;

        public static void AwaitBackgroundThread(ThreadStart _delegate, Action finished)
        {
            var thread = new Thread(() => {
                _delegate.Invoke();
                finished.Invoke();
            }) { IsBackground = true };                       
        }

        /// <summary>
        /// 开启新的后台线程
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Thread BackgroundThread(ThreadStart _delegate)
        {
            var thread = new Thread(_delegate) { IsBackground = true };
            thread.Start();
            return thread;
        }

        public static Timer StartTicking(TimerCallback callback, int interval = 1000, object param = null, int delay = 0)
        {
            return new Timer(callback, param, delay, interval);            
        }

        public int Limit
        {
            get { return limit; }
            set
            {
                limit = value;
            }
        }


        public Dispatcher()
        {
            LivingLock = new object();
            Tag = null;
            State = DispatcherState.IDLE;
            DispatchQueue = Queue.Synchronized(new Queue());

        }

        public void Clear()
        {
            DispatchQueue.Clear();
        }

        public void Append(Dispatchable Job, object Parameter, int Repeats, bool Wait)
        {
            JobItem NewJob = new JobItem();
            NewJob.Repeats = Repeats;
            NewJob.Wait = Wait;
            NewJob.Job = Job;
            NewJob.Parameter = Parameter;
            lock (DispatchQueue.SyncRoot)
            {
                DispatchQueue.Enqueue(NewJob);
            }
        }

        public void Remove(Dispatchable Job)
        {
            lock (DispatchQueue.SyncRoot)
            {
                var array = DispatchQueue.ToArray();
                var list = new ArrayList(array);
                list.Remove(Job);
                DispatchQueue = new Queue(list);
            }
        }

        public bool Dispatch(int ThreadCount, int ExecuteInt)
        {
            if (State != DispatcherState.IDLE) return false;
            State = DispatcherState.INIT;
            Living = ThreadCount;
            ExecuteInterval = ExecuteInt;
            limitAvailale = Limit;
            WorkingThread = new Thread[ThreadCount];
            State = DispatcherState.BUSY;
            for (int i = 0; i < WorkingThread.Length; i++)
            {
                WorkerArgs Args = new WorkerArgs() { ThreadIndex = i };
                WorkingThread[i] = new Thread(new ParameterizedThreadStart(ThreadWork)) { IsBackground = true };
                WorkingThread[i].Start(Args);
            }
            return true;
        }

        public bool Halt()
        {
            if (State != DispatcherState.BUSY) return false;
            State = DispatcherState.HALTING;
            BackgroundThread(() =>
            {
                for (int i = 0; i < WorkingThread.Length; i++)
                {
                    try
                    {
                        WorkingThread[i].Abort();
                    }
                    catch
                    {
                    }
                }
                State = DispatcherState.IDLE;
                if (DispatchFinished != null) DispatchFinished(this);
            });            
            return true;
        }

        private void ExecuteProxy(object Obj)
        {
            ThreadProxyArgs Args = (ThreadProxyArgs)Obj;
            JobItem Job = Args.Job;
            Job.Job.Execute(this, Args.ThreadIndex, Job.Parameter);
        }

        private void ThreadWork(object Param)
        {
            WorkerArgs WorkerArg = (WorkerArgs)Param;
            int i = WorkerArg.ThreadIndex;
            DateTime LastExecute = new DateTime(1970, 1, 1);
            JobItem Job;
            while (State == DispatcherState.BUSY)
            {
                lock (limitLock)
                {
                    //check thread limit
                    if (Limit > 0 && limitAvailale > 0)
                    {

                    }
                }

                lock (DispatchQueue.SyncRoot)
                {
                    if (DispatchQueue.Count == 0 || (DateTime.Now - LastExecute).TotalMilliseconds < ExecuteInterval)
                    {
                        goto workerWait;
                    }
                    Job = (JobItem)DispatchQueue.Dequeue();

                    if (Job.Repeats == -1 || (Job.Repeats--) > 0)
                    {
                        DispatchQueue.Enqueue(Job);

                    }
                }
                LastExecute = DateTime.Now;
                ThreadProxyArgs Args = new ThreadProxyArgs() { ThreadIndex = i, Job = Job };
                if (Job.Wait)
                {
                    ExecuteProxy(Args);
                }
                else
                {
                    Thread Worker = new Thread(new ParameterizedThreadStart(ExecuteProxy));
                    try
                    {
                        Worker.Start(Args);
                    }
                    catch
                    {

                    }
                }
                workerWait:
                Thread.Sleep(16);
                continue;
            }
        }
    }
}
