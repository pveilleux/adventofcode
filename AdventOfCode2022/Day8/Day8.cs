using System.Linq;
using static AdventOfCode2022.Day8;

namespace AdventOfCode2022;

internal class Day8
{
    public enum ScenicViewDirection
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public class ScenicView
    {
        public ScenicViewDirection Direction { get; set; }
        public int NumTrees { get; set; }
        public bool IsClear { get; set; } // we see up to the border
    }

    public class Tree
    {
        public int Height { get; set; }
        public int Row { get; set; } // use row/col names for simplicity
        public int Col { get; set; }
        public List<ScenicView> ScenicViews { get; set; } = new List<ScenicView>();
        public int ScenicScore => ScenicViews.Aggregate(1, (x, y) => x * y.NumTrees); // multiply all values
    }

    private int[][] _TreeGrid;

    public int GetResult(string filename, bool firstTask = true)
    {
        // parse input
        var lines = File.ReadAllLines(@$"{GetType().Name}\{filename}").Where(l => !string.IsNullOrEmpty(l));
        _TreeGrid = lines.Select(x => x.ToCharArray().Select(c => c - '0').ToArray()).ToArray();

        var width = _TreeGrid.Length; // we assume a square for now
        var trees = new List<Tree>();

        for (int row = 0; row < width; row++)
        {
            for (int col = 0; col < width; col++)
            {
                var height = _TreeGrid[row][col];

                // new tree analysis
                var tree = new Tree() { Row = row, Col = col, Height = height };
                trees.Add(tree);

                tree.ScenicViews.Add(GetScenicView(tree, ScenicViewDirection.Top));
                tree.ScenicViews.Add(GetScenicView(tree, ScenicViewDirection.Right));
                tree.ScenicViews.Add(GetScenicView(tree, ScenicViewDirection.Bottom));
                tree.ScenicViews.Add(GetScenicView(tree, ScenicViewDirection.Left));
            }
        }

        if (firstTask)
            return trees.Where(t => t.ScenicViews.Any(v => v.IsClear)).Count();
        else
            return trees.Max(t => t.ScenicScore);
    }

    private ScenicView GetScenicView(Tree tree, ScenicViewDirection direction)
    {
        var scenicView = new ScenicView() { Direction = direction };

        // int = number of trees, bool = if we see up to the border
        (int, bool)? calculationResult = null;

        switch (direction)
        {
            case ScenicViewDirection.Top:
                calculationResult = CalculateScenicViewTop(tree);
                break;
            case ScenicViewDirection.Right:
                calculationResult = CalculateScenicViewRight(tree);
                break;
            case ScenicViewDirection.Bottom:
                calculationResult = CalculateScenicViewBottom(tree);
                break;
            case ScenicViewDirection.Left:
                calculationResult = CalculateScenicViewLeft(tree);
                break;
        }

        scenicView.NumTrees = calculationResult.Value.Item1;
        scenicView.IsClear = calculationResult.Value.Item2;

        return scenicView;
    }

    private (int, bool) CalculateScenicViewTop(Tree tree)
    {
        var numberOfTrees = 0;
        var isClear = true;

        for (int i = tree.Row - 1; i >= 0; i--)
        {
            numberOfTrees++;

            if (_TreeGrid[i][tree.Col] >= tree.Height)
            {
                isClear = false;
                break;
            }
        }

        return (numberOfTrees, isClear);
    }

    private (int, bool) CalculateScenicViewRight(Tree tree)
    {
        var numberOfTrees = 0;
        var isClear = true;

        for (int i = tree.Col + 1; i < _TreeGrid.Length; i++)
        {
            numberOfTrees++;

            if (_TreeGrid[tree.Row][i] >= tree.Height)
            {
                isClear = false;
                break;
            }
        }

        return (numberOfTrees, isClear);
    }

    private (int, bool) CalculateScenicViewBottom(Tree tree)
    {
        var numberOfTrees = 0;
        var isClear = true;

        for (int i = tree.Row + 1; i < _TreeGrid.Length; i++)
        {
            numberOfTrees++;

            if (_TreeGrid[i][tree.Col] >= tree.Height)
            {
                isClear = false;
                break;
            }
        }

        return (numberOfTrees, isClear);
    }

    private (int, bool) CalculateScenicViewLeft(Tree tree)
    {
        var numberOfTrees = 0;
        var isClear = true;

        for (int i = tree.Col - 1; i >= 0; i--)
        {
            numberOfTrees++;

            if (_TreeGrid[tree.Row][i] >= tree.Height)
            {
                isClear = false;
                break;
            }
        }

        return (numberOfTrees, isClear);
    }
}
