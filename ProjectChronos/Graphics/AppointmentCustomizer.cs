using DevExpress.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProjectChronos.Models.App.Const;

namespace ProjectChronos.Graphics
{
    class AppointmentCustomizer : IAppointmentCustomizer
    { 
        
        public List<EventInfo> infos {  get; set; }
        public void Customize(AppointmentViewModel appointment)
        {
            appointment.TextFontSize = 14;
            appointment.ContentPadding = 1;
            appointment.BorderThickness = 0;
            
        }
    }
}
