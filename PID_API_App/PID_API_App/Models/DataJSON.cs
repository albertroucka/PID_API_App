using System;
using System.Collections.Generic;
using System.Text;

namespace PID_API_App.Models
{
    public class AgencyName
    {
        public string real { get; set; }
        public string scheduled { get; set; }
    }

    public class AllPositions
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }

    public class Cis
    {
        public string line_id { get; set; }
        public int trip_number { get; set; }
    }

    public class Delay
    {
        public int actual { get; set; }
        public int last_stop_arrival { get; set; }
        public int last_stop_departure { get; set; }
    }

    public class Feature
    {
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
        public string type { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Gtfs
    {
        public string route_id { get; set; }
        public string route_short_name { get; set; }
        public int route_type { get; set; }
        public string trip_id { get; set; }
        public string trip_headsign { get; set; }
    }

    public class LastPosition
    {
        public int bearing { get; set; }
        public Delay delay { get; set; }
        public LastStop last_stop { get; set; }
        public NextStop next_stop { get; set; }
        public bool is_canceled { get; set; }
        public DateTime origin_timestamp { get; set; }
        public int speed { get; set; }
        public string shape_dist_traveled { get; set; }
        public bool tracking { get; set; }
    }

    public class LastStop
    {
        public string id { get; set; }
        public int sequence { get; set; }
        public DateTime arrival_time { get; set; }
        public DateTime departure_time { get; set; }
    }

    public class NextStop
    {
        public string id { get; set; }
        public int sequence { get; set; }
        public DateTime arrival_time { get; set; }
        public DateTime departure_time { get; set; }
    }

    public class Properties
    {
        public Trip trip { get; set; }
        public LastPosition last_position { get; set; }
        public AllPositions all_positions { get; set; }
        public int bearing { get; set; }
        public Delay delay { get; set; }
        public LastStop last_stop { get; set; }
        public NextStop next_stop { get; set; }
        public bool is_canceled { get; set; }
        public DateTime origin_timestamp { get; set; }
        public int speed { get; set; }
        public string shape_dist_traveled { get; set; }
        public bool tracking { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }

    public class Trip
    {
        public AgencyName agency_name { get; set; }
        public Cis cis { get; set; }
        public int sequence_id { get; set; }
        public string origin_route_name { get; set; }
        public Gtfs gtfs { get; set; }
        public DateTime start_timestamp { get; set; }
        public VehicleType vehicle_type { get; set; }
        public int vehicle_registration_number { get; set; }
        public bool wheelchair_accessible { get; set; }
        public bool air_conditioned { get; set; }
    }

    public class VehicleType
    {
        public int id { get; set; }
        public string description_cs { get; set; }
        public string description_en { get; set; }
    }
}
