using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectChronos.Models.App.Const
{
    public class DeadlineFilters : INotifyPropertyChanged
    {
        private bool _priorityLowSelected;
        public bool PriorityLowSelected
        {
            get => _priorityLowSelected;
            set
            {
                if (_priorityLowSelected != value)
                {
                    _priorityLowSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _priorityNormalSelected;
        public bool PriorityNormalSelected
        {
            get => _priorityNormalSelected;
            set
            {
                if (_priorityNormalSelected != value)
                {
                    _priorityNormalSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _priorityHighSelected;
        public bool PriorityHighSelected
        {
            get => _priorityHighSelected;
            set
            {
                if (_priorityHighSelected != value)
                {
                    _priorityHighSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _dateSelected;
        public bool DateSelected
        {
            get => _dateSelected;
            set
            {
                if (_dateSelected != value)
                {
                    _dateSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _fromDate = DateTime.Today;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                if (_fromDate != value)
                {
                    _fromDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _toDate = DateTime.Today;
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                if (_toDate != value)
                {
                    _toDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _onlyCompletedSelected;
        public bool OnlyCompletedSelected
        {
            get => _onlyCompletedSelected;
            set
            {
                if (_onlyCompletedSelected != value)
                {
                    _onlyCompletedSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _onlyUncompletedSelected;
        public bool OnlyUncompletedSelected
        {
            get => _onlyUncompletedSelected;
            set
            {
                if (_onlyUncompletedSelected != value)
                {
                    _onlyUncompletedSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
