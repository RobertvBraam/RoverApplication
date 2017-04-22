using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RoverApp.Models;

namespace RoverApp.Test
{
    [TestFixture]
    public class RoverTests
    {
        [TestCaseSource(nameof(SetupSamples))]
        public void StartRover_ShouldReturnStringOfCoordinates_WhenGivenTheSetupValues(
            Point startingLocation, int gridSize, IList<Point> locations, string expected)
        {
            //Arrange
            var pathFinder = Substitute.For<IPathFinder>();
            pathFinder
                .FindPath(Arg.Any<SearchParameters>())
                .Returns(locations);

            //Act
            var rover = new Rover(pathFinder);
            var actual = rover.StartRover(startingLocation, gridSize, locations);

            //Assert
            actual.Should().Be(expected);
        }

        private static object[] SetupSamples =
        {
            new object[]
            {
                new Point(1, 2),
                2,
                new List<Point>
                {
                    new Point(2, 2)
                },
                "Component pickup: E-P\r\n"
            },
            new object[]
            {
                new Point(1, 2),
                3,
                new List<Point>
                {
                    new Point(2, 2),
                    new Point(2, 3)
                },
                "Component pickup: E-N-P\r\nComponent pickup:S-N-P\r\n"
            }
        };
    }
}
