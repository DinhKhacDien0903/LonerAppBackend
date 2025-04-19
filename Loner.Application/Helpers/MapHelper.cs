namespace Loner.Application.Helpers
{
    public static class MapHelper
    {
        private const double meanEarthRadiusInKilometers = 6371.0;
        public static double GetDistance(
            double latitudeStart,
            double longitudeStart,
            double latitudeEnd,
            double longitudeEnd)
        {
            return Math.Round(CalculateDistance(latitudeStart, longitudeStart, latitudeEnd, longitudeEnd), 2);
        }

        public static double CalculateDistance(
            double latitudeStart,
            double longitudeStart,
            double latitudeEnd,
            double longitudeEnd)
        {
            return CoordinatesToKilometers(latitudeStart, longitudeStart, latitudeEnd, longitudeEnd);
        }

        private static double CoordinatesToKilometers(double lat1, double lon1, double lat2, double lon2)
        {
            if (lat1 == lat2 && lon1 == lon2)
                return 0;

            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            lat1 = DegreesToRadians(lat1);
            lat2 = DegreesToRadians(lat2);

            var dLat2 = Math.Sin(dLat / 2) * Math.Sin(dLat / 2);
            var dLon2 = Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var a = dLat2 + dLon2 * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Asin(Math.Sqrt(a));

            return meanEarthRadiusInKilometers * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}