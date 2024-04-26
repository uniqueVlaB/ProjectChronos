using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using ProjectChronos.Models.Cist.Groups;
using ProjectChronos.Services;
using ProjectChronos.Views.Popups;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace ProjectChronos.ViewModels.Popups
{
    public partial class GroupSelectionPopupViewModel : BaseViewModel
    {
       public ObservableCollection<Group> Result { get; } = new();

        List<Group> _groups;
        CistService cistService;

        public GroupSelectionPopupViewModel(CistService cistService) {
        this.cistService = cistService;
        }

        [RelayCommand]
        async Task SearchAsync(string query)
        {
            if (IsBusy) return;

            try
            {
                if (_groups is null)
                {
                    _groups = await cistService.GetAllGroupsAsync();
                }

                List<Group> groupsResult = new();

                groupsResult = _groups.Where(x => x.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

                if (Result.Count != 0)
                    Result.Clear();

                foreach (var group in groupsResult)
                    Result.Add(group);
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;

            } 
        }
        [RelayCommand]
        public async Task OnGroupTappedAsync(Group group) {
            Preferences.Default.Set("GroupId", group.Id.ToString());
            Preferences.Default.Set("GroupName", group.Name);
            
            await Shell.Current.DisplayAlert("Group Selected!", $"Selected group: {group.Name}", "OK");
            await MopupService.Instance.PopAsync();
        }
    }

}
