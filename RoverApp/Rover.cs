using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoverApp.Models;

namespace RoverApp
{
    public class Rover : IRover
    {
        private readonly IPathFinder _pathFinder;
        private IList<SearchParameters> _searchParameters = new List<SearchParameters>();

        public Rover(IPathFinder pathFinder)
        {
            _pathFinder = pathFinder;
        }

        public string StartRover(Point startingLocation, int gridSize, IList<Point> componentLocations)
        {
            InitializeSearchParameters(startingLocation, gridSize, componentLocations);
            var directions = new StringBuilder();
            var beginPath = startingLocation;

            foreach (var searchParameter in _searchParameters)
            {
                var path = _pathFinder.FindPath(searchParameter);

                if (!path.Any())
                {
                    return "No path was found";
                }

                var directionsToComponent = PathDirections(path, beginPath);
                beginPath = searchParameter.EndLocation;

                directions.Append("Component pickup: ");
                directions.Append(directionsToComponent);
                directions.Append("P");
                directions.AppendLine();
            }

            return directions.ToString();
        }

        /// <summary>
        /// Setup of all the nessesary data for the pathfinding algorithm
        /// </summary>
        /// <param name="startingLocation"></param>
        /// <param name="gridSize"></param>
        /// <param name="componentLocations"></param>
        private void InitializeSearchParameters(Point startingLocation, int gridSize, IList<Point> componentLocations)
        {
            var startLocation = startingLocation;

            foreach (var componentLocation in componentLocations)
            {
                var endLocation = new Point(componentLocation.X, componentLocation.Y);
                _searchParameters.Add(new SearchParameters(startLocation, endLocation, gridSize));
                startLocation = endLocation;
            }
        }

        /// <summary>
        /// Calculate what cardinal direction the rover is moving
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="startingLocation"></param>
        /// <returns></returns>
        private static string PathDirections(IEnumerable<Point> paths, Point startingLocation)
        {
            var path = new StringBuilder();
            var startPoint = startingLocation;

            foreach (var endPoint in paths)
            {
                var xDifference = endPoint.X - startPoint.X;
                var yDifference = endPoint.Y - startPoint.Y;

                var xCardinal = xDifference > 0 ? 'E' : 'W';
                var yCardinal = yDifference > 0 ? 'N' : 'S';

                if (xDifference < 0)
                {
                    path.Append(xCardinal, -xDifference);
                }
                else
                {
                    path.Append(xCardinal, xDifference);
                }

                if (yDifference < 0)
                {
                    path.Append(yCardinal, -yDifference);
                }
                else
                {
                    path.Append(yCardinal, yDifference);
                }

                startPoint = endPoint;
            }

            return path.ToString();
        }
    }
}
