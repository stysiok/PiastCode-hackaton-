using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piast_Code_App.StysiokCode;

namespace Piast_Code_App.AdamCode
{
    class Ride
    {
        private string busStopFrom;
        private string busStopTo;
        private string date, time;
        private string change;

        public Ride(string from, string to, string date, string time, string change)
        {
            busStopFrom = from;
            busStopTo = to;
            this.date = date;
            this.time = time;
            this.change = change;
        }
        public string BusStopFrom { get; set; }
        public string BusStopTo { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Change { get; set; }
    }
}
