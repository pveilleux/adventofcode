namespace AdventOfCode2022;

internal class Day4
{
    public int GetResult(string filename, bool onlyOverlap = false)
    {
        string[] lines = File.ReadAllLines(@$"Day4\{filename}");

        var total = 0;

        foreach (var line in lines)
        {
            var pairs = line.Split(',');

            if (pairs.Length != 2) throw new Exception();

            var first  = CreateList(pairs[0]);  
            var second  = CreateList(pairs[1]);

            if (onlyOverlap && IsOverlaps(first, second)) total++;
            else if (IsFullyContained(first, second)) total++;
        }

        return total;
    }

    private bool IsFullyContained(IList<int> first, IList<int> second)
    {
        if (first.Intersect(second).Count() == first.Count()) return true;
        else if (second.Intersect(first).Count() == second.Count()) return true;
        return false;
    }

    private bool IsOverlaps(IList<int> first, IList<int> second)
    {
        if (first.Intersect(second).Count() > 0) return true;
        return false;
    }

    private IList<int> CreateList(string list)
    {
        var spl = list.Split('-');

        if (spl.Length != 2) throw new Exception();

        var first = Int32.Parse(spl[0]);
        var last = Int32.Parse(spl[1]);

        var finalList = new List<int>();

        for (int i = first; i <= last; i++)
        {
            finalList.Add(i);
        }

        return finalList;
    }
}
