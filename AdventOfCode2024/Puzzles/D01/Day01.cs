namespace AdventOfCode2024.Puzzles.D01;

public class Day01
{

    [Result(2164381)]
    [TestCase(result: 11)]
    public static int GetAnswer1(string[] input)
    {
        var s = Parse(input);
        var left = s.Select(t => t.Item1 ).Order();
        var right = s.Select(t => t.Item2 ).Order();
        return left.Zip(right).Select(t => Abs(t.First - t.Second)).Sum();
    }


    [Result(20719933)]
    [TestCase(result: 31)]
    public static int GetAnswer2(string[] input)
    {
        var s = Parse(input);
        var left = s.Select(t => t.Item1 ).ToArray();
        var rLookup = s.Select(t => t.Item2 ).ToArray().ToLookup(i => i);
        return left.Select(l => l * rLookup[l].Count()).Sum();
    }

    static (int, int)[] Parse(string[] input)
    {
        return input.Select(i =>
        {
            var parts = i.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }).ToArray();
    }
}
