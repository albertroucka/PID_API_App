using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PID_API_App.Models
{
    class Vehicle
    {
        public string VehicleType { get; set; }
        public string LineNumber { get; set; }
        public int LineNumInt { get; set; }
        public int Sequence { get; set; }
        public int ActualDelay { get; set; }
        public string AgencyName { get; set; }

        public Vehicle(string description_cs, string route_short_name, string origin_route_name, int sequence_id, int actual, string real)
        {
            VehicleType = description_cs;
            LineNumber = route_short_name;
            LineNumInt = Convert.ToInt32(origin_route_name);
            Sequence = sequence_id;
            ActualDelay = actual;
            AgencyName = real;
        }
    }

    class AverageLine
    {
        public string VehicleType { get; set; }
        public Color VehicleColor { get; set; }
        public string LineNumber { get; set; }
        public string AgencyName { get; set; }
        public int NumberOfVehicles { get; set; }
        public string AverageDelay { get; set; }
        public Color DelayColor { get; set; }

        public AverageLine(string vehicleType, Color vehicleColor, string lineNumber, string agencyName, int numberOfVehicles, string averageDelay, Color delayColor)
        {
            VehicleType = vehicleType;
            VehicleColor = vehicleColor;
            LineNumber = lineNumber;
            AgencyName = agencyName;
            NumberOfVehicles = numberOfVehicles;
            AverageDelay = averageDelay;
            DelayColor = delayColor;
        }
    }
}
