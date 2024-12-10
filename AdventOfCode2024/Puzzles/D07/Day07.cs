using System.Diagnostics;

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
            .Where(equation => HasPossibleValueRecursive(equation.Result, equation.Terms, 0))
            .Sum(e => e.Result);
    }

    static bool HasPossibleValueRecursive(long expectedResult, ReadOnlySpan<int> remainingTerms, long register)
    {
        // Stop recursion
        if (remainingTerms.Length == 0) return register == expectedResult;
    
        if (register > expectedResult) return false; // values only increase so we can stop now

        var nextTerm = remainingTerms[0];

        var nextTerms = remainingTerms[1..];
        return HasPossibleValueRecursive(expectedResult, nextTerms, register == 0 ? nextTerm : register * nextTerm) ||
               HasPossibleValueRecursive(expectedResult, nextTerms, Concat(register, nextTerm)) ||
               HasPossibleValueRecursive(expectedResult, nextTerms, register + nextTerm);
    }

    static long Concat(long x, long y)
    {
        var remY = y;
        while (remY >= 10)
        {
            remY /= 10;
            x *= 10;
        }

        return x * 10 + y;
    }

    static Equation[] ParseInput(string[] input) => input.Select(ParseLine).ToArray();

    private static Equation ParseLine(string line)
    {
        var split = line.IndexOf(":", StringComparison.Ordinal);
        return new Equation(long.Parse(line[..split]),
            line[(split + 2) ..].Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray());
    }

    record struct Equation(long Result, int[] Terms);
}