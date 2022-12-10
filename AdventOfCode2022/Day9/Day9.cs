using System.Drawing;

namespace AdventOfCode2022;

internal class Day9
{
    private enum Direction
    {
        U, D, L, R
    }

    private class Instruction
    {
        public Direction Direction { get; set; }
        public int Steps { get; set; }

        public override string ToString() => $"{Direction}{Steps}";
    }

    private class Knot
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() => $"{X}, {Y}";
    }

    public int GetResult(string filename, bool firstTask = true)
    {
        var lines = File.ReadAllLines(@$"{GetType().Name}\{filename}");

        var instructions = new List<Instruction>();
        var tailLocations = new List<Point>();

        var numKnots = firstTask ? 2 : 10;

        var knots = new Knot[numKnots];

        for (int i = 0; i < knots.Length; i++)
        {
            knots[i] = new Knot();
        }

        // parse inputs
        foreach (var line in lines)
        {
            if (line.StartsWith("#")) continue;

            var parts = line.Split(' ');

            instructions.Add(
                new Instruction() 
                { 
                    Direction = (Direction)Enum.Parse(typeof(Direction), parts[0]), 
                    Steps = int.Parse(parts[1])
                });
        }

        // apply instructions
        foreach (var instruction in instructions)
        {
            //Console.WriteLine(instruction.ToString()); 

            for (int i = 0; i < instruction.Steps; i++)
            {
                // first we move the head
                var head = knots[0]; 

                switch (instruction.Direction)
                {
                    case Direction.U: { head.Y++; break; }
                    case Direction.R: { head.X++; break; }
                    case Direction.D: { head.Y--; break; }
                    case Direction.L: { head.X--; break; }
                }

                // then move each following knots
                for (int j = 0; j < knots.Length - 1; j++)
                {
                    var knotA = knots[j];
                    var knotB = knots[j + 1];

                    if (AreAway(knotA, knotB))
                    {
                        if (knotA.Y != knotB.Y) knotB.Y += Math.Sign(knotA.Y - knotB.Y);
                        if (knotA.X != knotB.X) knotB.X += Math.Sign(knotA.X - knotB.X);
                    }
                }

                // save tail position
                var tail = knots[knots.Length - 1];
                var tailLocation = new Point(tail.X, tail.Y);

                if (!tailLocations.Contains(tailLocation)) tailLocations.Add(tailLocation);
            }

            //PrintKnots(knots);
        }

        return tailLocations.Count();
    }

    private bool AreAway(Knot a, Knot b, int distance = 2)
    {
        if (Math.Abs(a.X - b.X) >= distance) return true;
        else if (Math.Abs(a.Y - b.Y) >= distance) return true;

        return false;
    }

    private void PrintKnots(Knot[] knots)
    {
        for (int i = 15; i > -15; i--)
        {
            for (int j = -15; j < 15; j++)
            {
                var found = false;
                for (int k = 0; k < knots.Length; k++)
                {
                    if (knots[k].X == j && knots[k].Y == i)
                    {
                        Console.Write(k == 0 ? "H" : k + 1);
                        found = true;
                        break;
                    }
                }
                if (!found) Console.Write(".");
            }
            Console.Write("\n");
        }

        Console.WriteLine(); Console.WriteLine();
    }
}
