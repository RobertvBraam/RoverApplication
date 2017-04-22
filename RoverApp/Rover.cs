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
        /// <param name="fullPath"></param>
        /// <param name="startingLocation"></param>
        /// <returns></returns>
        private string PathDirections(IEnumerable<Point> fullPath, Point startingLocation)
        {
            var startPoint = startingLocation;
            var cardinalDirections = new StringBuilder();

            foreach (var endPoint in fullPath)
            {
                var xDifference = endPoint.X - startPoint.X;
                var yDifference = endPoint.Y - startPoint.Y;

                var cardinalDirection = CalculateCardinalDirection(xDifference, yDifference);

                cardinalDirections.Append(cardinalDirection + "-");

                startPoint = endPoint;
            }

            return cardinalDirections.ToString();
        }

        /// <summary>
        /// Calculate what cardinal direction based on coordinates
        /// </summary>
        /// <param name="xDifference"></param>
        /// <param name="yDifference"></param>
        /// <returns></returns>
        private char CalculateCardinalDirection(int xDifference, int yDifference)
        {
            if (xDifference == 0)
            {
                if (yDifference == 1)
                {
                    return 'N';
                }

                return 'S';
            }
            else
            {
                if (xDifference == 1)
                {
                    return 'E';
                }

                return 'W';
            }
        }
    }
}
