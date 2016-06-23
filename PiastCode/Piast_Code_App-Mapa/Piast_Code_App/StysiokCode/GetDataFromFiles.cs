using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piast_Code_App.StysiokCode
{
    class GetDataFromFiles
    {
        public static List<Relic> GetAllRelics(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new Exception("Nie mogę znaleźć pliku '" + fileName + "'.");
            }

            return File.ReadAllLines(fileName)
                .Select(line =>
                {
                    var d = line.Split('|');
                    return new Relic
                    {
                        Name = d[0],
                        XCoordinate = Convert.ToDouble(d[1]),
                        YCoordinate = Convert.ToDouble(d[2])
                    };
                })
                .ToList();
        }
        public static List<BusStop> GetAllBusStops(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new Exception("Nie mogę znaleźć pliku '" + fileName + "'.");
            }

            return File.ReadAllLines(fileName)
                .Select(line =>
                {
                    var d = line.Split('|');
                    return new BusStop
                    {
                        Name = d[0],
                        XCoordinate = Convert.ToDouble(d[1]),
                        YCoordinate = Convert.ToDouble(d[2])
                    };
                })
                .ToList();
        }

    }
}
