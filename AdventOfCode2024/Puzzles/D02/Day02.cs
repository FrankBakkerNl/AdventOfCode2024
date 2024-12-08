namespace AdventOfCode2024.Puzzles.DayXX;

/// <summary>
/// https://adventofcode.com/2024/day/xx
/// </summary>
public class Day02
{
    [Result(624)]
    [TestCase(result: 2)]
    public static int GetAnswer1(string[] input)
    {
        var a = Parse(input);
        return a.Count(IsValid);
    }

    static bool IsValid(int[] report)
    {
        var diffs = report.Skip(1).Select((v, i) => report[i] - v).ToArray();
        var rising = diffs.First() > 0;
        return !diffs.Any(d => d==0|| d > 3 || d < -3 || (d > 0 != rising));
    }

    [Result(658)]
    [TestCase(result: 4)]

    public static long GetAnswer2(string[] input)
    {
        var a = Parse(input);
        return  a.Count(IsValidDampened);
    }

    static bool IsValidDampened(int[] report)
    {
        for (var i = 0; i < report.Length; i++)
        {
            int[] s = [.. report[..i], .. report[(i+1) ..]];
            if (IsValid(s)) return true;
        }

        return false;
    }

    static IEnumerable<int[]> Parse(string[] input)
    {
        return input.Select(i =>
            i.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                .ToArray());
    }
}