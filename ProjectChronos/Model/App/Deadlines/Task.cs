using ProjectChronos.Model.App.Const;

namespace ProjectChronos.Model.App.Deadlines
{
    public class Task
    {
        public string Text {  get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DeadlineTime { get; set; }
        public DateTime SetTime { get; set; }
        public Priority Priority { get; set; }
        public bool IsInProcess { get; set; }
    }
}
