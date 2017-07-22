using System;
using System.Collections.Generic;
using System.Linq;
using Flai.DataStructures;

namespace Flai.Pathfinding
{
    // NOTE: Currently not working completely if diagonal movement is enabled. For example
    // . == empty tile, O == blocked tile, S == start, G == goal
    //   .OS..
    //   .GO..
    //   .....
    //
    // It is possible to move from S to G

    // Not thoroughly tested and not sure if default heuristic/path cost function is optimal 
    // TODO: SearchNode pooling
    public static class PathFinder2D
    {
        #region Fields

        private static readonly QuaternaryHeap<SearchNode2D> _quaternaryHeap = new QuaternaryHeap<SearchNode2D>(16, HeapType.MinHeap);
        private static readonly BinaryHeap<SearchNode2D> _binaryHeap = new BinaryHeap<SearchNode2D>(16, HeapType.MinHeap);
        private static readonly Dictionary<Vector2i, SearchNode2D> _openSetMembership = new Dictionary<Vector2i, SearchNode2D>();

        private static readonly Vector2i[] AdjacentTiles = new Vector2i[] { -Vector2i.UnitX, Vector2i.UnitX, -Vector2i.UnitY, Vector2i.UnitY };
        private static readonly Vector2i[] DiagonalTiles = new Vector2i[] 
        { 
            new Vector2i(-1, -1), new Vector2i(-1, 1), new Vector2i(1, -1), new Vector2i(1, 1) // Diagonal tiles
            -Vector2i.UnitX, Vector2i.UnitX, -Vector2i.UnitY, Vector2i.UnitY, // Adjacent tiles
        };

        public static readonly Func<Vector2i, Vector2i, bool, int> DefaultHeuristicFunction =
            new Func<Vector2i, Vector2i, bool, int>((start, goal, allowDiagonal) =>
            {
                if (allowDiagonal)
                {
                    int xDifference = Math.Abs(start.X - goal.X);
                    int yDifference = Math.Abs(start.Y - goal.Y);

                    return (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + Math.Abs(xDifference - yDifference)) * 10;
                }
                return (int)(Math.Abs(start.X - goal.X) + Math.Abs(start.Y - goal.Y)) * 10;
            });

        public static readonly Func<Vector2i, Vector2i, int> DefaultPathCostFunction =
            new Func<Vector2i, Vector2i, int>((start, goal) =>
            {
                if (start.X != goal.X && start.Y != goal.Y)
                {
                    return 14;
                }

                return 10;
            });

        #endregion

        #region Find Path

        #region Overloads

        public static Vector2i[] FindPath(bool[,] map, Vector2i start, Vector2i goal)
        {
            return PathFinder2D.FindPath(map, start, goal, false);
        }

        public static Vector2i[] FindPath(bool[,] map, Vector2i start, Vector2i goal, bool allowDiagonalMovement)
        {
            return PathFinder2D.FindPath((position) =>
                {
                    if (position.X < 0 || position.Y < 0 || position.X >= map.GetLength(0) || position.X >= map.GetLength(1))
                    {
                        return false;
                    }

                    return map[position.X, position.Y] == false;
                }, start, goal, PathFinder2D.DefaultHeuristicFunction, PathFinder2D.DefaultPathCostFunction, allowDiagonalMovement);
        }

        public static Vector2i[] FindPath(IMap2D map, Vector2i start, Vector2i goal)
        {
            return PathFinder2D.FindPath((position) => map.IsFree(position), start, goal, PathFinder2D.DefaultHeuristicFunction, PathFinder2D.DefaultPathCostFunction, false);
        }

        public static Vector2i[] FindPath(IMap2D map, Vector2i start, Vector2i goal, bool allowDiagonalMovement)
        {
            return PathFinder2D.FindPath((position) => map.IsFree(position), start, goal, PathFinder2D.DefaultHeuristicFunction, PathFinder2D.DefaultPathCostFunction, allowDiagonalMovement);
        }

        public static Vector2i[] FindPath(IMap2D map, Vector2i start, Vector2i goal, Func<Vector2i, Vector2i, bool, int> heuristicFunction, Func<Vector2i, Vector2i, int> pathCostFunction)
        {
            return PathFinder2D.FindPath((position) => map.IsFree(position), start, goal, heuristicFunction, pathCostFunction, false);
        }

        public static Vector2i[] FindPath(IMap2D map, Vector2i start, Vector2i goal, Func<Vector2i, Vector2i, bool, int> heuristicFunction, Func<Vector2i, Vector2i, int> pathCostFunction, bool allowDiagonalMovement)
        {
            return PathFinder2D.FindPath((position) => map.IsFree(position), start, goal, heuristicFunction, pathCostFunction, allowDiagonalMovement);
        }

        public static Vector2i[] FindPath(Predicate<Vector2i> isFreeFunc, Vector2i start, Vector2i goal)
        {
            return PathFinder2D.FindPath(isFreeFunc, start, goal, PathFinder2D.DefaultHeuristicFunction, PathFinder2D.DefaultPathCostFunction, false);
        }

        public static Vector2i[] FindPath(Predicate<Vector2i> isFreeFunc, Vector2i start, Vector2i goal, bool allowDiagonalMovement)
        {
            return PathFinder2D.FindPath(isFreeFunc, start, goal, PathFinder2D.DefaultHeuristicFunction, PathFinder2D.DefaultPathCostFunction, allowDiagonalMovement);
        }

