﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using ProjectChronos.Graphics;
using ProjectChronos.Models.App.Deadlines;
using ProjectChronos.Services;
using ProjectChronos.Pages;
using System.Collections.ObjectModel;
using Task = System.Threading.Tasks.Task;

namespace ProjectChronos.ViewModels
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

        [ObservableProperty]
        CompleteCircleDrawing progress = new() { NumOfCompletedTasks = 1, TotalTasks = 10 };
        public ObservableCollection<Models.App.Deadlines.Task> Tasks { get; } = new();
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
            if (info is null) {
                await Shell.Current.DisplayAlert("Error!", "info is null", "OK");
                return;
            }
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            var result = await Shell.Current.DisplayAlert("Delete deadline?",$"Are you sure you want to delete deadline with id {info.Id}?","Yes","No");
            if (result) Deadlines.Remove(info);      
        }

        [RelayCommand]
        async Task AddCustomDeadlineAsync() {
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            AddEditPageTitle = "Add deadline";
            DeadlineObj = new();
            TimeObj = new();
            DeadlineObj.DeadlineTime = DateTime.Now;
            TimeObj = DeadlineObj.DeadlineTime.TimeOfDay;
            Tasks.Clear();
            Tasks.Add(new Models.App.Deadlines.Task { Text = "Fat Fuck" });
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
                        if (Tasks.Last().Text != string.Empty) Tasks.Add(new Models.App.Deadlines.Task() { Text = string.Empty });
                    }
                    catch (Exception ex)
                    {
                        Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                    }
                }
                );
            
        }

        [RelayCommand]
        void RemoveTask(Models.App.Deadlines.Task task) {
                try {
                if (task.Text == string.Empty) return;
            Tasks.Remove(task);
                if (Tasks.Count == 0) Tasks.Add(new Models.App.Deadlines.Task() {Text = string.Empty });
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
                DeadlineObj.SetTime = DateTime.Now;
                DeadlineObj.IsInProcess = false;
                DeadlineObj.IsCompleted = false;
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
