using System.Collections.Generic;
using RoverApp.Models;

namespace RoverApp
{
    public interface IRover
    {
        /// <summary>
        /// The method wil start the calculation of the fastest route to get one or more compentents
        /// </summary>
        /// <param name="startingLocation"></param>
        /// <param name="gridSize"></param>
        /// <param name="componentLocations"></param>
        /// <returns>A pretty string of cardinal directions</returns>
        string StartRover(Point startingLocation, int gridSize, IList<Point> componentLocations);
    }
}