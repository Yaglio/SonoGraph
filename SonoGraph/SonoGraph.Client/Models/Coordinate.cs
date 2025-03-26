namespace SonoGraph.Client.Models
{
    /// <summary>
    /// Represents a coordinate with x and y values
    /// </summary>
    public struct Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public static class CoordinateUtils
    {
        /// <summary>
        /// Returns the speed in pixels per second
        /// </summary>
        /// <param name="previousCoordinate"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static double CalculateSpeed(Coordinate previousCoordinate, Coordinate currentCoordinate, TimeSpan timeSpan)
        {
            return CalculateDistance(previousCoordinate, currentCoordinate) / timeSpan.TotalMilliseconds * 1000;
        }

        /// <summary>
        /// Returns the distance between two coordinates
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static double CalculateDistance(Coordinate startCoordinate, Coordinate endCoordinate)
        {
            return Math.Sqrt(Math.Pow(endCoordinate.X - startCoordinate.X, 2) + Math.Pow(endCoordinate.Y - startCoordinate.Y, 2));
        }
    }
}
