namespace AdventOfCode2024.Puzzles.DayXX;

/// <summary>
/// https://adventofcode.com/2024/day/07
/// </summary>
public class Day07
{
    [Result(945512582195)]
    [TestCase(result: 3749)]
    public static long GetAnswer1(string[] input)
    {
        var equations = ParseInput(input);
        return equations.Where(FindPattern).Sum(e => e.Result);
    }

    static bool FindPattern(Equation equation)
    {
        var maxPattern = 1 << equation.Terms.Length - 1;
        
        for (int pattern = 0; pattern < maxPattern; pattern++)
        {
            if (HasValue(equation, pattern))
                return true;
        }

        return false;
    }

    static bool HasValue(Equation equation, int pattern)
    {
        long register = equation.Terms[0];
        int mask = 1;
        for (int i = 1; i < equation.Terms.Length; i++)
        {
            if ((pattern & mask) == 0) // means add
            {
                register += equation.Terms[i];
            }
            else
            {
                register *= equation.Terms[i];
            }

            if (register > equation.Result) return false;
            
            // move the mask
            mask <<= 1;
        }

        return register == equation.Result;
    }

    //[Result(0)]
    [TestCase(result: 0)]
    public static long GetAnswer2(string[] input)
    {
        return 0;
    }

    static Equation[] ParseInput(string[] input)
    {
        return input.Select(ParseLine).ToArray();
    }

    private static Equation ParseLine(string line)
    {
        var split = line.IndexOf(":", StringComparison.Ordinal);
        return new Equation(long.Parse(line[..split]),
            line[(split+2) ..].Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray());
    }


    record Equation(long Result, int[] Terms);
}