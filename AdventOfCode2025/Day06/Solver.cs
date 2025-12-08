using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Day06
{
    public class Solver(string input) : ISolver
    {
        private readonly string _input = input;

        public string Part1()
        {
            var lines = _input.AsList();
            var cols = new List<List<long>>();
            var operators = new List<string>();

            for (int i = 0; i < lines.Count - 1; i++)
            {
                var line = lines[i];
                var row = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int x=0;x<row.Length;x++)
                {
                    if (i == 0)
                    {
                        cols.Add(new List<long>());
                    }
                    cols[x].Add(long.Parse(row[x]));
                }
            }

            foreach(var op in lines[lines.Count-1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                operators.Add(op);
            }

            long runningTotal = 0;
            for (int i=0; i<cols.Count;i++)
            {
                long total = operators[i] == "+" ? 0 : 1;
                foreach (var val in cols[i])
                {
                    switch (operators[i])
                    {
                        case "+": total += val; break;
                        case "*": total *= val; break;
                    }
                }
                runningTotal += total;
            }

            return runningTotal.ToString();
        }

        public string Part2()
        {
            //var lines = _input.AsList();
            var lines = SplitLines(_input); // It seems my original AsList() helper function discards trailing spaces in the last line. Need to fix that.
            var colSizes = GetColumnSizes(lines.Last());
            var operators = new List<string>();
            var grid = SplitTo2DArray(lines, colSizes);

            foreach (var op in lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                operators.Add(op);
            }

            var columns = GetAllColumns<string>(grid);
            var runningTotal = 0L;
            for(var col=0;col< columns.Count;col++)
            {
                var column = columns[col][..^1];
                var op = columns[col].Last().Replace(" ", "");
                var charGrid = ToCharArray2D(column);
                var xFormedValues = charGrid.GetRowStrings();

                long total = op == "+" ? 0 : 1;
                foreach(var val in xFormedValues)
                {
                    switch (op)
                    {
                        case "+": total += long.Parse(val.Replace(" ", "")); break;
                        case "*": total *= long.Parse(val.Replace(" ", "")); break;
                    }
                }

                runningTotal += total;
            }

            return runningTotal.ToString();
        }

        public static List<string> SplitLines(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var lines = input
                .Replace("\r\n", "\n")
                .Split('\n')
                .ToList();

            return lines;
        }

        public static string[,] SplitTo2DArray(List<string> input, List<int> colSizes)
        {
            int rows = input.Count;
            int cols = colSizes.Count;
            string[,] result = new string[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                string line = input[r];
                int position = 0;

                for (int c = 0; c < cols; c++)
                {
                    int size = colSizes[c];

                    string segment = line.Substring(position, size);
                    result[r, c] = segment;

                    position += size;

                    if (position < line.Length)
                        position += 1;
                }
            }

            return result;
        }

        public static List<int> GetColumnSizes(string pattern)
        {
            var markers = new List<int>();

            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == '+' || pattern[i] == '*')
                    markers.Add(i);
            }

            var result = new List<int>();

            for (int i = 0; i < markers.Count - 1; i++)
            {
                int size = markers[i + 1] - markers[i];
                result.Add(size-1);
            }

            result.Add(pattern.Length - markers.Last());

            return result;
        }

        public static List<List<T>> GetAllColumns<T>(T[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            var result = new List<List<T>>(cols);

            for (int c = 0; c < cols; c++)
            {
                var column = new List<T>(rows);

                for (int r = 0; r < rows; r++)
                {
                    column.Add(array[r, c]);
                }

                result.Add(column);
            }

            return result;
        }

        public static char[,] ToCharArray2D(List<string> lines)
        {
            int rows = lines.Count;
            int cols = lines[0].Length;

            var result = new char[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                var line = lines[r];
                for (int c = 0; c < cols; c++)
                {
                    result[r, c] = line[c];
                }
            }

            return result;
        }
    }
}
