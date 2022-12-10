namespace AdventOfCode2022;

internal class Day5
{
    public struct Move
    {
        public int Quantity { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }

    public string GetResult(string filename, bool use9001 = false)
    {
        var lines = File.ReadAllLines(@$"Day5\{filename}").ToList();

        var stacks = new List<Stack<char>>();
        var moves = new List<Move>();

        var separateIndex = lines.IndexOf("");

        // Create stacks
        var numStacks = lines.ElementAt(separateIndex - 1).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();

        for (int i = 0; i < numStacks; ++i)
            stacks.Add(new Stack<char>());

        // Read stacks
        foreach (var line in lines.Take(separateIndex - 1).Reverse()) // start from the bottom
        {
            for (int i = 0; i<numStacks; i++)
            {
                var index = (4 * i) + 1;
                var c = line.ElementAt(index);

                if (char.IsLetter(c))
                    stacks[i].Push(c);
            }
        }

        // Read moves
        foreach (var line in lines.Where(l => l.StartsWith("move")))
        {
            var parts = line.Split(new char[] { ' ' });

            var qty = int.Parse(parts[1]);
            var from = int.Parse(parts[3]);
            var to = int.Parse(parts[5]);

            moves.Add(new Move() { Quantity = qty, From = from, To = to });
        }

        // Apply moves
        foreach (var move in moves)
        {
            var fromStack = stacks.ElementAt(move.From - 1);
            var toStack = stacks.ElementAt(move.To - 1);

            if (!use9001)
            {
                for (int i = 0; i < move.Quantity; i++)
                {
                    toStack.Push(fromStack.Pop());
                }
            }
            else
            {
                var items = new List<char>();

                for (int i = 0; i < move.Quantity; i++)
                    items.Add(fromStack.Pop());

                foreach (var item in items.ToArray().Reverse())
                    toStack.Push(item);
            }
        }

        return string.Join("", stacks.Select(s => s.Peek()));
    }
}
