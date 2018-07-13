namespace CustomMazeSolver
{
    /// <summary>
    /// Interface that all exit finder algorithms must implement.
    /// </summary>
    public interface IMazeExitFinder
    {
        /// <summary>
        /// Finds exit from given maze, starting from given point.
        /// </summary>
        /// <param name="maze">
        /// Maze as 2d array where 0 stands for "able to go to" 
        /// and -1 stands for "wall".
        /// </param>
        /// <param name="start">
        /// Start position.
        /// </param>
        /// <returns>
        /// The <see cref="Cell[]"/>.
        /// Path from start to exit, where 0 element is start and last is the exit.
        /// Returns null if impossible to exit.
        /// </returns>
        Cell[] FindExit(int[,] maze, Cell start);
    }
}