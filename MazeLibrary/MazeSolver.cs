namespace MazeLibrary
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Class for finding exit out of a maze.
    /// </summary>
    public class MazeSolver
    {
        #region Private Fields

        /// <summary>
        /// Maze matrix.
        /// </summary>
        private readonly int[,] maze;

        /// <summary>
        /// Coordinate of start position.
        /// </summary>
        private readonly Cell start;
        
        /// <summary>
        /// Indicates if exit from the maze was found.
        /// </summary>
        private bool exitWasFound;

        /// <summary>
        /// Indicates if path search algorithm was started.
        /// </summary>
        private bool triedToFindPath;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MazeSolver"/> class. 
        /// </summary>
        /// <param name="mazeModel">
        /// Maze represented as matrix.
        /// </param>
        /// <param name="startX">
        /// X coordinate of start.
        /// </param>
        /// <param name="startY">
        /// Y coordinate of start.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if passed mazeModel is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if startX of startY are not in mazeModel.
        /// </exception>
        public MazeSolver(int[,] mazeModel, int startX, int startY)
        {
            ThrowForInvalidParameters();

            this.maze = mazeModel;
            this.start = new Cell(startX, startY);
            
            void ThrowForInvalidParameters()
            {
                if (mazeModel == null)
                {
                    throw new ArgumentNullException(nameof(mazeModel));
                }

                if (startX < 0 || startX >= mazeModel.GetLength(0))
                {
                    throw new ArgumentException(nameof(startX));
                }

                if (startY < 0 || startY >= mazeModel.GetLength(1))
                {
                    throw new ArgumentException(nameof(startY));
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts an algorithm for finding shortest path with BFS.
        /// </summary>
        public void PassMaze()
        {
            this.triedToFindPath = true;
            
            var queue = new Queue<Cell>();
            queue.Enqueue(this.start);
            
            // Stores true if element vas alredy visited, false if not.
            var visited = new bool[this.maze.GetLength(0), this.maze.GetLength(1)];
            visited[this.start.X, this.start.Y] = true;
        
            // Stores each passed node parent, so we can restore path later.
            var parent = new Cell[this.maze.GetLength(0), this.maze.GetLength(1)];
            var startParent = new Cell(-1, -1);
            parent[this.start.X, this.start.Y] = startParent;

            while (queue.Count != 0 && !this.exitWasFound)
            {
                var current = queue.Dequeue();
                foreach (var neighbor in this.GetNeighbors(current, visited))
                {
                    if (visited[neighbor.X, neighbor.Y])
                    {
                        continue;
                    }

                    parent[neighbor.X, neighbor.Y] = current;
                    
                    if (this.IsExit(neighbor))
                    {
                        this.exitWasFound = true;
                        this.MarkPathOnMazeMatrix(parent, startParent, neighbor);
                        break;
                    }
                    
                    visited[neighbor.X, neighbor.Y] = true;
                    queue.Enqueue(neighbor);
                }
            }
        }
        
        /// <summary>
        /// Returns maze array with path on it.
        /// </summary>
        /// <returns>
        /// Maze array with path on it.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if path finding algorithm wasn't started or path was not found.
        /// </exception>
        public int[,] MazeWithPass()
        {
            if (!this.triedToFindPath)
            {
                throw new InvalidOperationException("Path finding algorithm was not started.");
            }

            if (!this.exitWasFound)
            {
                throw new InvalidOperationException("Path was not found.");
            }

            return this.maze;
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Places number that indicate path from start position to exit.
        /// </summary>
        /// <param name="parent">
        /// Array of parent cells.
        /// </param>
        /// <param name="startParent">
        /// Constant value used to indicate that this cell is a start one (has no parent).
        /// </param>
        /// <param name="end">
        /// Exit cell.
        /// </param>
        private void MarkPathOnMazeMatrix(Cell[,] parent, Cell startParent, Cell end)
        {
            var path = new List<Cell>();
            Cell current = end;
            while (current.X != startParent.X && current.Y != startParent.Y)
            {
                path.Add(current);
                current = parent[current.X, current.Y];
            }

            path.Reverse();

            for (int i = 0; i < path.Count; i++)
            {
                Cell element = path[i];
                this.maze[element.X, element.Y] = i + 1;
            }
        }

        /// <summary>
        /// Checks if given element is exit from the maze.
        /// </summary>
        /// <param name="element">
        /// Element that needs to be checked.
        /// </param>
        /// <returns>
        /// True if passed element is exit from the maze,
        /// false otherwise.
        /// </returns>
        private bool IsExit(Cell element)
        {
            bool isExitOnTop = element.X == 0;
            bool isExitOnLeftSide = element.Y == 0;
            
            int lastIndexX = this.maze.GetLength(0) - 1;
            int lastIndexY = this.maze.GetLength(1) - 1;
            
            bool isExitOnDownSide = element.X == lastIndexX;
            bool isExitOnRightSide = element.Y == lastIndexY;

            return isExitOnTop || isExitOnLeftSide || isExitOnDownSide || isExitOnRightSide;
        }

        /// <summary>
        /// Finds all neighbors of a given element.
        /// </summary>
        /// <param name="next">
        /// Given element.
        /// </param>
        /// <param name="visited">
        /// Array that indicated if element was already visited.
        /// </param>
        /// <returns>
        /// List of neighbor's coordinates.
        /// </returns>
        private IEnumerable<Cell> GetNeighbors(Cell next, bool[,] visited)
        {
            var left = new Cell(next.X, next.Y - 1);
            var right = new Cell(next.X, next.Y + 1);
            var up = new Cell(next.X - 1, next.Y);
            var down = new Cell(next.X + 1, next.Y);

            if (left.Y >= 0 && !visited[left.X, left.Y] && this.maze[left.X, left.Y] != -1)
            {
                yield return left;
            }
            
            if (up.X >= 0 && !visited[up.X, up.Y] && this.maze[up.X, up.Y] != -1)
            {
                yield return up;
            }

            if (right.Y < this.maze.GetLength(1) && !visited[right.X, right.Y] && this.maze[right.X, right.Y] != -1)
            {
                yield return right;
            }

            if (down.X < this.maze.GetLength(0) && !visited[down.X, down.Y] && this.maze[down.X, down.Y] != -1)
            {
                yield return down;
            }
        }
        
        /// <summary>
        /// Class-helper for indicating single cell in maze matrix.
        /// </summary>
        private class Cell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Cell"/> class.
            /// </summary>
            /// <param name="x">
            /// Cell's x coordinate.
            /// </param>
            /// <param name="y">
            /// Cell's y coordinate.
            /// </param>
            public Cell(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            /// <summary>
            /// Gets this cell X coordinate.
            /// </summary>
            public int X { get; }

            /// <summary>
            /// Gets this cell Y coordinate.
            /// </summary>
            public int Y { get; }

            /// <summary>
            /// The == operator.
            /// </summary>
            /// <param name="lhs">
            /// Left operand.
            /// </param>
            /// <param name="rhs">
            /// Right operand.
            /// </param>
            /// <returns>
            /// True if both cell's coordinates are equal.
            /// </returns>
            /// <exception cref="ArgumentNullException">
            /// Thrown if one of operands was null.
            /// </exception>
            public static bool operator ==(Cell lhs, Cell rhs)
            {
                if (lhs == null)
                {
                    throw new ArgumentNullException(nameof(lhs));
                }

                if (rhs == null)
                {
                    throw new ArgumentNullException(nameof(rhs));
                }
                
                return lhs.X == rhs.X && lhs.Y == rhs.Y;
            }

            /// <summary>
            /// The != operator.
            /// </summary>
            /// <param name="lhs">
            /// Left operand.
            /// </param>
            /// <param name="rhs">
            /// Right operand.
            /// </param>
            /// <returns>
            /// True if cell's coordinates are not equal.
            /// </returns>
            /// <exception cref="ArgumentNullException">
            /// Thrown if one of operands was null.
            /// </exception>
            public static bool operator !=(Cell lhs, Cell rhs) => !(lhs == rhs);

            /// <summary>
            /// String representation of Cell.
            /// </summary>
            /// <returns>
            /// The <see cref="string"/>.
            /// </returns>
            public override string ToString()
            {
                return $"({this.X}, {this.Y})";
            }
        
        #endregion
        }
    }
}
