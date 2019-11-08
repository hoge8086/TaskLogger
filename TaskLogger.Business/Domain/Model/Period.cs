using System;

namespace TaskLogger.Business.Domain.Model
{
    public interface Period {
        bool IsIn(DateTime dateTime);
    }
    public class WholePeriod : Period {
        public bool IsIn(DateTime dateTime) { return true; }
    }
    
    public class PartialPeriod : Period {
        private DateTime startDay;
        private DateTime endDay;
        public DateTime StartDay
        {
            set
            {
                startDay = new DateTime(value.Year, value.Month, value.Day);
            }
            get { return startDay; }
        }
        public DateTime EndDay
        {
            set
            {
                endDay = new DateTime(value.Year, value.Month, value.Day);
            }
            get { return endDay; }
        }
        public bool IsIn(DateTime dateTime) { return startDay <= dateTime && dateTime < endDay.AddDays(1); }
    }

    public class OneDayPeriod : Period {
        public DateTime Day { get; set; }
        public bool IsIn(DateTime dateTime) { return Day.Year==dateTime.Year && Day.Month==dateTime.Month && Day.Day==dateTime.Day; }
    }
}
