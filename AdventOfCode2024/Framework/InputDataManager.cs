﻿using System.IO;
using System.Reflection;

namespace AdventOfCode2024.Framework;

public class InputDataManager
{
    public static object[]? GetInputArgs(MethodInfo method, string fileName = "input")
    {
        var filePath = $@"..\..\..\Puzzles\D{method.DeclaringType?.Name[3..5]}\{fileName}.txt";

        var param = method.GetParameters().FirstOrDefault();
        if (param == null)
        {
            return null;
        }

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
            throw new Exception($"Input file {filePath} has been created");
        }

        var parameterType = param.ParameterType;
        
        if (parameterType == typeof(string)) return new object [] {File.ReadAllText(filePath)};

        if (parameterType == typeof(string[])) return new object [] {File.ReadAllLines(filePath)};

        if (parameterType == typeof(int[]))
            return new object[]{ File.ReadAllLines(filePath).Select(int.Parse).ToArray()};

        if (parameterType == typeof(long[]))
            return new object[] { File.ReadAllLines(filePath).Select(long.Parse).ToArray() };

        throw new InvalidOperationException($"Unable to map input data for {method.DeclaringType?.Name}.{method.Name}");
    }
}