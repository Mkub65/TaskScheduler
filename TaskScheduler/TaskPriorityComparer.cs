using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler
{
    internal class TaskPriorityComparer : IComparer<AbstractTask>
    {
        public int Compare(AbstractTask? x, AbstractTask? y)
        {
           return x.Priority.CompareTo(y.Priority);
        }
    }
}
