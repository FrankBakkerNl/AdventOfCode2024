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

        for (var pattern = 0; pattern < maxPattern; pattern++)
        {
            if (HasValue(equation, pattern)) return true;
        }

        return false;
    }

    static bool HasValue(Equation equation, int operatorPattern)
    {
        var terms = equation.Terms;
        long register = terms[0];
        var mask = 1;

        for (var i = 1; i < terms.Length; i++)
        {
            if ((operatorPattern & mask) == 0) // means add
            {
                register += terms[i];
            }
            else
            {
                register *= terms[i];
            }

            if (register > equation.Result) return false;

            // move the mask
            mask <<= 1;
        }

        return register == equation.Result;
    }

    [Result(271691107779347)]
    [TestCase(result: 11387)]
    public static long GetAnswer2(string[] input)
    {
        var equations = ParseInput(input);
        
        return equations
            .Where(equation => HasPossibleValueRecursive(equation, 0, 0))
            .Sum(e => e.Result);
    }

    static bool HasPossibleValueRecursive(Equation equation, long register, int termsUsed)
    {
        // Stop recursion
        if (termsUsed == equation.Terms.Length) return register == equation.Result;
    
        if (register > equation.Result) return false; // values only increase so we can stop now

        var nextTerm = equation.Terms[termsUsed];

        return HasPossibleValueRecursive(equation, termsUsed == 0 ? nextTerm : register * nextTerm, termsUsed + 1) ||
               HasPossibleValueRecursive(equation, register + nextTerm, termsUsed + 1) ||
               HasPossibleValueRecursive(equation, Concat(register, nextTerm), termsUsed + 1);
    }

    static long Concat(long x, long y) => long.Parse(x.ToString() + y.ToString());

    static Equation[] ParseInput(string[] input) => input.Select(ParseLine).ToArray();

    private static Equation ParseLine(string line)
    {
        var split = line.IndexOf(":", StringComparison.Ordinal);
        return new Equation(long.Parse(line[..split]),
            line[(split + 2) ..].Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray());
    }

    record Equation(long Result, int[] Terms);
}