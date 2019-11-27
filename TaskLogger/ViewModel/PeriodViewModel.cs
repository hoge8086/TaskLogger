using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskLogger.ViewModel
{
    public class PeriodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public enum PeriodType
    {
        [Description("全期間")]
        WholePeriod,
        [Description("指定期間")]
        PartialPeriod,
        [Description("日付")]
        DatePeriod
    }
    public class WholePeriodViewModel : PeriodViewModel { }
    public class PartialPeriodViewModel : PeriodViewModel
    {
        public PartialPeriodViewModel()
        {
            End = DateTime.Today;
            Start = DateTime.Today.AddDays(-7);
        }
        private DateTime _Start;
        public DateTime Start
        {
            get { return _Start; }
            set
            {
                if (value == _Start)
                    return;
                _Start = value;
                RaisePropertyChanged(nameof(Start));
            }
        }
        private DateTime _End;
        public DateTime End
        {
            get { return _End; }
            set
            {
                if (value == _End)
                    return;
                _End = value;
                RaisePropertyChanged(nameof(End));
            }
        }

    }
    public class DatePeriodViewModel : PeriodViewModel
    {
        public DatePeriodViewModel()
        {
            Date = DateTime.Today;
        }
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (value == _Date)
                    return;
                _Date = value;
                RaisePropertyChanged(nameof(Date));
            }
        }
    }


}
