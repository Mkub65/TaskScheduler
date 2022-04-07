namespace TaskScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Milestone : AbstractTask
    {
        public Milestone()
        {
            this.Children = new List<AbstractTask>();
        }

        public override DateTime StartDate
        {
            get
            {
                return this.List.Where(x => x.ID.Contains(this.ID + ".")).ToList().Select(x => x.StartDate).Min();
            }
        }

        public override DateTime EndDate
        {
            get
            {
                return this.List.Where(x => x.ID.Contains(this.ID + ".")).ToList().Select(x => x.EndDate).Max();
            }
        }

        public List<AbstractTask> Children { get; }

        public static DateTime GetStartDate()
        {
            return new DateTime();
        }
    }
}
