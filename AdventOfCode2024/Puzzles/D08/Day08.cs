namespace AdventOfCode2024.Puzzles.DayXX;

/// <summary>
/// https://adventofcode.com/2024/day/08
/// </summary>
public class Day08
{
    [Result(252)]
    [TestCase(result: 14)]
    public static int GetAnswer1(string[] input)
    {
        var map = Parse(input).GroupBy(t=>t.name, t=>t.position);
        var xBound = input[0].Length;
        var yBound = input.Length;

        return map
            .SelectMany(GetAntiPositions)
            .Distinct()
            .Count(p => p.InBounds(xBound, yBound));
    }

    private static IEnumerable<Position> GetAntiPositions(IEnumerable<Position> antennas)
    {
        var all = antennas.ToArray();
        for (var i = 0; i < all.Length-1; i++)
        {
            for (var j = i+1; j < all.Length; j++)
            {
                var pos1 = all[i];
                var pos2 = all[j];
                var delta = pos1 - pos2;
                yield return pos1 + delta; 
                yield return pos2 - delta; 
            }
        }
    }

    //[Result(0)]
    [TestCase(result: 0)]
    public static long GetAnswer2(string[] input)
    {
        return 0;
    }

    static IEnumerable<(char name, Position position)> Parse(string[] input)
    {
        for (int y = 0; y < input[0].Length; y++)
            for (int x = 0; x < input[y].Length; x++)
            {
                var c = input[y][x];
                if (char.IsAsciiLetterOrDigit(c))
                {
                    yield return (c, new Position(x, y));
                }
            }
    }

    record Position(int X, int Y)
    {
        public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
        public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
        public static Position operator -(Position a) => new(-a.X, -a.Y);
        public bool InBounds(int xbound, int yBound) => X >= 0 && X < xbound && Y >= 0 && Y < yBound;
    }
}