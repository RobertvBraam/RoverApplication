using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using RoverApp.Models;

namespace RoverApp
{
    public class Program
    {
        /// <summary>
        /// The maximum value of a grid is set to 1000
        /// </summary>
        private static int _gridSize = 1000;

        private static IContainer _container;

        private static void Main(string[] args)
        {
            SetUpContainer();

            Console.WriteLine("Welcome to the Roverbot (values can only be a number bigger then 0 or bigger then the gridsize)");
            Console.WriteLine("For a simple rover type: YES!");

            var isSimpleRover = Console.ReadLine();
            var roverType = isSimpleRover == "YES!" ? RoverType.SimpleRover : RoverType.Rover;

            _gridSize = ValidateInput("Please fill in the grid size");

            var xStartCoordinate = ValidateInput("Please fill in the X coordinate of the starting position");
            var yStartCoordinate = ValidateInput("Please fill in the Y coordinate of the starting position");
            var startingPosition = new Point(xStartCoordinate - 1, yStartCoordinate - 1);

            var numberOfComponents = ValidateInput("Please fill in the number of components");

            var components = new List<Point>();
            for (var i = 1; i < numberOfComponents + 1; i++)
            {
                var xCoordinate = ValidateInput($"Please fill in the X coordinate of component number {i}");
                var yCoordinate = ValidateInput($"Please fill in the Y coordinate of component number {i}");
                components.Add(new Point(xCoordinate - 1, yCoordinate - 1));
            }

            var rover = _container.ResolveKeyed<IRover>(roverType);
            var directionsToComponents = rover.StartRover(startingPosition, _gridSize, components);

            Console.WriteLine(directionsToComponents);
            Console.ReadKey();
        }

        /// <summary>
        /// Setting up the DI container
        /// </summary>
        private static void SetUpContainer()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterType<SimpleRover>()
                .Keyed<IRover>(RoverType.SimpleRover)
                .InstancePerDependency();
            builder
                .RegisterType<Rover>()
                .Keyed<IRover>(RoverType.Rover)
                .InstancePerDependency();
            builder
                .RegisterType<PathFinder>()
                .As<IPathFinder>()
                .InstancePerDependency();

            _container = builder.Build();
        }

        /// <summary>
        /// It validates the user input: 
        /// It needs to be an interger between 0 and 1000
        /// </summary>
        /// <param name="message"></param>
        /// <returns>The number that has been input</returns>
        private static int ValidateInput(string message)
        {
            Console.WriteLine(message);
            var value = Console.ReadLine();
            int output;

            while (!int.TryParse(value, out output) || 
                output < 0 || 
                output > _gridSize)
            {
                Console.WriteLine("Please fill in a number that's bigger then 0 and not bigger then the gridsize");
                value = Console.ReadLine();
            }

            return output;
        }
    }
}
