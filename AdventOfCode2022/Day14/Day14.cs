using System.Drawing;

namespace AdventOfCode2022;

internal class Day14
{
    private readonly Point SourcePoint = new Point(500, 0);

    private Dictionary<Point, ObjectType> _objects = new Dictionary<Point, ObjectType>();

    private int _floorRow;
    private bool _hasFloor = false;

    public long GetResult(string filename, bool firstTask = true)
    {
        var lines = File.ReadAllLines(@$"{GetType().Name}\{filename}");

        // Add rocks
        foreach (var line in lines)
        {
            var coordinates = line.Split(' ').Where(c => c != "->").Select(c => StringToPoint(c));
            if (!coordinates.Any()) continue;

            var from = coordinates.First();

            foreach (var to in coordinates.Skip(1))
            {
                var points = GetPointsBetween(from, to);
                foreach (var point in points)
                {
                    if (!_objects.ContainsKey(point))
                        _objects.Add(point, ObjectType.Rock);
                }

                from = to;
            }
        }

        // Set the floor row index
        _floorRow = _objects
            .Where(r => r.Value == ObjectType.Rock)
            .Select(r => r.Key.Y)
            .Max() + 2; // Margin bottom 2

        // Generate the floor (hard coded to 1000 col for now)
        if (!firstTask)
            for (int col = 0; col < 1000; col++) _objects.Add(new Point(col, _floorRow), ObjectType.Rock);

        // Drop sand until we cannot anymore
        while (true)
        {
            var dropTo = DropFrom(SourcePoint);

            if (dropTo == null) break; // Cannot drop anymore

            _objects.Add(dropTo.Value, ObjectType.Sand);

            if (dropTo == SourcePoint) break; // We reached the source point
        }

        //Print();

        return _objects.Count(o => o.Value == ObjectType.Sand);
    }

    private Point? DropFrom(Point currentPos)
    {
        if (currentPos.Y > _floorRow + 2) return null;

        // Drop one position vertically
        var dropPosition = new Point(currentPos.X, currentPos.Y + 1);

        if (CanDropTo(dropPosition))
            return DropFrom(dropPosition); // There is nothing, we can continue to drop
        else
        {
            dropPosition = new Point(currentPos.X - 1, currentPos.Y + 1); // Try diagonal left

            if (CanDropTo(dropPosition))
                return DropFrom(dropPosition);
            else
            {
                dropPosition = new Point(currentPos.X + 1, currentPos.Y + 1); // Try diagonal right

                if (CanDropTo(dropPosition))
                    return DropFrom(dropPosition);
                else
                    return currentPos; // Cannot drop anymore, we return that point
            }
        }
    }

    private bool CanDropTo(Point point)
    {
        return !_objects.ContainsKey(point);
    }

    public List<Point> GetPointsBetween(Point from, Point to)
    {
        var points = new List<Point>();

        var currentX = from.X;
        var currentY = from.Y;

        points.Add(new Point(currentX, currentY)); // Add the starting point

        // Loop until we reach the destination point
        while (currentX != to.X || currentY != to.Y)
        {
            // Only vertical and horizontal movements are supported (no diagonals)
            if (Math.Abs(to.X - currentX) > Math.Abs(to.Y - currentY))
                currentX += Math.Sign(to.X - currentX);
            else
                currentY += Math.Sign(to.Y - currentY);

            points.Add(new Point(currentX, currentY));
        }

        return points;
    }

    private Point StringToPoint(string coordinates)
    {
        var values = coordinates.Split(',');

        if (values.Length != 2) throw new Exception($"Invalid coordinates: {coordinates}");

        var x = int.Parse(values[0]);
        var y = int.Parse(values[1]);

        return new Point(x, y);
    }

    private void Print()
    {
        if (!_objects.Any()) return;

        var startX = _objects.Select(r => r.Key.X).Min() - 1; // Margin left 1
        var endX = _objects.Select(r => r.Key.X).Max() + 1; // Margin right 1
        var startY = 0;
        var endY = _floorRow;

        for (var row = startY; row <= endY; row++)
        {
            for (var col = startX; col <= endX; col++)
            {
                if (_objects.TryGetValue(new Point(col, row), out var obj))
                {
                    Console.Write(ObjectTypeToChar(obj));
                }
                else if (row == endY)
                    Console.Write("#");
                else
                {
                    Console.Write(".");
                }
            }

            Console.Write(Environment.NewLine);
        }
    }

    private char ObjectTypeToChar(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Rock: return '#';
            case ObjectType.Sand: return 'o';
            case ObjectType.Empty: return '.';
            default: throw new Exception($"Invalid object type: {type}");
        }
    }

    public enum ObjectType
    {
        Empty,
        Rock,
        Sand
    }
}
