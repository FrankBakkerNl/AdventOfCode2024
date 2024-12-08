namespace AdventOfCode2024.Puzzles.DayXX;

/// <summary>
/// https://adventofcode.com/2024/day/xx
/// </summary>
public class Day06
{
    private static readonly (int dx, int dy)[] Directions = [
        ( 0, -1), // Up
        ( 1,  0), // Right
        ( 0,  1), // Down
        (-1,  0)  // Left
    ];

    [Result(5153)]
    [TestCase(result: 41)]
    public static int GetAnswer1(string[] input)
    {
        var (map, position) = ParseMap(input);

        var visitmap = new bool[map.GetLength(0), map.GetLength(1)];

        return GetVisitedcellCount(visitmap, position, map);
    }

    private static int GetVisitedcellCount(bool[,] visitmap, (int x, int y) position, bool[,] map)
    {
        visitmap[position.x, position.y] = true; 
        var count = 1; // include start position

        var dirIndex = 0;
        var directionVector = Directions[dirIndex];
        while (true)
        {
            var newPosition = Add(position, directionVector);

            if (newPosition.x < 0 || newPosition.x >= map.GetLength(0) || 
                newPosition.y < 0 || newPosition.y >= map.GetLength(1)) return count;
            
            if (map[newPosition.x, newPosition.y])
            {
                // We hit an obstacle, turn right
                dirIndex = (dirIndex + 1) % 4;
                directionVector = Directions[dirIndex];
            }
            else
            {
                position = newPosition;
                ref var visitedRef = ref visitmap[position.x, position.y];
                if (!visitedRef) count++;
                visitedRef = true;
            }
        }
    }

    static (int x, int y) Add((int x, int y) a, (int x, int y) b) => (a.x + b.x, a.y + b.y);
        
    [Result(1711)]
    //[TestCase(result: 6)]
    [Focus]
    public static long GetAnswer2(string[] input)
    {
        var (map, position) = ParseMap(input);
        
        // First find the cells visited in the normal path
        var xLength = map.GetLength(0);
        var yLength = map.GetLength(1);
        
        var visitmap = new bool[xLength, yLength];
        GetVisitedcellCount(visitmap, position, map);

        var numberOfpossibleObstaclePostions = 0;

        // reuse the same visitDirectionHistory array for each iteration.  
        var visitDirectionHistory = new int[xLength, yLength];
        
        for (var x = 0; x < xLength; x++)
        {
            for (var y = 0; y < yLength; y++)
            {
                // If this cell is not in the normal path, setting a new obstacle here will not change anything   
                if (!visitmap[x,y]) continue;
                
                ref var testCell = ref map[x, y]; 
                if (testCell) continue; // do not try to put an obstacle where there was one already
                testCell = true;

                // Prepare the map for reuse
                if (x != 0 && y != 0) Array.Clear(visitDirectionHistory);

                if (IsInLoop(map, position, visitDirectionHistory, xLength, yLength)) numberOfpossibleObstaclePostions++;

                testCell = false;
            }
        }

        return numberOfpossibleObstaclePostions;
    }

    private static bool IsInLoop(bool[,] map, (int x, int y) position, int[,] visitDirectionHistory, int xLength, int yLength)
    {
        var dirIndex = 0;
        var directionVector = Directions[dirIndex];
        
        visitDirectionHistory[position.x, position.y] = dirIndex;

        while (true)
        {
            var newPosition = Add(position, directionVector);

            if (newPosition.x < 0 || newPosition.x >= xLength || 
                newPosition.y < 0 || newPosition.y >= yLength) return false;
            
            if (map[newPosition.x, newPosition.y])
            {
                // we hit an obstacle, turn right
                dirIndex = (dirIndex + 1) % 4; // turn right
                directionVector = Directions[dirIndex];
            }
            else if (visitDirectionHistory[newPosition.x, newPosition.y] == dirIndex  +1)
            {
                // we have been here before in same direction, so it is a loop
                return true;
            }
            else
            {
                position = newPosition;
                visitDirectionHistory[position.x, position.y] = dirIndex + 1;
            }
        }
    }

    static (bool[,] map, (int x, int y) start) ParseMap(string[] input)
    {
        var map = new bool[input[1].Length, input.Length];
        var start = (0,0);
        foreach (var (y, line) in input.Index())
        {
            foreach (var (x, c) in line.Index())
            {
                if (c == '#') map[x, y] = true;
                if (c == '^') start = (x, y);
            }
        }

        return (map, start);
    }
}