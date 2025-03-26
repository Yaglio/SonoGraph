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

        /// <summary>
        /// Sets the coordinate to the given values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void setCoodinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Sets the coordinate to the values of the given coordinate
        /// </summary>
        /// <param name="coordinate"></param>
        public void setCoodinate(Coordinate coordinate)
        {
            X = coordinate.X;
            Y = coordinate.Y;
        }

        /// <summary>
        /// Returns the speed in pixels per second
        /// </summary>
        /// <param name="lastCoordinate"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public double getSpeed(Coordinate lastCoordinate, TimeSpan timeSpan)
        {
            return getDistance(lastCoordinate) / timeSpan.TotalMilliseconds * 1000;
        }

        /// <summary>
        /// Returns the distance between two coordinates
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public double getDistance(Coordinate coordinate)
        {
            return Math.Sqrt(Math.Pow(X - coordinate.X, 2) + Math.Pow(Y - coordinate.Y, 2));
        }


    }
}
