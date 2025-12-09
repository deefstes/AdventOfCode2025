namespace AdventOfCode2025.Utils
{
    using AdventOfCode2025.Utils.Graph;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    public static class Utils
    {
        public static (T, TimeSpan) MeasureExecutionTime<T>(Func<T> function)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            T result = function();
            stopwatch.Stop();

            return (result, stopwatch.Elapsed);
        }

        public static int Levenshtein(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Verify arguments.
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Initialize arrays.
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Begin looping.
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // Compute cost.
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
                }
            }
            // Return cost.
            return d[n, m];
        }

        public static T Gcd<T>(T a, T b)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            dynamic x = a;
            dynamic y = b;

            if (y == 0)
                return Math.Abs(x);
            else
                return Gcd(y, x % y);
        }

        public static T Lcm<T>(T[] values)
        {
            return values.Aggregate((a, b) => {
                if (a == null)
                    throw new ArgumentNullException(nameof(a));
                if (b == null)
                    throw new ArgumentNullException(nameof(b));

                dynamic x = a;
                dynamic y = b;
                return Math.Abs(x * y / Gcd(x, y));
            });
        }

        public static string GridToString(char[,] grid)
        {
            StringBuilder sb = new();
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    sb.Append(grid[i, j]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static char[,] IntGridToCharGrid(int[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            char[,] result = new char[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    result[r, c] = (char)grid[r, c];
                }
            }

            return result;
        }

        public static List<(T,T)> Combinations<T>(List<T> input)
        {
            List<(T, T)> output = [];

            for (int i = 0; i< input.Count;i++)
            {
                for (int j = i+1;j<input.Count;j++)
                    output.Add((input[j], input[i]));
            }

            return output;
        }

        /// <summary>
        /// Returns the values for all the coefficients (values for a) in a polynomial of the form
        /// y = a[n].x^n + a[n-1].x^(n-1) + ... + a[2].x^2 + a[1].x + a[0]
        /// given a list of n solutions for y and x where y=f(x)
        /// </summary>
        /// <param name="solutions">List of x and y solutions for y=f(x)</param>
        /// <returns>Array of coefficients from highest order (a[n]) to lowest order (a[0])</returns>
        public static double[] DiscoverPolynomial(List<(long y, long x)> solutions)
        {
            var order = solutions.Count;
            var coeffMatrix = new double[order, order + 1];

            for (int row = 0; row < order; row++)
            {
                coeffMatrix[row, order] = solutions[row].y;

                for (int n = 0; n < order; n++)
                {
                    var exp = Math.Pow(solutions[row].x, n);
                    coeffMatrix[row, order - n - 1] = exp;
                }
            }

            return SolveLinearEqSystem(coeffMatrix);
        }

        public static double SolvePolynomial(double[] coefficients, long x)
        {
            double y = 0;
            for (int i=0; i<coefficients.Length; i++)
                y += coefficients[i]*Math.Pow(x, coefficients.Length-i-1);

            return y;
        }

        /// <summary>Computes the solution of a linear equation system.</summary>
        /// <param name="matrix">
        /// The system of linear equations as an augmented matrix[row, col] where (rows + 1 == cols).
        /// It will contain the solution in "row canonical form" if the function returns "true".
        /// </param>
        /// <returns>Returns the list of constants.</returns>
        public static double[] SolveLinearEqSystem(double[,] matrix)
        {
            // input checks
            int rowCount = matrix.GetUpperBound(0) + 1;
            if (matrix == null || matrix.Length != rowCount * (rowCount + 1))
                throw new ArgumentException("The algorithm must be provided with a (n x n+1) matrix.");
            if (rowCount < 1)
                throw new ArgumentException("The matrix must at least have one row.");

            var workingMatrix = new double[rowCount, rowCount + 1];
            Array.Copy(matrix, workingMatrix, matrix.Length);
            var indeterminates = new double[rowCount];

            // pivoting
            for (int col = 0; col + 1 < rowCount; col++)
                if (workingMatrix[col, col] == 0) // check for zero coefficients
                {
                    // find non-zero coefficient
                    int swapRow = col + 1;
                    for (; swapRow < rowCount; swapRow++)
                        if (workingMatrix[swapRow, col] != 0)
                            break;

                    if (workingMatrix[swapRow, col] != 0) // found a non-zero coefficient?
                    {
                        // yes, then swap it with the above
                        double[] tmp = new double[rowCount + 1];
                        for (int i = 0; i < rowCount + 1; i++)
                        {
                            tmp[i] = workingMatrix[swapRow, i];
                            workingMatrix[swapRow, i] = workingMatrix[col, i];
                            workingMatrix[col, i] = tmp[i];
                        }
                    }
                    else
                        throw new ArgumentException("Set of linear equations does not have a solution");
                }

            // elimination
            for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++)
            {
                for (int destRow = sourceRow + 1; destRow < rowCount; destRow++)
                {
                    double df = workingMatrix[sourceRow, sourceRow];
                    double sf = workingMatrix[destRow, sourceRow];
                    for (int i = 0; i < rowCount + 1; i++)
                        workingMatrix[destRow, i] = workingMatrix[destRow, i] * df - workingMatrix[sourceRow, i] * sf;
                }
            }

            // back-insertion
            for (int row = rowCount - 1; row >= 0; row--)
            {
                double f = workingMatrix[row, row];
                if (f == 0)
                    throw new ArgumentException("Set of linear equations does not have a solution");

                for (int i = 0; i < rowCount + 1; i++)
                    workingMatrix[row, i] /= f;

                for (int destRow = 0; destRow < row; destRow++)
                {
                    workingMatrix[destRow, rowCount] -= workingMatrix[destRow, row] * workingMatrix[row, rowCount];
                    workingMatrix[destRow, row] = 0;
                }
            }

            for (int row = 0; row < rowCount; row++)
                indeterminates[row] = workingMatrix[row, rowCount];

            return indeterminates;
        }

        public static T[,] TransposeGrid<T>(T[,] input)
        {
            int rows = input.GetLength(0);
            int cols = input.GetLength(1);

            T[,] output = new T[cols, rows];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    output[c, r] = input[r, c];
                }
            }

            return output;
        }

        public static bool IsPointInPolygon(Coordinates point, List<Coordinates> poly)
        {
            int n = poly.Count;

            // First: check if point is on any edge
            for (int i = 0; i < n; i++)
            {
                var a = poly[i];
                var b = poly[(i + 1) % n];

                // Check if point is collinear with edge segment a->b and lies within bounds
                if (IsPointOnSegment(point, a, b))
                    return true;
            }

            // Standard ray-casting
            bool inside = false;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                double xi = poly[i].X, yi = poly[i].Y;
                double xj = poly[j].X, yj = poly[j].Y;

                if (((yi > point.Y) != (yj > point.Y)) &&
                    (point.X < (xj - xi) * (point.Y - yi) / (yj - yi) + xi))
                {
                    inside = !inside;
                }
            }
            return inside;
        }

        public static bool IsPointOnSegment(Coordinates p, Coordinates a, Coordinates b)
        {
            // Check if point is within bounding box
            if (p.X < Math.Min(a.X, b.X) || p.X > Math.Max(a.X, b.X) ||
                p.Y < Math.Min(a.Y, b.Y) || p.Y > Math.Max(a.Y, b.Y))
                return false;

            // Check collinearity: (b - a) x (p - a) == 0
            double cross = (b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X);
            return Math.Abs(cross) < 1e-9; // allow floating point tolerance
        }

        /// <summary>
        /// This function checks whether two orthogonal lines are perpendicular and intersect. Lines that but up against each other are not counted as interesecting.
        /// </summary>
        public static bool DoesLinesIntersect(
            (Coordinates Line1Start, Coordinates Line1End) line1,
            (Coordinates Line2Start, Coordinates Line2End) line2)
        {
            // Determine orientation of line1
            bool line1Horizontal = line1.Line1Start.Y == line1.Line1End.Y;
            bool line1Vertical = line1.Line1Start.X == line1.Line1End.X;

            // Determine orientation of line2
            bool line2Horizontal = line2.Line2Start.Y == line2.Line2End.Y;
            bool line2Vertical = line2.Line2Start.X == line2.Line2End.X;

            // Only perpendicular pairs (one horizontal, one vertical) can intersect
            if (!((line1Horizontal && line2Vertical) || (line1Vertical && line2Horizontal)))
                return false;

            // Assign horizontal and vertical lines consistently
            var horizontal = line1Horizontal ? line1 : line2;
            var vertical = line1Vertical ? line1 : line2;

            // Horizontal line endpoints
            int hx1 = (int)Math.Min(horizontal.Item1.X, horizontal.Item2.X);
            int hx2 = (int)Math.Max(horizontal.Item1.X, horizontal.Item2.X);
            int hy = (int)horizontal.Item1.Y;

            // Vertical line endpoints
            int vy1 = (int)Math.Min(vertical.Item1.Y, vertical.Item2.Y);
            int vy2 = (int)Math.Max(vertical.Item1.Y, vertical.Item2.Y);
            int vx = (int)vertical.Item1.X;

            // Intersection point
            int ix = vx;
            int iy = hy;

            // Exclude intersections on endpoints
            bool onHorizontal = ix > hx1 && ix < hx2;
            bool onVertical = iy > vy1 && iy < vy2;

            return onHorizontal && onVertical;
        }
    }
}
