using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Design;
using System.Drawing;
using Microsoft.Win32;
using Piast_Code_App.AdamCode;
using Piast_Code_App.StysiokCode;
//using System.Windows.Forms;


namespace Piast_Code_App
{
  
    public partial class MainWindow : Window
    {
        //LocationConverter locConverter = new LocationConverter();
        public MainWindow()
        {
            InitializeComponent();
            myMap.Focus();
            ReadRelics();
            ReadBusStops();
            foreach (Relic relic in _allReclis)
            {
                comboBox.Items.Add(relic.Name);
                //PinPointForRelic(relic);
            }
            

        }

        private List<string> allStops = new List<string>();
        private List<Relic> myTrip = new List<Relic>();
       

        private void GetKey(Action<string> callback)
        {
            if (callback != null)
            {
                myMap.CredentialsProvider.GetCredentials((c) =>
                {
                    callback(c.ApplicationId);
                });
            }
        }

        private void Route(List<string> stops, string key, Action<Response> callback)
        {
            string tmp = null;

            for (int i = 2; i < stops.Count(); i++)
            {
                tmp += "&wp." + i.ToString() + "=" + stops[i];
            }

            Uri requestURI = new Uri(string.Format("http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={0}&wp.1={1}{2}&rpo=Points&key={3}", Uri.EscapeDataString(stops[0]), Uri.EscapeDataString(stops[1]), tmp, key));
         
         
            GetResponse(requestURI, callback); 
        }

