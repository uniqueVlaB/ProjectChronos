using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectChronos.Model.App
{
    public class DeadlineInfo
    {
        public DateTime DeadlineTime { get; set; }
        public DateTime SetTime { get; set; }
        public string Title { get; set;}
        public string Description { get; set;}
        public string DeadlineTimeString { get { return DeadlineTime.ToString("dd.MM.yyyy HH:mm"); } }

    }
}
