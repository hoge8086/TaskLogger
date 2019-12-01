using System;
using System.Xml.Serialization;

namespace TaskLogger.Business.Domain.Model
{
    [XmlInclude(typeof(WholePeriod))]
    [XmlInclude(typeof(PartialPeriod))]
    [XmlInclude(typeof(DatePeriod))]
    public abstract class Period {
        public abstract bool IsIn(DateTime? dateTime);
    }

    public class WholePeriod : Period {
        public override bool IsIn(DateTime? dateTime) { return true; }
        public WholePeriod() { }
    }
    
    public class PartialPeriod : Period {
        private DateTime startDay;
        private DateTime endDay;
        public PartialPeriod() { }
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
        public override bool IsIn(DateTime? dateTime)
        {
            if (dateTime == null)
                return false;

            return startDay <= dateTime && dateTime < endDay.AddDays(1);
        }
    }

    public class DatePeriod : Period {
        public DateTime Date { get; set; }
        public DatePeriod(DateTime date) { this.Date = date; }
        public DatePeriod() { }
        public override bool IsIn(DateTime? dateTime)
        {
            if (dateTime == null)
                return false;

            return Date.Year==dateTime.Value.Year && Date.Month==dateTime.Value.Month && Date.Day==dateTime.Value.Day;
        }
    }
}
