using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Framework;

public class PuzzleRefection
{
    public static IEnumerable<MethodInfo> GetAnswerMethods(bool ignoreFocus = false, bool optimized = false)
    {
        var classes = optimized ? GetOptimizedPuzzleClasses() : GetPuzzleClasses();
        var methods = classes.SelectMany(GetAnswerMethods)
            .Where(m => m.GetCustomAttribute<SkipAttribute>() == null).ToList();
            
        if (ignoreFocus) return methods;

        var focusMethods = methods.Where(d => d.GetCustomAttribute<FocusAttribute>() != null).ToList();
        return focusMethods.Any() ? focusMethods : methods;
    }

    public static List<Type> GetPuzzleClasses() =>
        Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => Regex.IsMatch(t.Name, "^Day[0-9][0-9]$"))
            .OrderBy(t => t.Name).ToList();

    
    public static List<Type> GetOptimizedPuzzleClasses() =>
        Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => Regex.IsMatch(t.Name, "^Day[0-9][0-9]Optimized$"))
            .OrderBy(t => t.Name).ToList();
    
    private static IEnumerable<MethodInfo> GetAnswerMethods(Type puzzleClass) => 
        puzzleClass.GetMethods()
            .Where(m => m.Name.StartsWith("GetAnswer", StringComparison.OrdinalIgnoreCase));

}