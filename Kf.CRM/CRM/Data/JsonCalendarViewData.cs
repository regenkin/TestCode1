using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KfCrm.CRM.Data
{
    public class JsonCalendarViewData
    {
        public JsonCalendarViewData(List<KfCrm.Model.Personal_Calendar> eventList, DateTime startDate, DateTime endDate)
        {
            events = eventList;
            start = startDate;
            end = endDate;
            issort = true;
        }

        public JsonCalendarViewData(List<KfCrm.Model.Personal_Calendar> eventList, DateTime startDate, DateTime endDate, bool isSort)
        {
            start = startDate;
            end = endDate;
            events = eventList;
            issort = isSort;
        }
        
        public List<KfCrm.Model.Personal_Calendar> events { get; private set; }
        public bool issort
        {
            get;
            private set;
        }

        public DateTime start { get; private set; }
        public DateTime end { get; private set; }
    }
}
