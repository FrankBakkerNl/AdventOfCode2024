namespace AdventOfCode2024.Puzzles.Day10;

/// <summary>
/// https://adventofcode.com/2024/day/10
/// </summary>
public class Day10
{
    //[Result(0)]
    [TestCase(result: 36)]
    [Focus]
    public static int GetAnswer1(string[] input)
    {
        var countPaths = 0;
        for (var y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] - '0' == 0)
                {
                    var allTrailsValidPath = GetAllTrailsValidPath(new Position(x,y), 0, input);
                    countPaths += allTrailsValidPath.Distinct().Count();
                }
                    
            }
        }

        return countPaths;
    }
    static readonly Position[] Directions =
    [
        new Position(1, 0),
        new Position(-1,  0),
        new Position(0,  1),
        new Position( 0, -1)
    ];
    
    private static List<Position> GetAllTrailsValidPath(Position position, int currentValue, string[] input)
    {
        List<Position> found = [];
        
        foreach (var direction in Directions)
        {
            var newPosition = position + direction;
            if (!newPosition.InBounds(input[0].Length, input.Length)) continue;

            var newValue = input[newPosition.Y][newPosition.X] - '0';
            
            if (newValue == currentValue + 1)
            {
                if (newValue == 9) found.Add(newPosition); 
                found.AddRange(GetAllTrailsValidPath(newPosition, currentValue + 1, input));
            }
        }

        return found;
    }

    //[Result(0)]
    [TestCase(result: 81)]
    [Focus]
    public static long GetAnswer2(string[] input)
    {
        var countPaths = 0;
        for (var y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] - '0' == 0)
                {
                    var allTrailsValidPath = GetAllTrailsValidPath(new Position(x,y), 0, input).ToList();
                    countPaths += allTrailsValidPath.Count();
                }
                    
            }
        }

        return countPaths;
    }
    
    record Position(int X, int Y)
    {
        public static Position operator + (Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
        public static Position operator - (Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
        public static Position operator - (Position a) => new(-a.X, -a.Y);
        public bool InBounds(int xBound, int yBound) => X >= 0 && X < xBound && Y >= 0 && Y < yBound;
    }
    
}