using CommunityToolkit.Maui.Alerts;
using ProjectChronos.Models.App;
using ProjectChronos.Models.Cist.Events;
using System.Xml.Serialization;

namespace ProjectChronos.Services
{
    public class StorageService
    {
        private string _baseDirectory;
        private string _dataStorageDirectory;
        private string _timetablePath;
        private string _deadlinesPath;
        public StorageService() {
        _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _dataStorageDirectory = Path.Combine(_baseDirectory + "/data");
        _timetablePath = Path.Combine(_dataStorageDirectory + "/events.xml");
        _deadlinesPath = Path.Combine(_dataStorageDirectory + "/deadlines.xml");
        }

        public bool SaveTimetable(Timetable timetable) {
            try
            {
                var serializer = new XmlSerializer(typeof(Timetable));
                Directory.CreateDirectory(_dataStorageDirectory);
                using (var writer = new StreamWriter(_timetablePath))
                {
                    serializer.Serialize(writer, timetable);
                }
            }
            catch(Exception ex)
            {
                Shell.Current.DisplaySnackbar(ex.Message);
                return false;
            }
            return true;
        }

        public Timetable GetTimetable() {
            Timetable timetable = new();

            var serializer = new XmlSerializer(typeof(Timetable));

            try
            {
                using (var reader = new StreamReader(_timetablePath))
                {
                    timetable = (Timetable)serializer.Deserialize(reader);
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }

            return timetable;
        }

        public bool SaveDeadlines(List<DeadlineInfo> deadlines) {
            try
            {
                var serializer = new XmlSerializer(typeof(List<DeadlineInfo>));
                Directory.CreateDirectory(_dataStorageDirectory);
                using (var writer = new StreamWriter(_deadlinesPath))
                {
                    serializer.Serialize(writer, deadlines);
                }
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
            return true;

        }

        public List<DeadlineInfo> GetDeadlines() {
            List<DeadlineInfo> deadlines = new();

            var serializer = new XmlSerializer(typeof(List<DeadlineInfo>));

            try
            {
                using (var reader = new StreamReader(_deadlinesPath))
                {
                    deadlines = (List<DeadlineInfo>)serializer.Deserialize(reader);

                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
            catch(Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return null;
            }

            return deadlines;
        }
    }
}
