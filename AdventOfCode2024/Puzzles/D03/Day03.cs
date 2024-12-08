using System.Text.RegularExpressions;

namespace AdventOfCode2024.Puzzles.DayXX;

/// <summary>
/// https://adventofcode.com/2024/day/xx
/// </summary>
public partial class Day03
{
    [Result(179834255)]
    [TestCase(result: 161)]
    public static int GetAnswer1(string input)
    {
        var expressions = GetMulExpressions(input);
        return expressions.Select(m => m.Item1 * m.Item2).Sum();
    }

    [Result(80570939)]
    [TestCase(result: 48)]
    public static long GetAnswer2(string input)
    {
        var expressions = GetAllExpressions(input).ToArray();
        var ints = expressions.Select(m => m.Item1 * m.Item2).ToArray();
        return ints.Sum();
    }

    static IEnumerable<(int, int)> GetMulExpressions(string input)
    {
        return MulRegex().Matches(input).Select(m => (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)));
    }
    
    static IEnumerable<(int, int)> GetAllExpressions(string input)
    {
        var doMode = true; 
        
        var matchCollection = MulDpDOntRegex().Matches(input);
        foreach (Match m in matchCollection)
        {
            if (m.Value == "do()")
            {
                doMode = true;
            }
            else if (m.Value == "don't()")
            {
                doMode = false;
            }
            else if (doMode)
            {
                yield return (int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));
            }
        }
    }
    

    [GeneratedRegex("mul\\((\\d+),(\\d+)\\)")]
    private static partial Regex MulRegex();
    
    [GeneratedRegex("(mul\\((\\d+),(\\d+)\\)|do\\(\\)|don't\\(\\))")]
    private static partial Regex MulDpDOntRegex();

    
    
}