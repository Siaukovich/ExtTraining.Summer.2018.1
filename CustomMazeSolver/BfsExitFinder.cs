namespace CustomMazeSolver
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class that performs BFS algorithm for finding exit from the maze.
    /// </summary>
    public class BfsExitFinder : IMazeExitFinder
    {
        /// <summary>
        /// Starts an algorithm for finding shortest path with BFS.
        /// </summary>
        /// <param name="maze">
        /// Maze represented as 2d array.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <returns>
        /// The <see cref="Cell[]"/>.
        /// Array that represets path from given start cell to exit.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if exit was not found.
        /// </exception>
        public Cell[] FindExit(int[,] maze, Cell start)
        {
            var queue = new Queue<Cell>();
            queue.Enqueue(start);

            // Stores true if element vas alredy visited, false if not.
            var visited = new bool[maze.GetLength(0), maze.GetLength(1)];
            visited[start.X, start.Y] = true;

            // Stores each passed node parent, so we can restore path later.
            var parent = new Cell[maze.GetLength(0), maze.GetLength(1)];
            var startParent = new Cell(-1, -1);
            parent[start.X, start.Y] = startParent;

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                foreach (var neighbor in GetNeighbors(maze, current, visited))
                {
                    if (visited[neighbor.X, neighbor.Y])
                    {
                        continue;
                    }

                    parent[neighbor.X, neighbor.Y] = current;

                    if (IsExit(maze, neighbor))
                    {
                        return CreatePath(parent, startParent, neighbor);
                    }

                    visited[neighbor.X, neighbor.Y] = true;
                    queue.Enqueue(neighbor);
                }
            }

            throw new ArgumentException("Exit was not found.");
        }

        /// <summary>
        /// Finds all neighbors of a given element.
        /// </summary>
        /// <param name="maze">
        /// Maze as 2d array.
        /// </param>
        /// <param name="next">
        /// Given element.
        /// </param>
        /// <param name="visited">
        /// Array that indicated if element was already visited.
        /// </param>
        /// <returns>
        /// List of neighbor's coordinates.
        /// </returns>
        private IEnumerable<Cell> GetNeighbors(int[,] maze, Cell next, bool[,] visited)
        {
            var left = new Cell(next.X, next.Y - 1);
            var right = new Cell(next.X, next.Y + 1);
            var up = new Cell(next.X - 1, next.Y);
            var down = new Cell(next.X + 1, next.Y);

            if (left.Y >= 0 && !visited[left.X, left.Y] && maze[left.X, left.Y] != -1)
            {
                yield return left;
            }

            if (up.X >= 0 && !visited[up.X, up.Y] && maze[up.X, up.Y] != -1)
            {
                yield return up;
            }

            if (right.Y < maze.GetLength(1) && !visited[right.X, right.Y] && maze[right.X, right.Y] != -1)
            {
                yield return right;
            }

            if (down.X < maze.GetLength(0) && !visited[down.X, down.Y] && maze[down.X, down.Y] != -1)
            {
                yield return down;
            }
        }

        /// <summary>
        /// Checks if given element is exit from the maze.
        /// </summary>
        /// <param name="maze">
        /// Maze as 2d array.
        /// </param>
        /// <param name="element">
        /// Element that needs to be checked.
        /// </param>
        /// <returns>
        /// True if passed element is exit from the maze,
        /// false otherwise.
        /// </returns>
        private static bool IsExit(int[,] maze, Cell element)
        {
            bool isExitOnTop = element.X == 0;
            bool isExitOnLeftSide = element.Y == 0;

            int lastIndexX = maze.GetLength(0) - 1;
            int lastIndexY = maze.GetLength(1) - 1;

            bool isExitOnDownSide = element.X == lastIndexX;
            bool isExitOnRightSide = element.Y == lastIndexY;

            return isExitOnTop || isExitOnLeftSide || isExitOnDownSide || isExitOnRightSide;
        }

        /// <summary>
        /// The create path.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="startParent">
        /// The start parent.
        /// </param>
        /// <param name="end">
        /// The end.
        /// </param>
        /// <returns>
        /// The <see cref="Cell[]"/>.
        /// </returns>
        private Cell[] CreatePath(Cell[,] parent, Cell startParent, Cell end)
        {
            // Will contain path FROM END TO START.
            var path = new List<Cell>();

            Cell current = end;
            while (current != startParent)
            {
                path.Add(current);
                current = parent[current.X, current.Y];
            }

            path.Reverse();

            return path.ToArray();
        }
    }
}