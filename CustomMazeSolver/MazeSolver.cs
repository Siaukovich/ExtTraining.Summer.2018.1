namespace CustomMazeSolver
{
    using System;

    /// <summary>
    /// Class for finding shortest path to exit in a maze.
    /// </summary>
    public static class MazeSolver
    {
        /// <summary>
        /// Finds path from start point to exit with given algorithm.
        /// </summary>
        /// <param name="mazeExitFinder">
        /// Object that implements <see cref="IMazeExitFinder"/> and finds the way.
        /// </param>
        /// <param name="maze">
        /// Maze as 2d array where 0 stands for "able to go to" 
        /// and -1 stands for "wall".
        /// </param>
        /// <param name="startX">
        /// X coordinate of start.
        /// </param>
        /// <param name="startY">
        /// Y coordinate of start.
        /// </param>
        /// <returns>
        /// The <see cref="Cell[]"/>.
        /// Path from start to exit, where 0 element is start and last is the exit.
        /// Returns null if impossible to exit.
        /// </returns>
        public static Cell[] FindPath(IMazeExitFinder mazeExitFinder, int[,] maze, int startX, int startY)
        {
            ThrowForInvalidParametes();

            return mazeExitFinder.FindExit(maze, new Cell(startX, startY));

            void ThrowForInvalidParametes()
            {
                if (mazeExitFinder == null)
                {
                    throw new ArgumentNullException(nameof(mazeExitFinder));
                }

                ThrowForInvalid(maze, startX, startY);
            }
        }

        /// <summary>
        /// Finds path from start point to exit with BFS algorithm.
        /// </summary>
        /// <param name="maze">
        /// Maze as 2d array where 0 stands for "able to go to" 
        /// and -1 stands for "wall".
        /// </param>
        /// <param name="startX">
        /// X coordinate of start.
        /// </param>
        /// <param name="startY">
        /// Y coordinate of start.
        /// </param>
        /// <returns>
        /// The <see cref="Cell[]"/>.
        /// Path from start to exit, where 0 element is start and last is the exit.
        /// Returns null if impossible to exit.
        /// </returns>
        public static Cell[] FindPath(int[,] maze, int startX, int startY)
        {
            ThrowForInvalid(maze, startX, startY);

            return FindPath(new BfsExitFinder(), maze, startX, startY);
        }

        /// <summary>
        /// Checks for invalid parameters.
        /// </summary>
        /// <param name="maze">
        /// The maze.
        /// </param>
        /// <param name="startX">
        /// X coordinate of start.
        /// </param>
        /// <param name="startY">
        /// Y coordinate of start.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if maze array is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if start position is equal to -1 
        /// or start position out of maze array range.
        /// </exception>
        private static void ThrowForInvalid(int[,] maze, int startX, int startY)
        {
            if (maze == null)
            {
                throw new ArgumentNullException(nameof(maze));
            }

            if (startX < 0 || startX >= maze.GetLength(1))
            {
                throw new ArgumentException(nameof(startX));
            }

            if (startY < 0 || startY >= maze.GetLength(0))
            {
                throw new ArgumentException(nameof(startY));
            }

            if (maze[startX, startY] == -1)
            {
                throw new ArgumentException("Start position is -1.");
            }
        }
    }
}
