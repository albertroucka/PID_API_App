using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PID_API_App.Models
{
    class ResourceData
    {
        public static List<Vehicle> buses = new List<Vehicle>();
        public static List<Vehicle> trams = new List<Vehicle>();
        public static List<Vehicle> regbuses = new List<Vehicle>();
        public static List<Vehicle> trains = new List<Vehicle>();
        public static List<Vehicle> boats = new List<Vehicle>();
        public Root response = null; public static DateTime lastUpdate;

        public bool MainMethod()
        {
            try
            {
                string responseData = DownloadData().Result;

                var settings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                };

                Root responseList = JsonConvert.DeserializeObject<Root>(responseData, settings);
                this.response = responseList;
            }
            catch (Exception e)
            {
                return true;
            }

            lastUpdate = DateTime.Now;
            return false;
        }

        public async Task<string> DownloadData()
        {
            //Stažení dat pomocí REST API
            var baseAddress = new Uri("https://api.golemio.cz/v2/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                string YOUR_ACCESS_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFsYmVydHJvdWNrYUBnbWFpbC5jb20iLCJpZCI6MTQ1NCwibmFtZSI6bnVsbCwic3VybmFtZSI6bnVsbCwiaWF0IjoxNjY0NDQwMjI4LCJleHAiOjExNjY0NDQwMjI4LCJpc3MiOiJnb2xlbWlvIiwianRpIjoiNjhiODIzMDEtMDU2Mi00OTIwLWI0OWEtZmE1MGQ3ODMwYWNkIn0.E52ThxAT2AMV8J-ikYPbc-EJa8uPDMFJ5De-hzXlkyk";
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-access-token", YOUR_ACCESS_TOKEN);

                using (var response = await httpClient.GetAsync("vehiclepositions").ConfigureAwait(false))
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    return responseData;
                }               
            }
        }

        public void DeseliazeJSON()
        {
            int i = 0;
            Root responseList = this.response;
            buses.Clear(); trams.Clear(); regbuses.Clear(); trains.Clear(); boats.Clear();

            if (responseList.features != null)
            {
                foreach (var item in responseList.features)
                {
                    try
                    {
                        switch (item.properties.trip.vehicle_type.description_cs)
                        {
                            case "autobus":
                                Vehicle vehicleBus = new Vehicle(item.properties.trip.vehicle_type.description_cs, item.properties.trip.gtfs.route_short_name, item.properties.trip.origin_route_name, item.properties.trip.sequence_id, item.properties.last_position.delay.actual, item.properties.trip.agency_name.real);
                                buses.Add(vehicleBus); break;
                            case "tramvaj":
                                Vehicle vehicleTram = new Vehicle(item.properties.trip.vehicle_type.description_cs, item.properties.trip.gtfs.route_short_name, item.properties.trip.origin_route_name, item.properties.trip.sequence_id, item.properties.last_position.delay.actual, item.properties.trip.agency_name.real);
                                trams.Add(vehicleTram); break;
                            case "regionální autobus":
                                Vehicle vehicleRegBus = new Vehicle(item.properties.trip.vehicle_type.description_cs, item.properties.trip.gtfs.route_short_name, item.properties.trip.origin_route_name, item.properties.trip.sequence_id, item.properties.last_position.delay.actual, item.properties.trip.agency_name.real);
                                regbuses.Add(vehicleRegBus); break;
                            case "náhradní doprava":
                                Vehicle vehicleReplacementBus = new Vehicle(item.properties.trip.vehicle_type.description_cs, item.properties.trip.gtfs.route_short_name, item.properties.trip.origin_route_name, item.properties.trip.sequence_id, item.properties.last_position.delay.actual, item.properties.trip.agency_name.real);
                                buses.Add(vehicleReplacementBus); break;
                            case "loď":
                                Vehicle vehicleBoat = new Vehicle(item.properties.trip.vehicle_type.description_cs, item.properties.trip.gtfs.route_short_name, item.properties.trip.origin_route_name, item.properties.trip.sequence_id, item.properties.last_position.delay.actual, item.properties.trip.agency_name.real);
                                boats.Add(vehicleBoat); break;
                            case "noční autobus":
                                Vehicle vehicleNightBus = new Vehicle(item.properties.trip.vehicle_type.description_cs, item.properties.trip.gtfs.route_short_name, item.properties.trip.origin_route_name, item.properties.trip.sequence_id, item.properties.last_position.delay.actual, item.properties.trip.agency_name.real);
                                buses.Add(vehicleNightBus); break;
                            case "noční tramvaj":
                                Vehicle vehicleNightTram = new Vehicle(item.properties.trip.vehicle_type.description_cs, item.properties.trip.gtfs.route_short_name, item.properties.trip.origin_route_name, item.properties.trip.sequence_id, item.properties.last_position.delay.actual, item.properties.trip.agency_name.real);
                                trams.Add(vehicleNightTram); break;
                            case "noční regionální autobus":
                                Vehicle vehicleNightRegBus = new Vehicle(item.properties.trip.vehicle_type.description_cs, item.properties.trip.gtfs.route_short_name, item.properties.trip.origin_route_name, item.properties.trip.sequence_id, item.properties.last_position.delay.actual, item.properties.trip.agency_name.real);
                                regbuses.Add(vehicleNightRegBus); break;
                            default:
                                i++; break;
                        }
                    }
                    catch (Exception)
                    {
                        Vehicle vehicleTrain = new Vehicle("vlak", item.properties.trip.gtfs.route_short_name, item.properties.trip.origin_route_name, item.properties.trip.sequence_id, item.properties.last_position.delay.actual, item.properties.trip.agency_name.scheduled);
                        trains.Add(vehicleTrain);                       
                    }
                }               
            }
            else
            {
                //nemůže nastat
            }
        }

        public List<AverageLine> SortBuses()
        {
            List<Vehicle> sorted = buses.OrderBy(Vehicle => Vehicle.LineNumber).ToList();
            return DivideLines(sorted);
        }

        public List<AverageLine> SortTrams()
        {
            List<Vehicle> sorted = trams.OrderBy(Vehicle => Vehicle.LineNumber).ToList();
            return DivideLines(sorted);
        }

        public List<AverageLine> SortRegBuses()
        {
            List<Vehicle> sorted = regbuses.OrderBy(Vehicle => Vehicle.LineNumber).ToList();
            return DivideLines(sorted);
        }

        public List<AverageLine> SortTrains()
        {
            List<Vehicle> sorted = trains.OrderBy(Vehicle => Vehicle.LineNumber).ToList();
            return DivideLines(sorted);
        }

        public List<AverageLine> SortBoats()
        {
            List<Vehicle> sorted = boats.OrderBy(Vehicle => Vehicle.LineNumber).ToList();
            return DivideLines(sorted);
        }

        public List<AverageLine> DivideLines(List<Vehicle> VehicleType)
        {
            int lineNum = 0; //VehicleType[0].LineNumInt;
            int overall = 999; int typeCount = 0; bool ae = false;
            List<Vehicle> vehicles = new List<Vehicle>();
            List<AverageLine> result = new List<AverageLine>();

            while (lineNum < overall)
            {
                while (typeCount < 6)
                {
                    if (ae == false)
                    {
                        foreach (var item in VehicleType)
                        {
                            if ("AE" == item.LineNumber)
                            {
                                vehicles.Add(item);
                            }
                        }
                        ae = true;
                    }

                    if (typeCount == 0)
                    {
                        foreach (var item in VehicleType)
                        {
                            if (lineNum.ToString() == item.LineNumber)
                            {
                                vehicles.Add(item);
                            }
                        }
                    }
                    else if (typeCount == 1)
                    {
                        foreach (var item in VehicleType)
                        {
                            string s = "L" + lineNum.ToString();
                            if (s == item.LineNumber)
                            {
                                vehicles.Add(item);
                            }
                        }
                    }
                    else if (typeCount == 2)
                    {
                        foreach (var item in VehicleType)
                        {
                            string s = "P" + lineNum.ToString();
                            if (s == item.LineNumber)
                            {
                                vehicles.Add(item);
                            }
                        }
                    }
                    else if (typeCount == 3)
                    {
                        foreach (var item in VehicleType)
                        {
                            string s = "R" + lineNum.ToString();
                            if (s == item.LineNumber)
                            {
                                vehicles.Add(item);
                            }
                        }
                    }
                    else if (typeCount == 4)
                    {
                        foreach (var item in VehicleType)
                        {
                            string s = "S" + lineNum.ToString();
                            if (s == item.LineNumber)
                            {
                                vehicles.Add(item);
                            }
                        }
                    }
                    else if (typeCount == 5)
                    {
                        foreach (var item in VehicleType)
                        {
                            string s = "X" + lineNum.ToString();
                            if (s == item.LineNumber)
                            {
                                vehicles.Add(item);
                            }
                        }
                    }

                    if (vehicles.Count > 0)
                    {
                        int delay = 0;
                        foreach (var item in vehicles)
                        {
                            delay = delay + item.ActualDelay;
                        }

                        int avgDelay = delay / vehicles.Count;
                        int i = avgDelay / 60;
                        string averageDelay; Color delayColor;

                        if (i < 0.5)
                        {
                            averageDelay = "včas";
                            delayColor = Color.FromHex("#5ca95e");
                        }
                        else if (i < 6)
                        {
                            averageDelay = $"+{i} min";
                            delayColor = Color.FromHex("#5ca95e");
                        }
                        else if (i < 11)
                        {
                            averageDelay = $"+{i} min";
                            delayColor = Color.FromHex("#f1a22f");
                        }
                        else if (true)
                        {
                            averageDelay = $"+{i} min";
                            delayColor = Color.FromHex("#d64e45");
                        }

                        Color vehicleColor = VehicleTypeColor(vehicles[0].VehicleType);

                        AverageLine avline = new AverageLine(vehicles[0].VehicleType, vehicleColor, vehicles[0].LineNumber, vehicles[0].AgencyName, vehicles.Count, averageDelay, delayColor);
                        result.Add(avline);
                        vehicles.Clear();
                    }

                    typeCount++;
                }

                typeCount = 0;
                lineNum++;
            }

            return result;
        }

        private Color VehicleTypeColor(string vehicleType)
        {

            if (vehicleType == "autobus" || vehicleType == "regionální autobus" || vehicleType == "noční autobus" || vehicleType == "noční regionální autobus" || vehicleType == "náhradní doprava")
            {
                Color color = Color.FromHex("#047aa9"); 
                return color;
            }
            else if (vehicleType == "tramvaj" || vehicleType == "noční tramvaj")
            {
                Color color = Color.FromHex("#7a0404");
                return color;
            }
            else if (vehicleType == "vlak")
            {
                Color color = Color.FromHex("#313867");
                return color;
            }
            else if (vehicleType == "loď")
            {
                Color color = Color.FromHex("#00b3cb");
                return color;
            }
            else
            {
                return Color.Black;
            }

        }

        public string GetUpdateTime()
        {
            return lastUpdate.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
