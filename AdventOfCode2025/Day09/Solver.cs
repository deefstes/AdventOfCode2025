namespace AdventOfCode2025.Day09
{
    using AdventOfCode2025.Utils;
    using AdventOfCode2025.Utils.Graph;

    public class Solver : ISolver
    {
        private readonly string _input;
        private readonly List<Coordinates> _coordinates = [];

        public Solver(string input)
        {
            _input = input;

            var tiles = _input.AsList();
            foreach (var tile in tiles)
            {
                var vals = tile.Split(',');
                _coordinates.Add(new Coordinates(int.Parse(vals[0]), int.Parse(vals[1])));
            }
        }

        public string Part1()
        {
            var (first, second) = FindLargestRectangle();

            var size = Area(first, second);

            return size.ToString();
        }

        public string Part2()
        {
            var (first, second) = FindLargestInscribedRectangle();

            var size = Area(first, second);

            return size.ToString();
        }

        private static long Area(Coordinates first, Coordinates second)
        {
            return (long)(Math.Abs(second.X - first.X) + 1) * (long)(Math.Abs(second.Y - first.Y) + 1);
        }

        private (Coordinates A, Coordinates B) FindLargestRectangle()
        {
            if (_coordinates == null || _coordinates.Count < 2)
                throw new ArgumentException("At least two points are required.");

            var bestA = new Coordinates(0,0);
            var bestB = new Coordinates(0,0);
            long maxArea = -1;

            for (int i = 0; i < _coordinates.Count; i++)
            {
                for (int j = i + 1; j < _coordinates.Count; j++)
                {
                    var a = _coordinates[i];
                    var b = _coordinates[j];

                    long width = Math.Abs(a.X - b.X);
                    long height = Math.Abs(a.Y - b.Y);
                    long area = width * height;

                    if (area > maxArea)
                    {
                        maxArea = area;
                        bestA = a;
                        bestB = b;
                    }
                }
            }

            return (bestA, bestB);
        }

        private (Coordinates A, Coordinates B) FindLargestInscribedRectangle()
        {
            var wrappedPoints = new List<Coordinates>() { _coordinates.Last() };
            wrappedPoints.AddRange(_coordinates);
            wrappedPoints.Add(_coordinates.First());

            var top = new Coordinates(0, 0);
            var bottom = new Coordinates(0, 0);

            long maxArea = 0;

            for (int i = 1; i < wrappedPoints.Count; i++)
            {
                // Find candidate rectangles where current vertex is at top left
                var tl = wrappedPoints[i];
                var maxX = (int)tl.X;
                while (Utils.IsPointInPolygon(new Coordinates(maxX+1, tl.Y), _coordinates))
                    maxX++;

                foreach (var br in _coordinates
                    .Where(
                        c => c.X > tl.X
                        && c.X < maxX
                        && c.Y > tl.Y))
                {
                    if (RectangleInPolygon(_coordinates, tl, br))
                    {
                        var area = (long)(Math.Abs(br.X - tl.X) + 1) * (long)(Math.Abs(br.Y - tl.Y) + 1);
                        if (area > maxArea)
                        {
                            top = tl;
                            bottom = br;
                            maxArea = area;
                        }
                    }
                }

                // Find candidate rectangles where current vertex is at top right
                var tr = wrappedPoints[i];
                var minX = (int)tl.X;
                while (Utils.IsPointInPolygon(new Coordinates(minX-1, tl.Y), _coordinates))
                    minX--;

                foreach (var bl in _coordinates
                    .Where(
                        c => c.X < tr.X
                        && c.X > minX
                        && c.Y > tr.Y))
                {
                    if (RectangleInPolygon(_coordinates, tr, bl))
                    {
                        var area = (long)(Math.Abs(bl.X - tr.X) + 1) * (long)(Math.Abs(bl.Y - tr.Y) + 1);
                        if (area > maxArea)
                        {
                            top = tr;
                            bottom = bl;
                            maxArea = area;
                        }
                    }
                }
            }

            return (top, bottom);
        }

        private static bool RectangleInPolygon(List<Coordinates> poly, Coordinates first, Coordinates second)
        {
            var minX = (int)Math.Min(first.X, second.X);
            var maxX = (int)Math.Max(first.X, second.X);
            var minY = (int)Math.Min(first.Y, second.Y);
            var maxY = (int)Math.Max(first.Y, second.Y);

            var tl = new Coordinates(minX, minY);
            var tr = new Coordinates(maxX, minY);
            var bl = new Coordinates(minX, maxY);
            var br = new Coordinates(maxX, maxY);

            var top = (tl, tr);
            var bot = (bl, br);
            var lft = (tl, bl);
            var rgt = (tr, br);

            for (int i=0; i<poly.Count;i++)
            {
                var nextI = i + 1;
                if (nextI >= poly.Count)
                    nextI = 0;

                var polyLine = (new Coordinates(poly[i].X, poly[i].Y), new Coordinates(poly[nextI].X, poly[nextI].Y));

                if (Utils.DoesLinesIntersect(top, polyLine)
                    || Utils.DoesLinesIntersect(bot, polyLine)
                    || Utils.DoesLinesIntersect(lft, polyLine)
                    || Utils.DoesLinesIntersect(rgt, polyLine)
                    || !Utils.IsPointInPolygon(tl, poly)
                    || !Utils.IsPointInPolygon(tr, poly)
                    || !Utils.IsPointInPolygon(bl, poly)
                    || !Utils.IsPointInPolygon(br, poly))
                    return false;
            }

            return true;
        }
    }
}
