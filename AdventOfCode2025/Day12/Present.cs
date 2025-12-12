namespace AdventOfCode2025.Day12
{
    using System;
    using Utils;

    public class Present
    {
        private readonly bool[,] _shape;

        public Present(List<string> lines)
        {
            int w = lines.Max(l => l.Length);
            int h = lines.Count;

            _shape = new bool[w, h];
            for (int row = 0; row < h; row++)
                for (int col = 0; col < w; col++)
                    _shape[col, row] = lines[row][col] == '#';
        }

        public int Tiles()
        {
            int n0 = _shape.GetLength(0);
            int n1 = _shape.GetLength(1);
            int count = 0;

            for (int r = 0; r < n0; r++)
                for (int c = 0; c < n1; c++)
                    if (_shape[r, c])
                        count++;

            return count;
        }

        public bool[,] Orientation(int orient)
        {
            return (orient % 4) switch
            {
                0 => _shape,
                1 => Rotate90Clockwise(),
                2 => Rotate180(),
                3 => Rotate90CounterClockwise(),
                _ => throw new ArgumentException("Unable to transpose grid"),
            };
        }

        private bool[,] Rotate90Clockwise()
        {
            int n = _shape.GetLength(0);
            bool[,] rotated = new bool[n, n];

            for (int r = 0; r < n; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    rotated[c, n - 1 - r] = _shape[r, c];
                }
            }

            return rotated;
        }

        private bool[,] Rotate90CounterClockwise()
        {
            int n = _shape.GetLength(0);
            bool[,] rotated = new bool[n, n];

            for (int r = 0; r < n; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    rotated[n - 1 - c, r] = _shape[r, c];
                }
            }

            return rotated;
        }

        private bool[,] Rotate180()
        {
            int n = _shape.GetLength(0);
            bool[,] rotated = new bool[n, n];

            for (int r = 0; r < n; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    rotated[n - 1 - r, n - 1 - c] = _shape[r, c];
                }
            }

            return rotated;
        }
    }
}
