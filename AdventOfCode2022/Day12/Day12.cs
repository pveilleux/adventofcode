using System.Drawing;

namespace AdventOfCode2022;

internal class Day12
{
    private char[][]? _elevationsGrid = null;
    Dictionary<Point, int>? _distances = null;
    private Point _startPosition;
    private Point _endPosition;

    public long GetResult(string filename, bool firstTask = true)
    {
        var lines = File.ReadAllLines(@$"{GetType().Name}\{filename}");
        _elevationsGrid = lines.Select(l => l.ToArray()).ToArray();

        // Load elevations grid
        for (int x = 0; x < _elevationsGrid.Length; x++)
        {
            for (int y = 0; y < _elevationsGrid[0].Length; y++)
            {
                if (_elevationsGrid[x][y] == 'S')
                {
                    _startPosition = new Point(x, y);
                    _elevationsGrid[x][y] = 'a';
                }
                else if (_elevationsGrid[x][y] == 'E')
                {
                    _endPosition = new Point(x, y);
                    _elevationsGrid[x][y] = 'z';
                }
                // else the good elevation is already loaded in the grid
            }
        }

        _distances = new() { { _endPosition, 0 } };

        FindNearestPath(_endPosition);
        
        // Task 1
        if (firstTask)
            return _distances[_startPosition];

        // Task 2
        var result = int.MaxValue;

        for (int x = 0; x < _elevationsGrid.Length; x++)
        {
            for (int y = 0; y < _elevationsGrid[0].Length; y++)
            {
                var point = new Point(x, y);
                if (_elevationsGrid[x][y] == 'a' && _distances.ContainsKey(point))
                {
                    var distance = _distances[point];
                    if (distance < result) result = distance;
                }  
            }
        }

        return result;
    }

    void FindNearestPath(Point fromPosition)
    {
        if (_distances == null) throw new NullReferenceException();

        if (!_distances.ContainsKey(fromPosition)) throw new Exception("Position not found");

        var distance = _distances[fromPosition];

        // Try all neighbors (up, down, left, right)
        VisitNeighbor(fromPosition, new Point(fromPosition.X - 1, fromPosition.Y), distance);
        VisitNeighbor(fromPosition, new Point(fromPosition.X + 1, fromPosition.Y), distance);
        VisitNeighbor(fromPosition, new Point(fromPosition.X, fromPosition.Y - 1), distance);
        VisitNeighbor(fromPosition, new Point(fromPosition.X, fromPosition.Y + 1), distance);
    }

    void VisitNeighbor(Point fromPosition, Point newPosition, int currentDistance)
    {
        if (_elevationsGrid == null) throw new NullReferenceException();
        if (_distances == null) throw new NullReferenceException();

        // Don't process if outside the grid
        if (newPosition.X < 0 || newPosition.X >= _elevationsGrid.Length || newPosition.Y < 0 || newPosition.Y >= _elevationsGrid[0].Length)
            return;

        // Must be 1 elevation difference
        if (_elevationsGrid[newPosition.X][newPosition.Y] + 1 >= _elevationsGrid[fromPosition.X][fromPosition.Y])
        {
            if (!_distances.TryGetValue(newPosition, out var currentNeighborCost) || currentNeighborCost > currentDistance + 1)
            {
                _distances[newPosition] = currentDistance + 1;
                FindNearestPath(newPosition);
            }
        }
    }
}