        public static Vector2i[] FindPath(Predicate<Vector2i> isFreeFunc, Vector2i start, Vector2i goal, Func<Vector2i, Vector2i, bool, int> heuristicFunction, Func<Vector2i, Vector2i, int> pathCostFunction)
        {
            return PathFinder2D.FindPath(isFreeFunc, start, goal, heuristicFunction, pathCostFunction, false);
        }

        #endregion

        public static Vector2i[] FindPath(Predicate<Vector2i> isFreeFunc, Vector2i start, Vector2i goal, Func<Vector2i, Vector2i, bool, int> heuristicFunction, Func<Vector2i, Vector2i, int> pathCostFunction, bool allowDiagonalMovement)
        {
            _openSetMembership.Clear();
            PriorityQueue<SearchNode2D> openSet = PathFinder2D.ChooseOptimalPriorityQueue(start, goal);

            SearchNode2D startNode = new SearchNode2D(null, start, heuristicFunction(start, goal, allowDiagonalMovement), 0);
            openSet.Add(startNode);
            _openSetMembership.Add(startNode.Position, startNode);

            while (openSet.HasNext)
            {
                SearchNode2D currentNode = openSet.Dequeue();
                _openSetMembership[currentNode.Position] = currentNode;

                if (currentNode.Position == goal)
                {
                    return PathFinder2D.ReconstructPath(currentNode);
                }

                // Neighbour tiles
                Vector2i[] adjacentTiles = allowDiagonalMovement ? PathFinder2D.DiagonalTiles : PathFinder2D.AdjacentTiles;
                foreach (Vector2i position in adjacentTiles.Select(adjacentTile => currentNode.Position + adjacentTile).Where(position => isFreeFunc(position)))
                {
                    if (!_openSetMembership.ContainsKey(position))
                    {
                        int heuristicCost = heuristicFunction(position, goal, allowDiagonalMovement);
                        int pathCost = currentNode.PathCost + pathCostFunction(currentNode.Position, position);

                        SearchNode2D node = new SearchNode2D(currentNode, position, heuristicCost, pathCost);
                        openSet.Add(node);
                        _openSetMembership[node.Position] = node;
                    }
                    else
                    {
                        // I'm not sure if this actually works. What if the node that is being updated has already child nodes?
                        SearchNode2D node = _openSetMembership[position];
                        int newPathCost = currentNode.PathCost + pathCostFunction(currentNode.Position, node.Position);

                        if (node.PathCost > newPathCost)
                        {
                            node.PathCost = newPathCost;
                            node.Parent = currentNode;
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        #region Other Methods and SearchNode2D

        private static Vector2i[] ReconstructPath(SearchNode2D goalNode)
        {
            int pathLength = goalNode.PathNodes + 1;
            if (pathLength <= 1) // 1 or 0?
            {
                return null;
            }

            Vector2i[] path = new Vector2i[pathLength];

            int pathIndex = pathLength - 1;
            SearchNode2D current = goalNode;
            while (current != null)
            {
                path[pathIndex--] = current.Position;
                current = current.Parent;
            }

            return path;
        }

        private static PriorityQueue<SearchNode2D> ChooseOptimalPriorityQueue(Vector2i start, Vector2i goal)
        {
            float estimatedAreaWidth = FlaiMath.Abs(start.X - goal.X) * 1.5f;
            float estimatedAreaHeight = FlaiMath.Abs(start.Y - goal.Y) * 1.5f;

            float estimatedArea = estimatedAreaWidth * estimatedAreaHeight;
            if (estimatedArea > 20000f)
            {
                _binaryHeap.Clear();
                return _binaryHeap;
            }
            else
            {
                _quaternaryHeap.Clear();
                return _quaternaryHeap;
            }
        }

        // Make this a struct or make a pool of SearchNode's which get
        private class SearchNode2D : IComparable<SearchNode2D>, IEquatable<SearchNode2D>
        {
            private int _pathCost;
            private readonly int _heuristicCost;

            private readonly Vector2i _position;
            private SearchNode2D _parent;

            public SearchNode2D(SearchNode2D parent, Vector2i position, int heuristicCost, int pathCost)
            {
                _parent = parent;
                _position = position;

                _heuristicCost = heuristicCost;
                _pathCost = pathCost;
            }

            // Get rid of properties? It'd faster to use only fields and since this is used only inside PathFinding.cs, it shouldn't matter
            public SearchNode2D Parent
            {
                get { return _parent; }
                set { _parent = value; }
            }

            public Vector2i Position
            {
                get { return _position; }
            }

            public int HeuristicCost
            {
                get { return _heuristicCost; }
            }

            public int PathCost
            {
                get { return _pathCost; }
                set { _pathCost = value; }
            }

            public int TotalCost
            {
                get { return _heuristicCost + _pathCost; }
            }

            public int PathNodes
            {
                get { return _parent == null ? 0 : _parent.PathNodes + 1; }
            }

            public override bool Equals(object obj)
            {
                if (obj is SearchNode2D)
                {
                    return this.Equals((SearchNode2D)obj);
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return _position.GetHashCode();
            }

            public override string ToString()
            {
                return string.Format("DisplayPosition: {0}, Cost: {1}", _position, this.TotalCost);
            }

            #region IEquatable<SearchNode2D> Members

            public bool Equals(SearchNode2D other)
            {
                return _position == other._position && _pathCost == other._pathCost && _heuristicCost == other._heuristicCost;
            }

            #endregion

            #region IComparable<SearchNode2D> Members

            public int CompareTo(SearchNode2D other)
            {
                return this.TotalCost - other.TotalCost;
            }

            #endregion
        }

        #endregion
    }
}
