using System.Collections.Generic;
using RoverApp.Models;

namespace RoverApp
{
    public class PathFinder : IPathFinder
    {
        private SearchParameters _searchParameters;
        private int _gridSize;
        private Node[,] _nodes;
        private Node _startNode;
        private Node _endNode;

        public List<Point> FindPath(SearchParameters searchParameters)
        {
            _searchParameters = searchParameters;
            _gridSize = searchParameters.GridSize;

            InitializeNodes();

            var path = new List<Point>();
            var success = Search(_startNode);
            if (success)
            {
                var node = _endNode;
                while (node.ParentNode != null)
                {
                    path.Add(node.Location);
                    node = node.ParentNode;
                }

                path.Reverse();
            }

            return path;
        }

        /// <summary>
        /// Builds the node grid from a simple grid of booleans indicating areas which are and aren't walkable
        /// </summary>
        /// <param name="map">A boolean representation of a grid in which true = walkable and false = not walkable</param>
        private void InitializeNodes()
        {
            _nodes = new Node[_gridSize, _gridSize];
            for (var y = 0; y < _gridSize; y++)
            {
                for (var x = 0; x < _gridSize; x++)
                {
                    _nodes[x, y] = new Node(x, y, _searchParameters.EndLocation);
                }
            }

            _startNode = _nodes[_searchParameters.StartLocation.X, _searchParameters.StartLocation.Y];
            _startNode.State = NodeState.Open;
            _endNode = _nodes[_searchParameters.EndLocation.X, _searchParameters.EndLocation.Y];
        }

        /// <summary>
        /// Attempts to find a path to the destination node using <paramref name="currentNode"/> as the starting location
        /// </summary>
        /// <param name="currentNode">The node from which to find a path</param>
        /// <returns>True if a path to the destination has been found, otherwise false</returns>
        private bool Search(Node currentNode)
        {
            currentNode.State = NodeState.Closed;
            var nextNodes = GetAdjacentWalkableNodes(currentNode);

            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (var nextNode in nextNodes)
            {
                if (nextNode.Location == _endNode.Location)
                {
                    return true;
                }

                if (Search(nextNode))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns any nodes that are adjacent to <paramref name="fromNode"/> and may be considered to form the next step in the path
        /// </summary>
        /// <param name="fromNode">The node from which to return the next possible nodes in the path</param>
        /// <returns>A list of next possible nodes in the path</returns>
        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            var walkableNodes = new List<Node>();
            var nextLocations = GetAdjacentLocations(fromNode.Location);

            foreach (var location in nextLocations)
            {
                var x = location.X;
                var y = location.Y;

                if (x < 0 || x >= _gridSize || y < 0 || y >= _gridSize)
                {
                    continue;
                }

                Node node = _nodes[x, y];

                if (node.State == NodeState.Closed)
                {
                    continue;
                }

                if (node.State == NodeState.Open)
                {
                    var traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                    var gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }

        /// <summary>
        /// Returns the four locations immediately adjacent to <paramref name="fromLocation"/>
        /// </summary>
        /// <param name="fromLocation">The location from which to return all adjacent points</param>
        /// <returns>The locations as an IEnumerable of Points</returns>
        private static IEnumerable<Point> GetAdjacentLocations(Point fromLocation)
        {
            return new[]
            {
                new Point(fromLocation.X - 1, fromLocation.Y    ),
                new Point(fromLocation.X,     fromLocation.Y + 1),
                new Point(fromLocation.X + 1, fromLocation.Y    ),
                new Point(fromLocation.X,     fromLocation.Y - 1)
            };
        }
    }
}
