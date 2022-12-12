using System.Runtime.InteropServices;
using System.Text;
using static AdventOfCode2022.Day8;

namespace AdventOfCode2022;

internal class Day11
{
    public long GetResult(string filename, bool firstTask = true)
    {
        var lines = File.ReadAllLines(@$"{GetType().Name}\{filename}");

        var monkeys = ParseInput(lines);

        var numRounds = firstTask ? 20 : 10000;
        var commonMultiple = monkeys.Aggregate(1, (x, y) => x * (int)y.TestDivisibleBy);

        // Rounds
        for (int i = 0; i < numRounds; i++)
        {
            // Each monkey
            foreach (var monkey in monkeys)
            {
                // Each item
                foreach (var item in monkey.Items)
                {
                    monkey.InspectedItems++;

                    // We apply the operation to the initial worry level
                    var worryLevel = GetWorryLevel(item, monkey.ItemOperation);

                    // Monkey gets bored, we divide by 3 (rounded down)
                    if (firstTask)
                        worryLevel = worryLevel / 3;

                    // Test if divisible the monkey value
                    var isDivisible = worryLevel % (long)monkey.TestDivisibleBy == 0;

                    // Prevent getting to high
                    worryLevel = worryLevel % commonMultiple;

                    // Send to monkey
                    var toMonkey = isDivisible ? monkey.MonkeyThrowIfTrue : monkey.MonkeyThrowIfFalse;
                    monkeys[toMonkey].Items.Add(worryLevel);
                }

                monkey.Items.Clear(); // He thrown everything
            }
        }

        var top2 = monkeys.Select(m => m.InspectedItems).OrderDescending().Take(2);
        //monkeys.ToList().ForEach(m => Console.WriteLine($"Monkey {m.Id} = {m.InspectedItems}"));

        return (long)top2.ElementAt(0) * (long)top2.ElementAt(1);
    }

    private long GetWorryLevel(long value, ItemOperation operation)
    {
        switch (operation.Operation)
        {
            case Operation.Add:
                return value + operation.Value;
            case Operation.Multiply:
                return value * operation.Value;
            case Operation.Square:
                return value * value;
            default:
                throw new NotSupportedException($"Operation {operation.Operation} not supported");
        }
    }

    private IList<Monkey> ParseInput(string[] lines)
    {
        var monkeys = new List<Monkey>();
        Monkey currentMonkey = null;

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;

            if (line.StartsWith("Monkey"))
            {
                currentMonkey = new Monkey(monkeys.Count); // we assume they are in order
                monkeys.Add(currentMonkey);
            }
            else
            {
                if (currentMonkey == null) throw new Exception("No monkey created");

                var parts = line.Split(':');
                var infoType = parts[0].Trim();
                var infoContent = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                switch (infoType)
                {
                    case "Starting items":
                        foreach (var item in infoContent.ToList())
                            currentMonkey.Items.Add(int.Parse(item.Replace(",", "").Trim()));
                        break;
                    case "Operation":
                        var operation = StringToOperation(infoContent[3]);
                        var operationValue = StringToValue(infoContent[4]);
                        if (operationValue == -1) operation = Operation.Square; // hack for now :(
                        currentMonkey.ItemOperation = new ItemOperation() { Operation = operation, Value = operationValue };
                        break;
                    case "Test":
                        currentMonkey.TestDivisibleBy = int.Parse(infoContent[2]);
                        break;
                    case "If true":
                        currentMonkey.MonkeyThrowIfTrue = int.Parse(infoContent[3]);
                        break;
                    case "If false":
                        currentMonkey.MonkeyThrowIfFalse = int.Parse(infoContent[3]);
                        break;
                    default:
                        throw new NotSupportedException($"Unexpected token: {infoType}");
                }
            }
        }

        return monkeys;
    }

    public class Monkey
    {
        public int Id { get; set; }
        public IList<long> Items { get; set;} = new List<long>();
        public ItemOperation ItemOperation { get; set; }
        public long TestDivisibleBy { get; set; }
        public int MonkeyThrowIfTrue { get; set; }
        public int MonkeyThrowIfFalse { get; set; }

        public int InspectedItems { get; set; }

        public Monkey(int id)
        {
            Id = id;
        }
    }

    public class ItemOperation
    {
        public Operation Operation { get; set; }
        public long Value { get; set; }
    }

    public enum Operation
    {
        Add,
        Multiply,
        Square
    }

    private Operation StringToOperation(string str)
    {
        str = str.Trim();

        if (str == "*") return Operation.Multiply;
        else if (str == "+") return Operation.Add;
        else throw new NotSupportedException($"Operation {str} not supported");
    }

    private int StringToValue(string str)
    {
        if (str == "old") return -1; // old * old
        else return int.Parse(str);
    }
}
