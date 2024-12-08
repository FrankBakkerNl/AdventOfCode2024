﻿using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using AdventOfCode2024.Framework;
using static System.Console;
using static System.ConsoleColor;

namespace AdventOfCode2024;

public class Program
{
    static void Main()
    {
        WindowsClipboard.SetText("");
        var totalTime = Stopwatch.StartNew();
        RunAll();
        WriteLine();
        WriteLine("Total:                 {0,12:##,###.000} ms", totalTime.Elapsed.TotalMilliseconds);
        WriteLine();
    }

    private static void RunAll()
    {

        WriteLine("Running tests");
        foreach (var answerMethod in PuzzleRefection.GetAnswerMethods())
        {
            RunTest(answerMethod);
        }

        WriteLine();
        WriteLine("Running real input");
        foreach (var answerMethod in PuzzleRefection.GetAnswerMethods())
        {
            PrintAnswer(answerMethod);
        }
        
        // WriteLine();
        // WriteLine("Running real input Optimized");
        // foreach (var answerMethod in PuzzleRefection.GetAnswerMethods(optimized:true))
        // {
        //     PrintAnswer(answerMethod);
        // }
    }

    private static void PrintAnswer(MethodInfo methodInfo)
    {
        Write($"{methodInfo.DeclaringType?.Name[^2..].TrimStart('0'),2}.{methodInfo.Name[^1..]} => ");

        var stopwatch = Stopwatch.StartNew();
        var result = GetAnswer(methodInfo);
        stopwatch.Stop();

        // Keep the last result on the clipboard
        WindowsClipboard.SetText(result?.ToString()!);

        var expectedResult = methodInfo.GetCustomAttribute<ResultAttribute>()?.Result;

        PrintResult(expectedResult, result);
            
        BackgroundColor = Black;
        ForegroundColor = Gray;
        WriteLine(string.Format(CultureInfo.InvariantCulture, "{0,12:##,##0.000} ms", stopwatch.Elapsed.TotalMilliseconds ));
    }

    private static void PrintResult(object? expectedResult, object? result)
    {
        if (expectedResult != null)
        {
            if ((dynamic?)expectedResult == (dynamic?)result)
            {
                BackgroundColor = DarkGreen;
                ForegroundColor = White;
                Write("{0,15}", result);
            }
            else
            {
                BackgroundColor = Red;
                ForegroundColor = White;
                Write("{0,15}", result);

                BackgroundColor = Black;
                ForegroundColor = DarkGray;
                Write(" Expected is " + expectedResult);
            }
        }
        else
        {
            BackgroundColor = DarkGray;
            ForegroundColor = White;
            Write("{0,15}", result);
            WindowsClipboard.SetText(result?.ToString()!);
        }
        BackgroundColor = Black;
        ForegroundColor = Gray;
    }

    private static object? GetAnswer(MethodInfo methodInfo)
    {
        var instance = methodInfo.IsStatic ? null : Activator.CreateInstance(methodInfo.DeclaringType!);
        var input = InputDataManager.GetInputArgs(methodInfo, "input");

        return methodInfo.Invoke(instance, input);
    }
        
    private static void RunTest(MethodInfo methodInfo)
    {
        var testAttribute = methodInfo.GetCustomAttribute<TestCase>();
        if (testAttribute == null) return;
            
        var instance = methodInfo.IsStatic ? null : Activator.CreateInstance(methodInfo.DeclaringType!);

        var fileName = testAttribute.Filename ??
                       methodInfo.Name switch
                       {
                           "GetAnswer1" => "ExampleInput1",
                           "GetAnswer2" => "ExampleInput2",
                           _ => "ExampleInput"
                       };
        var input = InputDataManager.GetInputArgs(methodInfo, fileName);

        var result =  methodInfo.Invoke(instance, input);

        Write($"{methodInfo.DeclaringType?.Name[^2..].TrimStart('0'),2}.{methodInfo.Name[^1..]} => ");
        PrintResult(testAttribute.Result, result);
        WriteLine();
            
    }
}