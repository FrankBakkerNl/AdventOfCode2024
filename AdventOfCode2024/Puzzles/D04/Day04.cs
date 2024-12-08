namespace AdventOfCode2024.Puzzles.DayXX;

/// <summary>
/// https://adventofcode.com/2024/day/04
/// </summary>
public class Day04
{
    [Result(2545)]
    [TestCase(result: 18)]
    public static int GetAnswer1(string[] input)
    {
        var searchValue = "XMAS";

        (int dx, int dy)[] directions =
        [
            (-1, -1),
            (-1,  0),
            (-1,  1),
            ( 0, -1),
            ( 0,  1),
            ( 1, -1),
            ( 1,  0),
            ( 1,  1),
        ];
        var numberFound = 0;
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] != 'X') continue;
                foreach (var (dx, dy) in directions)
                {
                    var maxx = x + dx * 3;
                    var maxy = y + dy * 3;
                    if (maxx >= input[y].Length || maxx < 0 || maxy >= input.Length || maxy < 0) goto nextDirection;

                    for (var i1 = 1; i1 < searchValue.Length; i1++)
                    {
                        if (input[y + dy * i1][x + dx * i1] != searchValue[i1]) goto nextDirection;
                    }

                    numberFound++;

                    nextDirection: ;
                }
            }
        }

        return numberFound;
    }

    [Result(1886)]
    [TestCase(result: 9)]
    public static long GetAnswer2(string[] input)
    {
        var numberFound = 0;
        for (var y = 1; y < input.Length -1; y++)
        {
            for (var x = 1; x < input[y].Length -1; x++)
            {
                if (input[y][x] != 'A') continue;

                var topLeft = input[y - 1][x - 1];
                if (topLeft == 'M' && input[y + 1][x + 1] == 'S' || topLeft == 'S' && input[y + 1][x + 1] == 'M')
                {
                    var topRight = input[y - 1][x + 1];
                    if (topRight == 'M' && input[y + 1][x - 1] == 'S' || topRight == 'S' && input[y + 1][x - 1] == 'M')
                    {
                        numberFound++;
                    }
                }
            }
        }

        return numberFound;
    }
}