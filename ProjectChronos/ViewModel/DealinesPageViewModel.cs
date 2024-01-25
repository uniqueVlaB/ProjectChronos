using ProjectChronos.Model.App;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectChronos.ViewModel
{

    public partial class DeadlinesPageViewModel : BaseViewModel
    {
        public ObservableCollection<DeadlineInfo> Deadlines { get; } = new();
        public DeadlinesPageViewModel() {
            Title = "Deadlines";
            var d = new DeadlineInfo
            {
                Title = "Title",
                Description = "Примітка: Переконайтеся, що ви додали шрифт і відповідні ресурси до вашого проекту та правильно оновили ваші файли конфігурації. Якщо у вас виникли проблеми або вам потрібна додаткова допомога, будь ласка, дайте мені знати! 😊",
                DeadlineTime = DateTime.Now.AddDays(30),
                SetTime = DateTime.Now,
            };
            for(int i = 0; i < 15;i++) Deadlines.Add(d);
        }
    }
}
