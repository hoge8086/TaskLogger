using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskLogger.Business.Domain.Model;

namespace TaskLogger.ViewModel
{
    public abstract class PeriodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public abstract Period Create();
        public static PeriodViewModel Create(Period period)
        {
            if (period is DatePeriod)
            {
                return new DatePeriodViewModel() { Date = ((DatePeriod)period).Date };
            }
            if (period is WholePeriod)
            {
                return new WholePeriodViewModel();
            }

            if (period is PartialPeriod)
            {
                return new PartialPeriodViewModel()
                {
                    Start = ((PartialPeriod)period ).StartDay,
                    End = ((PartialPeriod)period ).EndDay
                };
            }

            return new WholePeriodViewModel();
        }
    }

    [Description("全期間")]
    public class WholePeriodViewModel : PeriodViewModel
    {
        public override Period Create()
        {
            return new WholePeriod();
        }
    }

    [Description("指定期間")]
    public class PartialPeriodViewModel : PeriodViewModel
    {
        public PartialPeriodViewModel()
        {
            _End = DateTime.Today;
            _Start = DateTime.Today.AddDays(-7);
        }
        private DateTime _Start;
        public DateTime Start
        {
            get { return _Start; }
            set
            {
                if (value == _Start)
                    return;
                _End = value.AddDays((_End - _Start).Days);
                _Start = value;
                RaisePropertyChanged(nameof(Start));
                RaisePropertyChanged(nameof(End));
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
        public override Period Create()
        {
            return new PartialPeriod() { StartDay = Start.Date, EndDay = End.Date };
        }

    }

    [Description("日付")]
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
        public override Period Create()
        {
            return new DatePeriod() { Date = Date };
        }
    }


}
