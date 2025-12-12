using System.Xml.Linq;

namespace AdventOfCode2025.Day12
{
    public class Region(int width, int height, int[] counts)
    {
        public int Width { get; } = width;
        public int Height { get; } = height;
        public int[] Counts { get; } = counts;
    }
}
