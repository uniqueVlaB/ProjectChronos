using ProjectChronos.Models.App.Const;


namespace ProjectChronos.Models.App.Deadlines
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
        public Color PriorityColor
        {
            get
            {
                return Priority switch
                {
                    Priority.Low => Colors.Green,
                    Priority.High => Colors.Red,
                    Priority.Normal => Colors.Gold,
                    _ => Colors.White,
                };
            }
            set { return; }
        }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }

    }
}
