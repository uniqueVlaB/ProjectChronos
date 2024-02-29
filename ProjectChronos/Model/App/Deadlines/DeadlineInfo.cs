using ProjectChronos.Model.App.Const;


namespace ProjectChronos.Model.App.Deadlines
{

    public class DeadlineInfo
    {

        public Guid Id { get; set; }
        public DateTime DeadlineTime { get; set; }
        public DateTime SetTime { get; set; }
        public string Title { get; set;}
        public string Description { get; set;}
        public string DeadlineTimeString
        {
            get { return DeadlineTime.ToString("dd.MM.yyyy HH:mm"); }
            set { return; }
        }

        private List<Task> _tasks;
        public List<Task> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                IsInProcess = _tasks.Any(t => t.IsInProcess);
            }
        }

        public Priority Priority { get; set; }

        public bool IsInProcess { get;  set; }

        //public int NumOfCompletedTasks { get { return _tasks.Where(t => t.IsCompleted).Count();} set { NumOfCompletedTasks = value; } }
    }
}
