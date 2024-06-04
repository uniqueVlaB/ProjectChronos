using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using ProjectChronos.Graphics;
using ProjectChronos.Models.App.Deadlines;
using ProjectChronos.Services;
using ProjectChronos.Pages;
using System.Collections.ObjectModel;
using Task = System.Threading.Tasks.Task;
using ProjectChronos.Models.App.Const;
using Microsoft.Maui.Layouts;

namespace ProjectChronos.ViewModels
{

    public partial class DeadlinesPageViewModel : BaseViewModel
    {
        List<DeadlineInfo> deadlineInfos;

        public ObservableCollection<DeadlineInfo> Deadlines { get; } = new();
        StorageService StorageService;
        [ObservableProperty]
        bool isSortAscending = false;
        [ObservableProperty]
        Color ascendingColor = Colors.Gray;
        [ObservableProperty]
        Color descendingColor = Colors.White;
        [ObservableProperty]
        short sortByIndex;
        [ObservableProperty]
        bool isBottomSheetPresented = false;
        [ObservableProperty]
        DeadlineFilters filters = new();

        #region addEdit/details
        [ObservableProperty]
        DeadlineInfo deadlineObj = new();
        [ObservableProperty]
        TimeSpan timeObj = new();
        [ObservableProperty]
        string addEditPageTitle;
        [ObservableProperty]
        short priorityIndex = 0;
        #endregion
        public DeadlinesPageViewModel(StorageService storageService) {
            Title = "Deadlines";
            StorageService = storageService;
            deadlineInfos = StorageService.GetDeadlines();

            if (deadlineInfos is null || deadlineInfos.Count < 1) return;
            foreach (var deadline in deadlineInfos) {
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
        async Task ResetFiltersAsync()
        {
           await Task.Run(() => {
               Deadlines.Clear();
               foreach (var deadline in deadlineInfos)
               {
                   Deadlines.Add(deadline);
               }
           });
            IsBottomSheetPresented = false;
        }

        [RelayCommand]
        async Task ApplyFiltersAsync()
        {
            await Task.Run(() =>
            {
                var filteredList = deadlineInfos.AsEnumerable();

                if (Filters.DateSelected)
                {
                    filteredList = filteredList.Where(d => d.DeadlineTime >= Filters.FromDate.Date && d.DeadlineTime.Date <= Filters.ToDate);
                }

                if (Filters.OnlyCompletedSelected && !Filters.OnlyUncompletedSelected)
                {
                    filteredList = filteredList.Where(d => d.IsCompleted);
                }
                else if (Filters.OnlyUncompletedSelected && !Filters.OnlyCompletedSelected)
                {
                    filteredList = filteredList.Where(d => !d.IsCompleted);
                }

                if (Filters.PriorityLowSelected || Filters.PriorityNormalSelected || Filters.PriorityHighSelected)
                {
                    filteredList = filteredList.Where(d =>
                        (d.Priority == Priority.Low && Filters.PriorityLowSelected) ||
                        (d.Priority == Priority.Normal && Filters.PriorityNormalSelected) ||
                        (d.Priority == Priority.High && Filters.PriorityHighSelected)
                    );
                }

                var finalList = filteredList.ToList();

                Deadlines.Clear();
                foreach (var deadline in finalList)
                {
                    Deadlines.Add(deadline);
                }
            });

            IsBottomSheetPresented = false;
        }


        [RelayCommand]
        void ChangeBottomSheetState() {
            IsBottomSheetPresented = !IsBottomSheetPresented;
        }
        partial void OnSortByIndexChanged(short value)
        {
            Sort();
        }

        [RelayCommand]
        void Sort() {
            List<DeadlineInfo> sortedList;
            if (IsSortAscending)
            {
                switch (SortByIndex)
                {
                    case 0: sortedList = Deadlines.OrderBy(d => d.Priority).ToList(); break;
                    case 1: sortedList = Deadlines.OrderBy(d => d.DeadlineTime).ToList(); break;
                    case 2: sortedList = Deadlines.OrderBy(d => d.IsCompleted).ToList(); break;
                    default: return;
                }
            }
            else
            {
                switch (SortByIndex)
                {
                    case 0: sortedList = Deadlines.OrderByDescending(d => d.Priority).ToList(); break;
                    case 1: sortedList = Deadlines.OrderByDescending(d => d.DeadlineTime).ToList(); break;
                    case 2: sortedList = Deadlines.OrderByDescending(d => d.IsCompleted).ToList(); break;
                    default: return;
                }
            }

            for (int i = 0; i < sortedList.Count; i++)
            {
                if (!Deadlines[i].Equals(sortedList[i]))
                {
                    Deadlines.Move(Deadlines.IndexOf(sortedList[i]), i);
                }
            }
        }
    

        [RelayCommand]
        void ChangeSortState() {
            IsSortAscending = !IsSortAscending;
            if (IsSortAscending)
            {
                AscendingColor = Colors.White;
                DescendingColor = Colors.Gray;
            }
            else 
            {
                AscendingColor = Colors.Gray;
                DescendingColor = Colors.White;
            }
            Sort();
        }

        [RelayCommand]
        async Task DeleteAsync(DeadlineInfo info) {
            if (info is null) {
                await Shell.Current.DisplayAlert("Error!", "info is null", "OK");
                return;
            }
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            var result = await Shell.Current.DisplayAlert("Delete deadline?",$"Are you sure you want to delete deadline with id {info.Id}?","Yes","No");
            if (result) Deadlines.Remove(info);
            StorageService.SaveDeadlines(Deadlines.ToList());
        }

        [RelayCommand]
        async Task AddDeadlineAsync() {
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            AddEditPageTitle = "Add deadline";
            DeadlineObj = new();
            TimeObj = new();
            DeadlineObj.DeadlineTime = DateTime.Now;
            TimeObj = DeadlineObj.DeadlineTime.TimeOfDay;
            await Shell.Current.Navigation.PushAsync(new AddDeadlinePage(this));
        }
        //[RelayCommand]
        //[Obsolete]
        //void TaskTextEditorChanged() {
        //    Device.BeginInvokeOnMainThread(()=>
        //        {
        //            try
        //            {
        //                if (Tasks.Count == 0 || Tasks == null) return;
        //                for(int i = 0; i < Tasks.Count; i++)
        //                {
        //                    if (Tasks[i].Text == string.Empty && Tasks[i] != Tasks.Last()) Tasks.RemoveAt(i);
        //                }
        //                if (Tasks.Last().Text != string.Empty) Tasks.Add(new Models.App.Deadlines.Task() { Text = string.Empty });
        //            }
        //            catch (Exception ex)
        //            {
        //                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        //            }
        //        }
        //        );
            
        //}

        //[RelayCommand]
        //void RemoveTask(Models.App.Deadlines.Task task) {
        //        try {
        //        if (task.Text == string.Empty) return;
        //    Tasks.Remove(task);
        //        if (Tasks.Count == 0) Tasks.Add(new Models.App.Deadlines.Task() {Text = string.Empty });
        //    }
        //    catch (Exception ex) {
        //        Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        //    }

        //}

        [RelayCommand]
        async Task SubmitDeadline()
        {
            
            if (DeadlineObj != null) 
            {
                DeadlineObj.DeadlineTime = DeadlineObj.DeadlineTime.Date + TimeObj;
                DeadlineObj.Id = Guid.NewGuid();
                DeadlineObj.SetTime = DateTime.Now;
                DeadlineObj.IsCompleted = false;
                DeadlineObj.Priority = (Priority)PriorityIndex;
                Deadlines.Add(DeadlineObj);

                deadlineInfos.Add(DeadlineObj);
                StorageService.SaveDeadlines(deadlineInfos.ToList());
            }
            
            await Shell.Current.Navigation.PopAsync();
        }

        //[RelayCommand]
        //void ShowBottomDeadlineDetailsSheet(DeadlineInfo info) {
        //DeadlineObj = info;
        //IsBottomSheetPresented = true;
        //}

    }
}
