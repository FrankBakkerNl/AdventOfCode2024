namespace AdventOfCode2024.Framework;

public class ResultAttribute(object result) : Attribute
{
    public object Result { get; } = result;
}