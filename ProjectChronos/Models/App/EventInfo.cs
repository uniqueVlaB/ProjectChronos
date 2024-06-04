using ProjectChronos.Models.Cist.Events;
using System.Text;


namespace ProjectChronos.Models.App
{
    public class EventInfo
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Lesson Lesson { get; set; }
        public string ShortType { get; set; }
        public string FullType { get; set; }
        public string BaseTypeName { get; set; }
        public int PairNumber { get; set; }
        private List<Teacher> _teachers;
        public List<Teacher> Teachers { get { return _teachers; } set { _teachers = value; ComposeTeachersString(); } }

        private List<Group> _groups;
        public List<Group> Groups { get { return _groups; } set { _groups = value; ComposeGroupString(); } }
        public Color Color { get; set; }
        public string Location { get; set; }

        public string GroupsString { get; private set; }
        public string TeachersString { get; private set; }

        public string AppointmentString 
        { 
            get 
            {
                return $"{Lesson.ShortName}\n{StartTime.ToString("HH:mm")}\n   {EndTime.ToString("HH:mm")}";
            } 
            set 
            {
                return;
            } 
        }

        private void ComposeGroupString() {
            string str = string.Empty;
            if (_groups.Count == 0) GroupsString = str;
            else if (_groups.Count == 1) GroupsString = _groups[0].Name;
            else
            {
                str = _groups[0].Name;
                for (short i = 1; i < _groups.Count; i++)
                {
                    str += ", " + _groups[i].Name;
                }
                GroupsString = str;
                
            }
        }
        private void ComposeTeachersString()
        {
            string str = string.Empty;
            if (_teachers.Count == 0) TeachersString = str;
            else if(_teachers.Count == 1) TeachersString = _teachers[0].FullName;
            else
            {
                str = _teachers[0].FullName;
                for (short i = 1; i < _teachers.Count; i++)
                {
                    str += ", " + _teachers[i].FullName;
                }
                TeachersString = str;
            }
        }
    }
}
