namespace RoverApp.Models
{
    public class Point
    {
        /// <summary>
        /// The x coordinate of a point in the grid
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// The y coordinate of a point in the grid
        /// </summary>
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}