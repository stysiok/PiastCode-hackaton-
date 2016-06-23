using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piast_Code_App.StysiokCode
{
    class Calculations
    {
        public static BusStop NearestBusStop(Relic relic, List<BusStop> busStops)
        {
            BusStop nearestBusStop = new BusStop();
            double xPlus = relic.XCoordinate + 0.001;
            double yPlus = relic.YCoordinate + 0.001;
            double range = Math.Sqrt(((Math.Pow((xPlus - relic.XCoordinate), 2) + Math.Pow((yPlus - relic.YCoordinate), 2))));

            while (nearestBusStop.Name == null)
            {
                foreach (BusStop busStop in busStops)
                {
                    double stopRange =
                        Math.Sqrt(((Math.Pow((busStop.XCoordinate - relic.XCoordinate), 2) +
                                    Math.Pow((busStop.YCoordinate - relic.YCoordinate), 2))));
                    if (stopRange < range)
                    {
                        nearestBusStop = busStop;
                    }
                }
                xPlus = xPlus + 0.001;
                yPlus = yPlus + 0.001;
                range =
                    Math.Sqrt(((Math.Pow((xPlus - relic.XCoordinate), 2)) + Math.Pow((yPlus - relic.YCoordinate), 2)));
            }
            return nearestBusStop;
        }

        public static BusStop NearestBusStop(double xCoordinate, double yCoordinate, List<BusStop> busStops)
        {
            BusStop nearestBusStop = new BusStop();

            double xPlus = xCoordinate + 0.001;
            double yPlus = yCoordinate + 0.001;
            double range = Math.Sqrt(((Math.Pow((xPlus - xCoordinate), 2) + Math.Pow((yPlus - yCoordinate), 2))));

            while (nearestBusStop.Name == null)
            {
                foreach (BusStop busStop in busStops)
                {
                    double newrange = Math.Sqrt(((Math.Pow((xCoordinate - busStop.XCoordinate), 2) + Math.Pow((yCoordinate - busStop.YCoordinate), 2))));
                    if (newrange < range)
                    {
                        nearestBusStop = busStop;
                    }
                }
                xPlus = xPlus + 0.001;
                yPlus = yPlus + 0.001;
                range = Math.Sqrt(((Math.Pow((xPlus - xCoordinate), 2) + Math.Pow((yPlus - yCoordinate), 2))));
            }
            return nearestBusStop;
        }

        public static BusStop FasttestTrip(List<BusStop> busStops)
        {
            BusStop busStop = new BusStop();
            

            return busStop;
        }
    }
}
