using System.Runtime.CompilerServices;

namespace AdventOfCode2024.Puzzles.DayXX;

/// <summary>
/// https://adventofcode.com/2024/day/05
/// </summary>
public class Day05
{
    [Result(5991)]
    [TestCase(result: 143)]
    public static int GetAnswer1(string[] input)
    {
        var (rulebook, updates) = ParseInput(input);

        return updates.Where(u => UpdateValid(u, rulebook)).Select(u => u[(u.Length - 1) / 2]).Sum();
    }

    static bool UpdateValid(int[] update, HashSet<(int, int)> rules)
    {
        for (var i = 0; i < update.Length -1; i++)
        {
            for (var i1 = i+1; i1 < update.Length; i1++)
            {
                if (!rules.Contains((update[i], update[i1]))) return false;
            }
        }

        return true;
    }
    
    [Result(5479)]
    [TestCase(result: 123)]
    public static long GetAnswer2(string[] input)
    {
        var (rulebook, updates) = ParseInput(input);
        var pageComparer = new PageComparer(rulebook);

        return updates
            .Where(u => !UpdateValid(u, rulebook))
            .Select(u => u.Order(pageComparer)
                .ElementAt((u.Length - 1) / 2))
            .Sum();
    }

    static (HashSet<(int, int)> orderingRules, int[][] updates) ParseInput(string[] input)
    {
        var rules = input.TakeWhile(i => i != "").Select(l => AsTuple(l.Split('|'))).ToHashSet();
        var updates = input[(rules.Count + 1) ..].Select(l => l.Split(',').Select(int.Parse).ToArray()).ToArray();
        return (rules, updates);
    }

    static (int, int) AsTuple(string[] ints) => (int.Parse(ints[0]), int.Parse(ints[1]));
    
    class PageComparer(HashSet<(int,int)> rules) : IComparer<int>
    {
        public int Compare(int x, int y) => rules.Contains((x,y)) ? 1 : -1;
    }
}