        private void GetResponse(Uri uri, Action<Response> callback)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));

                        if (callback != null)
                        {
                            callback(ser.ReadObject(stream) as Response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private List<Pushpin> pushPins = new List<Pushpin>();
        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            var mousePosition = e.GetPosition(this);
            mousePosition.X -= myMap.Margin.Left;
            
            //Convert the mouse coordinates to a locatoin on the map
            var pinLocation = myMap.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();

            pin.Location = pinLocation;
            pushPins.Add(pin);
            Relic myStart = new Relic("Start", pin.Location.Latitude, pin.Location.Longitude);
            myTrip.Add(myStart);
            //comboBox1.Items.Add(myStart.Name);

            // Adds the pushpin to the map.
            myMap.Children.Add(pin);


        }

        //        private void GetRoute(string fromloc, string toloc)
        private void GetRoute(List<string>stops)
        {
            string to =stops[0];
            string from = stops[1];

            if (!string.IsNullOrWhiteSpace(from))
            {
                if (!string.IsNullOrWhiteSpace(to))
                {
                    GetKey((c) =>
                    {
                        Route(stops, c, (r) =>
                        {
                            if (r != null &&
                                r.ResourceSets != null &&
                                r.ResourceSets.Length > 0 &&
                                r.ResourceSets[0].Resources != null &&
                                r.ResourceSets[0].Resources.Length > 0)
                            {
                                Route route = r.ResourceSets[0].Resources[0] as Route;

                                double[][] routePath = route.RoutePath.Line.Coordinates;
                                LocationCollection locs = new LocationCollection();

                                for (int i = 0; i < routePath.Length; i++)
                                {
                                    if (routePath[i].Length >= 2)
                                    {
                                        locs.Add(new Microsoft.Maps.MapControl.WPF.Location(routePath[i][0], routePath[i][1]));
                                    }
                                }

                                MapPolyline routeLine = new MapPolyline()
                                {
                                    Locations = locs,
                                    Stroke = new SolidColorBrush(Colors.Red),
                                    StrokeThickness = 5
                                };

                                myMap.Children.Add(routeLine);

                                myMap.SetView(locs, new Thickness(30), 0);
                            }
                            else
                            {
                                MessageBox.Show("Brak Wyników");
                            }
                        });
                    });
                }
                else
                {
                    MessageBox.Show("Zły Koniec Podróży");
                }
            }
            else
            {
                MessageBox.Show("Zły Początek Podróży");
            }
        }

        

   

        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    allStops.Add(fromTxb.Text);
        //    allStops.Add(toTxb.Text);
        //    for (int i = 0; i < temp; i++)
        //    {
        //        allStops.Add(additionalBoxes[i].Text);
        //    }
        //    BusStop nearestBusStop = new BusStop();
            
        //    nearestBusStop = Calculations.NearestBusStop(pushPins[0].Location.Latitude, pushPins[0].Location.Altitude, _allBusStops);
        //    label.Content = nearestBusStop.Name;
        //    //GetRoute(allStops);
        //}

        private void zooom_out_Click(object sender, RoutedEventArgs e)
        {
            myMap.ZoomLevel -= 1;
        }

        private void zoom_in_Click(object sender, RoutedEventArgs e)
        {
            myMap.ZoomLevel += 1;
        }

        private List<TextBox> additionalBoxes = new List<TextBox>();
        int temp = 0;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //dodajemy kolejne textboxy
            
            TextBox txtNumber = new TextBox();
            txtNumber.Name = "stopTextBox"+temp;
            StackPanel.Children.Add(txtNumber);
            additionalBoxes.Add(txtNumber);
            temp++;
            
        }

        List<Relic> _allReclis = new List<Relic>();

        private void ReadRelics()
        {
            var fileName = (Directory.GetCurrentDirectory() + "\\zabytki.txt");
            try
            {
                _allReclis = GetDataFromFiles.GetAllRelics(fileName);
            }
            catch (Exception ex)
            {
                
            }
        }
        

        List<BusStop> _allBusStops = new List<BusStop>();
        private void ReadBusStops()
        {
            var fileName = (Directory.GetCurrentDirectory() + "\\przystanki.txt");
            try
            {
                _allBusStops = GetDataFromFiles.GetAllBusStops(fileName);
            }
            catch (Exception ex)
            {
                
            }
        }
        int counter = 0;
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            myTrip.Add(_allReclis[comboBox.SelectedIndex]);
            //comboBox1.Items.Add(myTrip[counter].Name);
            //PinPointForRelic(myTrip[counter].XCoordinate, myTrip[counter].YCoordinate);
            counter++;
        }

        List<Ride> rides = new List<Ride>();
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //BusStop nearestBusStopFrom = new BusStop();
            //BusStop nearestBusStops = new BusStop();
            //nearestBusStopFrom = Calculations.NearestBusStop(pushPins[0].Location.Latitude, pushPins[0].Location.Longitude, _allBusStops);
            //nearestBusStops = Calculations.NearestBusStop(myTrip[0], _allBusStops);

            //Parsers parser = new Parsers();
            

            //parser.getData()
            
            //label.Content = nearestBusStopFrom.Name;

        }

        BusStop fst = new BusStop();

        BusStop scnd = new BusStop();
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PinPointForRelic(_allReclis[comboBox.SelectedIndex]);
            label3.Content = _allReclis[comboBox.SelectedIndex].Name;
            foreach (Relic relic in _allReclis)
            {
                comboBox1.Items.Add(relic.Name);
            }
            fst = Calculations.NearestBusStop(_allReclis[comboBox.SelectedIndex], _allBusStops);
            label.Content = fst.Name;
        }

        private void PinPointForRelic(Relic relic)
        {
            // The pushpin to add to the map
            Pushpin pushpin = new Pushpin();
            MapLayer.SetPosition(pushpin, new Microsoft.Maps.MapControl.WPF.Location(relic.XCoordinate, relic.YCoordinate));
            myMap.Children.Add(pushpin);
            pushPins.Add(pushpin);
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //PinPointForRelic(_allReclis[comboBox1.SelectedIndex]);
            label2.Content = _allReclis[comboBox1.SelectedIndex].Name;
            PinPointForRelic(_allReclis[comboBox1.SelectedIndex]);
            scnd = Calculations.NearestBusStop(_allReclis[comboBox1.SelectedIndex], _allBusStops);
            label4.Content = scnd.Name;
            DataFromMZK();
        }

        private List<string> data;
        private void DataFromMZK()
        {
            Parsers parser = new Parsers();
            string date = DateTime.Today.Day + "." + DateTime.Today.Month + "." + DateTime.Today.Year;
            string time = DateTime.Today.Hour.ToString().PadLeft(1, '0') + ":" +
                          DateTime.Today.Minute.ToString().PadLeft(1, '0');

            List<string> parsedData = parser.getData(fst.Name, scnd.Name, date, time, "3");
            foreach (string str in parsedData)
            {
                data = str.Split(';').ToList();
            }
            labeltr.Content = data[0];
            labeltr1.Content = data[1];
            labeltr2.Content = data[2];
            labeltr3.Content = data[3];
            labeltr4.Content = data[4];
            labeltr5.Content = data[5];
        }
    }
}

