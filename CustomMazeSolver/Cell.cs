namespace CustomMazeSolver
{
    using System;

    /// <summary>
    /// Class-helper for indicating single cell in maze matrix.
    /// </summary>
    public class Cell
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
    }
}