using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Timers;

namespace LaunchpadMonitor
{
    class Gatherer : IDisposable
    {
        ISampler sampler;
        Timer timer;
        List<int> history = new List<int>();
        public string Name { get; private set; }

        private object HistoryLock = new object();

        public Gatherer(ISampler sampler, int interval, string name)
        {
            this.sampler = sampler;
            this.Name = name;
            timer = new Timer();
            timer.Interval = interval;
            timer.Elapsed += new ElapsedEventHandler(Gather);
            timer.Enabled = true;
        }

        public void Dispose()
        {
            timer.Enabled = false;
        }

        private void Gather(object source, ElapsedEventArgs e)
        {
            int sample = sampler.Sample();
            lock (HistoryLock)
            {
                history.Add(sample);
            }
        }

        public int Latest
        {
            get
            {
                lock (HistoryLock)
                {
                    return history.Last();
                }
            }
        }

        public IEnumerable<int> LatestN(int count)
        {
            lock (HistoryLock)
            {
                if (count > history.Count)
                {
                    return history;
                }
                return history.GetRange(history.Count - count, count);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
