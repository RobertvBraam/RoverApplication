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
    public class PathFinderTests
    {
        [TestCaseSource(nameof(SetupSamples))]
        public void FindPath_ShouldReturnListOfPoints_WhenGivenSearchParameters
            (SearchParameters searchParameters, IList<Point> expected)
        {
            //Arrange
            var pathFinder = new PathFinder();

            //Act
            var actual = pathFinder.FindPath(searchParameters);

            //Assert
            actual.ShouldAllBeEquivalentTo(expected);
        }

        private static object[] SetupSamples =
        {
            new object[]
            {
                new SearchParameters
                (
                    new Point(0, 1), 
                    new Point(1, 1),
                    2
                ),
                new List<Point>
                {
                    new Point(1, 1)
                } 
            },
            new object[]
            {
                new SearchParameters
                (
                    new Point(0, 0),
                    new Point(1, 1),
                    2
                ),
                new List<Point>
                {
                    new Point(0, 1),
                    new Point(1, 1)
                }
            }
        };
    }
}
