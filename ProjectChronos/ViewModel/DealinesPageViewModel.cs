using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using ProjectChronos.Model.App.Deadlines;
using ProjectChronos.Services;
using ProjectChronos.View;
using System.Collections.ObjectModel;
using Task = System.Threading.Tasks.Task;

namespace ProjectChronos.ViewModel
{

    public partial class DeadlinesPageViewModel : BaseViewModel
    {
        public ObservableCollection<DeadlineInfo> Deadlines { get; } = new();
        StorageService StorageService;

        #region addEdit/details
        [ObservableProperty]
        bool isBottomSheetPresented = false;
        [ObservableProperty]
        DeadlineInfo deadlineObj = new();
        [ObservableProperty]
        TimeSpan timeObj = new();
        [ObservableProperty]
        string addEditPageTitle;
        public ObservableCollection<Model.App.Deadlines.Task> Tasks { get; } = new();
        #endregion
        public DeadlinesPageViewModel(StorageService storageService) {
            Title = "Deadlines";
            StorageService = storageService;
            var deadlines = StorageService.GetDeadlines();
            if (deadlines is null || deadlines.Count < 1) return;
            foreach(var deadline in deadlines) {
                Deadlines.Add(deadline);
            }
            //var d = new DeadlineInfo
            //{
            //    Id = Guid.NewGuid(),
            //    Title = "Title",
            //    Description = "Примітка: Переконайтеся, що ви додали шрифт і відповідні ресурси до вашого проекту та правильно оновили ваші файли конфігурації. Якщо у вас виникли проблеми або вам потрібна додаткова допомога, будь ласка, дайте мені знати! 😊",
            //    DeadlineTime = DateTime.Now.AddDays(Random.Shared.Next(1,30)),
            //    SetTime = DateTime.Now.AddDays(Random.Shared.Next(1, 30)),
            //};
            //for(int i = 0; i < 15;i++) Deadlines.Add(d);
        }

        [RelayCommand]
        async Task LeftSwipeAsync(DeadlineInfo info) {
            Vibration.Vibrate(TimeSpan.FromMilliseconds(30));
            await Task.Delay(100);
            Vibration.Vibrate(TimeSpan.FromMilliseconds(30));
            Vibration.Default.Vibrate(20);
            var result = await Shell.Current.DisplayAlert("Delete deadline?",$"Are you sure you want to delete deadline with id {info.Id}?","Yes","No");
            if (result) Deadlines.Remove(info);      
        }

        [RelayCommand]
        async Task AddCustomDeadlineAsync() {
            AddEditPageTitle = "Add deadline";
            DeadlineObj = new();
            TimeObj = new();
            DeadlineObj.DeadlineTime = DateTime.Now;
            TimeObj = DeadlineObj.DeadlineTime.TimeOfDay;
            Tasks.Clear();
            Tasks.Add(new Model.App.Deadlines.Task { Text = "Fat Fuck"});
            await Shell.Current.Navigation.PushAsync(new AddDeadlinePage(this));  
        }
        [RelayCommand]
        [Obsolete]
        void TaskTextEditorChanged() {
            Device.BeginInvokeOnMainThread(()=>
                {
                    try
                    {
                        if (Tasks.Count == 0 || Tasks == null) return;
                        for(int i = 0; i < Tasks.Count; i++)
                        {
                            if (Tasks[i].Text == string.Empty && Tasks[i] != Tasks.Last()) Tasks.RemoveAt(i);
                        }
                        if (Tasks.Last().Text != string.Empty) Tasks.Add(new Model.App.Deadlines.Task() { Text = string.Empty });
                    }
                    catch (Exception ex)
                    {
                        Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                    }
                }
                );
            
        }

        [RelayCommand]
        void RemoveTask(Model.App.Deadlines.Task task) {
                try {
                if (task.Text == string.Empty) return;
            Tasks.Remove(task);
                if (Tasks.Count == 0) Tasks.Add(new Model.App.Deadlines.Task() {Text = string.Empty });
            }
            catch (Exception ex) {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

        }

        [RelayCommand]
        async Task SubmitDeadline()
        {
            if (DeadlineObj != null) 
            {
                DeadlineObj.DeadlineTime += TimeObj;
                DeadlineObj.Id = Guid.NewGuid();
                Deadlines.Add(DeadlineObj); 
            }
            StorageService.SaveDeadlines(Deadlines.ToList());
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand]
        void ShowBottomDeadlineDetailsSheet(DeadlineInfo info) {
        DeadlineObj = info;
        IsBottomSheetPresented = true;
        }

    }
}
