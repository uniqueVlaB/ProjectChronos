using Newtonsoft.Json;
using ProjectChronos.Model.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ProjectChronos.Model.Cist.Events;
using ProjectChronos.Model.Cist.Groups;

namespace ProjectChronos.Services
{

    public class CistService
    {
        HttpClient httpClient;

        public CistService()
        {
            this.httpClient = new HttpClient();
        }

        public async Task<Timetable> GetTimetableWithOffsetAsync(int days)
        {
            if (!Preferences.Default.ContainsKey("GroupId")) 
            {
                await Shell.Current.DisplayAlert("Error!", "Select your group in menu first!", "OK");
                return new Timetable();
            }
            Uri u = new($"https://cist.nure.ua/ias/app/tt/P_API_EVEN_JSON" +
                $"?type_id=1" +
                $"&timetable_id={Preferences.Default.Get("GroupId", "")}" +
                $"&time_from={new DateTimeOffset(DateTime.Now.AddDays(-1)).ToUnixTimeSeconds()}" +
                $"&time_to={new DateTimeOffset(DateTime.Now).AddDays(days-1).ToUnixTimeSeconds()}" +
                $"&idClient={Secrets.IdClient}");

            var timetable = new Timetable();
            var response = await httpClient.GetAsync(u);
            if (response.IsSuccessStatusCode)
            {
                var bytes1251 = await response.Content.ReadAsByteArrayAsync();

                var bytesUtf16 = Encoding.Convert(CodePagesEncodingProvider.Instance.GetEncoding(1251), Encoding.Unicode, bytes1251);
                var jsonStr = Encoding.Unicode.GetString(bytesUtf16);
                if (!jsonStr.Contains("\"events\":[\n]}]")) timetable = JsonConvert.DeserializeObject<Timetable>(jsonStr); 
            }
            for (int i = 0; i < timetable.Events.Count; i++) 
            {
                timetable.Events[i].StartTime = timetable.Events[i].StartTime.ToLocalTime();
                timetable.Events[i].EndTime = timetable.Events[i].EndTime.ToLocalTime();
            }
            return timetable;
        }
        public async Task<List<Model.Cist.Groups.Group>> GetAllGroupsAsync()
        {
            Uri u = new($"https://cist.nure.ua/ias/app/tt/P_API_GROUP_JSON");
            var university = new University();
            var response = await httpClient.GetAsync(u);
            if (response.IsSuccessStatusCode)
            {
                var bytes1251 = await response.Content.ReadAsByteArrayAsync();

                var bytesUtf16 = Encoding.Convert(CodePagesEncodingProvider.Instance.GetEncoding(1251), Encoding.Unicode, bytes1251);
                var jsonStr = Encoding.Unicode.GetString(bytesUtf16);
              
                university = JsonConvert.DeserializeObject<UniversityRootObject>(jsonStr).University;
            }
            var groups = new List<Model.Cist.Groups.Group>();
            for (int a = 0; a < university.Faculties.Count; a++) {
                for (int b = 0; b < university.Faculties[a].Directions.Count; b++) {
                    groups.AddRange(university.Faculties[a].Directions[b].Groups);
                }
            }
            return groups;
        }
    }

}
