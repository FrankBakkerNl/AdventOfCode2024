namespace AdventOfCode2024.Puzzles.DayXX;

/// <summary>
/// https://adventofcode.com/2024/day/09
/// </summary>
public class Day09
{
    [Result(6288599492129)]
    [TestCase(result: 60, "ExampleInput1a")]
    [TestCase(result: 1928)]
    public static long GetAnswer1(string input)
    {
        // Avoid parsing the entire string into integers,
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

    
    [Result(6321896265143)]
    [TestCase(result: 2858)]
    public static long GetAnswer2(string input)
    {
        var (files, gaps) = GetSegments(input);

        // For each gap size keep track of the lowest possible position it could fit
        Span<int> gapSearchStartValue = stackalloc int[10];

        var accumulator = 0L;

        for (var fileId = files.Length - 1; fileId >= 0; fileId--)
        {
            var file = files[fileId];
            
            ref var fitGap = ref FindGap(gaps, file.Size, ref gapSearchStartValue[file.Size], fileId);
            
            if (fitGap == Block.Null)
            {
                // Does not fit in a gap, so take the Checksum of original position 
                accumulator += GetSegmentChecksum(file.BlockId, fileId, file.Size);
                continue;
            }

            // Calculate the Checksum for the new position
            accumulator += GetSegmentChecksum(fitGap.BlockId, fileId, file.Size);

            // Update gap in place to new position and remaining size
            fitGap = new Block(fitGap.BlockId + file.Size, fitGap.Size - file.Size);
        }

        return accumulator;
    }

    static ref Block FindGap(Block[] gaps, int fileSize, ref int startSearch, int stopSearch)
    {
        for (; startSearch < stopSearch; startSearch++)
        {
            ref var gap = ref gaps[startSearch];
            if (gap.Size >= fileSize) return ref gap;
        }

        return ref Block.Null;
    }

    record struct Block(int BlockId, int Size)
    {
        public static Block Null = new (0, 0);
    }
    
    static (Block[] files, Block[] gaps) GetSegments(string input)
    {
        var gaps = new Block[(input.Length - 1) / 2];
        var files = new Block[(input.Length + 1) / 2];
        
        var blockId = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var size = input[i] - '0';
            
            if (i % 2 == 0)
                files[i/2] = new (blockId, size);
            else
                gaps[i/2] = new (blockId, size);;
            
            blockId += size;
        }

        return (files, gaps);
    }
}