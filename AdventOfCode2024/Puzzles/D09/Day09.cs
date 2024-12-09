namespace AdventOfCode2024.Puzzles.DayXX;

/// <summary>
/// https://adventofcode.com/2024/day/09
/// </summary>
public class Day09
{
    //[Result(0)] 2073992045 to low  3481838364193 to low
    [TestCase(result: 60, "ExampleInput1a")]
    [TestCase(result: 1928)]
    [Focus]
    public static long GetAnswer1(string input)
    {
        // Avoid parsing the entire sting into integers,
        // we only need each value at most once, so we will fetch and convert as needed from the original string
        int GetValue(int index) => input[index] - '0';

        var resultAccumulator = 0L;
        var leftBlockId = 0L;
        var leftIndex = 0;
        var leftFileId = 0L;
        var rightIndex = input.Length-1;
        var rightFileId =  input.Length /2 +1; // start +1 so we can decrement every loop
        var remainingRightBlocks = 0L;

        while (leftFileId < rightFileId) 
        {
            // Get size of next unmoved file
            int blockCountLeft = GetValue(leftIndex++);

            resultAccumulator += GetSegmentChecksum(leftBlockId, leftFileId, blockCountLeft);
            leftFileId++;

            leftBlockId += blockCountLeft;
            
            // new we have a gap, take segments from the right
            var leftGapCount = GetValue(leftIndex++);
            while (leftGapCount > 0 && leftFileId < rightFileId)
            {
                if (remainingRightBlocks == 0)
                {
                    remainingRightBlocks = GetValue(rightIndex);
                    rightIndex-=2; // skip the gap on the right, its value is irrelevant
                    rightFileId--;
                }

                int toMove = (int)Min(remainingRightBlocks, leftGapCount); 
                resultAccumulator += GetSegmentChecksum(leftBlockId, rightFileId, toMove);
                leftBlockId += toMove;
                remainingRightBlocks -= toMove; // leave the rest for the next gap
                leftGapCount -= toMove;
            }
        }
        resultAccumulator += GetSegmentChecksum(leftBlockId, rightFileId, remainingRightBlocks);

        return resultAccumulator;
    }

    static long GetSegmentChecksum(long startBlockId, long fileId, long blockCount)
    {
        var endBlockId = startBlockId + blockCount - 1;
        return (startBlockId + endBlockId) * blockCount * fileId /2 ;
    }

    
    //[Result(0)]
    [TestCase(result: 0)]
    public static long GetAnswer2(string[] input)
    {
        return 0;
    }
}