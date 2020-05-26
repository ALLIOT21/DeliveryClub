using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.Logic
{
    public class StringTimeSpanConverter
    {
        public string TimeSpanToString(TimeSpan? timeSpan)
        {
            if (timeSpan.HasValue)
            {
                string hoursStr = timeSpan.Value.Hours < 10 ? "0" + timeSpan.Value.Hours : timeSpan.Value.Hours.ToString();
                string minutesStr = timeSpan.Value.Minutes < 10 ? "0" + timeSpan.Value.Minutes : timeSpan.Value.Minutes.ToString();
                return hoursStr + ":" + minutesStr;
            }
            else
                return "";
        }

        public TimeSpan? StringToTimeSpan(string time)
        {
            if (time != null)
            {
                int hours = Int32.Parse(time.Substring(0, 2));
                int minutes = Int32.Parse(time.Substring(3, 2));
                return new TimeSpan(hours, minutes, 0);
            }
            else
                return null;
        }
    }
}
