namespace RoverApp.Models
{
    public class SearchParameters
    {
        /// <summary>
        /// The start location of the path
        /// </summary>
        public Point StartLocation { get; set; }
        /// <summary>
        /// The end location of the path
        /// </summary>
        public Point EndLocation { get; set; }
        /// <summary>
        /// The count of nodes in the grid per side
        /// </summary>
        public int GridSize { get; set; }

        public SearchParameters(Point startLocation, Point endLocation, int gridSize)
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
            GridSize = gridSize;
        }
    }
}