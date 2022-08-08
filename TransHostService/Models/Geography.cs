namespace TransHostService.Models
{
    public enum DistanceUnit { Km, Mile }
    public class Geography
    {
        public static double Distance(double lat1, double lon1, double lat2, double lon2, DistanceUnit unit) {
            return Distance(lat1, lon1, lat2, lon2, unit == DistanceUnit.Mile);        
        }
        private static double Distance(double lat1, double lon1, double lat2, double lon2, bool inMiles)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) + Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));
                dist = Math.Acos(dist);
                dist = Rad2deg(dist);
                dist = dist * 60 * 1.1515 * (inMiles ? 1 : 1.609344);                 
                return dist;
            }
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double Deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double Rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }

    public struct GeoPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public static double DistanceBetween(GeoPoint p1, GeoPoint p2, DistanceUnit unit = DistanceUnit.Km)
        {
            return Geography.Distance(p1.Latitude, p1.Longitude, p2.Latitude, p2.Longitude, unit);
        }
    }     
}
