using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoverApp.Models;

namespace RoverApp
{
    public class SimpleRover : IRover
    {
        public string StartRover(Point startingLocation, int gridSize, IList<Point> componentLocations)
        {
            if (startingLocation.X >= gridSize ||
                startingLocation.X < 0 ||
                startingLocation.Y >= gridSize ||
                startingLocation.Y < 0)
            {
                return "Startinglocation is outside of the grid";
            }

            var beginPath = startingLocation;
            var directions = new StringBuilder();

            foreach (var componentLocation in componentLocations)
            {
                if (componentLocation.X >= gridSize ||
                    componentLocation.X < 0 ||
                    componentLocation.Y >= gridSize ||
                    componentLocation.Y < 0)
                {
                    return "One component is outside of the grid";
                }

                var path = GetPath(beginPath, componentLocation);
                beginPath = componentLocation;
                directions.Append("Component pickup: ");
                directions.Append(path);
                directions.Append("P");
                directions.AppendLine();
            }

            return directions.ToString();
        }

        private static string GetPath(Point startPoint, Point endPoint)
        {
            var path = new StringBuilder();
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

            return path.ToString();
        }
    }
}
