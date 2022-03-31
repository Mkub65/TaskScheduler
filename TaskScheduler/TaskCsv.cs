namespace TaskScheduler
{
    using System.Globalization;
    using System.Text;
    using CsvHelper;
    using CsvHelper.Configuration;

    public class TaskCsv : IComparable<TaskCsv>
    {
        public TaskCsv()
        {
        }

        public string? ID { get; set; }

        public int? Priority { get; set; }

        public string? Description { get; set; }

        public string? Predecessors { get; set; }

        public int? Work { get; set; }

        public string? Responsible { get; set; }

        public DateTime MinStartDate { get; set; }

        public DateTime MaxEndDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public static List<AbstractTask> ToList(List<TaskCsv> listCsv)
        {
            var taskList = new Dictionary<string, AbstractTask>();

            for (int i = 0; i < listCsv.Count; i++)
            {
                taskList.Add(listCsv[i].ID, new Task
                {
                    ID = listCsv[i].ID,
                    Priority = listCsv[i].Priority.HasValue ? listCsv[i].Priority.Value : int.MaxValue,
                    Description = listCsv[i].Description,
                    Predecessors = new List<AbstractTask>(),
                    Work = listCsv[i].Work,
                    Responsible = listCsv[i].Responsible,
                    MinStartDate = listCsv[i].MinStartDate,
                    MaxEndDate = listCsv[i].MaxEndDate,
                });
            }

            for (int i = 0; i < listCsv.Count; i++)
            {
                if (listCsv[i].Predecessors != string.Empty)
                {
                    foreach (var item in listCsv[i].Predecessors.Split(","))
                    {
                        taskList[listCsv[i].ID].Predecessors.Add(taskList[item.Trim()]);
                    }
                }
            }

            var taskList2 = taskList.Select(task => task.Value).ToList();
            foreach (var task in taskList2)
            {
                task.List = taskList2;
            }

            taskList2.Sort(new TaskPriorityComparer());

            return taskList2;
        }

        public static List<TaskCsv> FromList(List<Task> list)
        {
            var taskList = new List<TaskCsv>();

            for (int i = 0; i < list.Count; i++)
            {
                taskList.Add(new TaskCsv
                {
                    ID = list[i].ID,
                    Priority = list[i].Priority != int.MaxValue ? list[i].Priority : null,
                    Description = list[i].Description,
                    Predecessors = string.Empty,
                    Work = list[i].Work,
                    Responsible = list[i].Responsible,
                    MinStartDate = list[i].MinStartDate,
                    MaxEndDate = list[i].MaxEndDate,
                    StartDate = list[i].StartDate,
                    EndDate = list[i].EndDate,
                });
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Predecessors.Count > 0)
                {
                    foreach (var item in list[i].Predecessors)
                    {
                        taskList[i].Predecessors = $"{taskList[i].Predecessors}{item.ID},";
                    }
                }
            }

            return taskList;
        }

        public int CompareTo(TaskCsv other)
        {
            return this.ID.CompareTo(other.ID);
        }
    }
}
