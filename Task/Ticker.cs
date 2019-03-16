using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroidLord.Task;
using System.Threading;
using System.Collections;

namespace DroidLord.Task
{
    public class Job : Dispatchable
    {
        Action func;

        public Job(Action func)
        {
            this.func = func;
        }

        public virtual void Execute(object sender, int ThreadIndex, object Parameter)
        {
            func?.DynamicInvoke();
        }
    }

    public class Ticker
    {
        Dispatcher clock;
        Hashtable jobs;

        public Hashtable Params;

        public Ticker()
        {
            Params = new Hashtable();
            clock = new Dispatcher();
            clock.Dispatch(1, 1000);
            jobs = new Hashtable();
        }

        public void Mount(string Name, TickerJob job)
        {
            jobs[Name] = job;
            clock.Append(job, this, -1, false);               
        }            

        public void Unmount(string Name)
        {
            clock.Remove(jobs[Name] as TickerJob);
        }
    }   
    
    public class TickerJob : Job
    {
        int _Accumulater = 0;

        public TickerJob(Action func) : base(func)
        {
        }

        public override void Execute(object sender, int ThreadIndex, object Parameter)
        {
            if (++_Accumulater >= Interval)
            {
                // 到时间
                _Accumulater = 0;
                base.Execute(sender, ThreadIndex, Parameter);
            }                     
        }

        public int Interval { get; set; } = 1; 
    }    
}
