namespace TaskScheduler
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Globalization;
    using CsvHelper;
    using CsvHelper.Configuration;

    public class Task : IComparable<Task>
    {
        public Task()
        {
            StartDateCalculated = false;
        }

        public List<Task> list { get; set; }

        public string? ID { get; set; }

        public int? Priority { get; set; }

        public string? Description { get; set; }

        public List<Task> Predecessors { get; set; }

        public int? Work { get; set; }

        public string Responsible { get; set; }

        public DateTime MinStartDate { get; set; }

        public DateTime MaxEndDate { get; set; }

        public bool StartDateCalculated { get; set; }

        public DateTime StartDate 
        {
            get
            {
                //We should based only on already calculated Task
                if (!StartDateCalculated)
                {
                    this.startDate = FindFirstEmptySlot(DateTimeMethode.Max(MinStartDate, Predecessors), list.Where(x => x.StartDateCalculated && x.Responsible == Responsible).ToList());
                    StartDateCalculated = true;
                }

                return startDate;
            }
        }

        private DateTime startDate;

        public DateTime? EndDate {
            get { return StartDate.AddDays((double)Work.GetValueOrDefault(0)); }
        }

        public int CompareTo(Task other)
        {
            if (StartDateCalculated && other.StartDateCalculated)
            {
                return this.StartDate.CompareTo(other.StartDate);
            }
            else if (StartDateCalculated)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public DateTime FindFirstEmptySlot(DateTime startDate, List<Task> taskList)
        {
            taskList.Sort();
            // there is need to check space betwean startDate and firstCalulcatedTask
            if (startDate.AddDays(this.Work.GetValueOrDefault(0)) < taskList[0].StartDate)
            {
                return startDate;
            }

            for (int i = 0; i < taskList.Count - 1; i++)
            {
                if (taskList[i].EndDate.Value.AddDays(this.Work.GetValueOrDefault(0)) < taskList[i + 1].StartDate)
                {
                    return taskList[i].EndDate.GetValueOrDefault(DateTime.MinValue);
                }
            }

            if (taskList.Count == 0 || startDate > taskList[taskList.Count - 1].EndDate.Value)
            {
                return startDate;
            }
            else
            {
                return taskList[taskList.Count - 1].EndDate.Value;
            }
        }
    }
}
