using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piast_Code_App.StysiokCode
{
    abstract class SharedData
    {
        private string name;
        private double xCoordinate;
        private double yCoordinate;
        public string Name { get; set; }
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
    }
    class BusStop : SharedData
    {

    }
    class Relic : SharedData
    {
        public Relic() { }

        public Relic(string name, double xCoordinate, double yCoordinate)
        {
            this.Name = name;
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
        }
    }
}
