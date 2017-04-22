using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RoverApp.Models;

namespace RoverApp.Test
{
    [TestFixture]
    public class SimpleRoverTests
    {
        [TestCaseSource(nameof(SetupSamples))]
        public void StartRover_ShouldReturnStringOfCoordinates_WhenGivenTheSetupValues(
            Point startingLocation, int gridSize, IList<Point> locations, string expected)
        {
            //Arrange
            var rover = new SimpleRover();
            
            //Act
            var actual = rover.StartRover(startingLocation, gridSize, locations);

            //Assert
            actual.Should().Be(expected);
        }

        private static object[] SetupSamples =
        {
            new object[]
            {
                new Point(0, 1),
                2,
                new List<Point>
                {
                    new Point(1, 1)
                },
                "Component pickup: EP\r\n"
            },
            new object[]
            {
                new Point(0, 2),
                5,
                new List<Point>
                {
                    new Point(2, 2),
                    new Point(2, 4)
                },
                "Component pickup: EEP\r\nComponent pickup: NNP\r\n"
            },
            new object[]
            {
                new Point(0, 1),
                3,
                new List<Point>
                {
                    new Point(2, 2),
                    new Point(2, 4)
                },
                "One component is outside of the grid"
            }
        };
    }
}
