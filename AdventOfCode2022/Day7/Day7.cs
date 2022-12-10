namespace AdventOfCode2022;

internal class Day7
{
    private Directory _rootDirectory = new Directory() { Name = "/" };
    private Directory _currentDirectory;

    public int GetResult(string filename, bool firstTask = true)
    {
        var lines = File.ReadAllLines(@$"{GetType().Name}\{filename}").ToList();

        foreach (var line in lines)
        {
            var parts = line.Split(' ');

            if (line.StartsWith("$"))
            {
                // my command
                var command = parts[1];

                if (command == "cd")
                {
                    ChangeDirectory(parts[2]);
                }
                else if (command == "ls")
                { 
                    // we assume all lines after won't have $
                }
            }
            else
            {
                // if no $, this is the result of the ls
                if (parts[0] == "dir")
                {
                    VerifyDirectory(parts[1]);
                }
                else
                {
                    VerifyFile(parts[1], int.Parse(parts[0])); 
                }
            }
        }

        //PrintStructure(_rootDirectory);

        if (firstTask)
            return GetTask1Result(_rootDirectory);
        else return 
                GetTask2Result();
    }

    private int GetTask1Result(Directory dir)
    {
        var allDirs = _rootDirectory.Directories.SelectManyRecursive(d => d.Directories);

        return allDirs.Where(d => d.Size <= 100000).Sum(d => d.Size);
    }

    private int GetTask2Result()
    {
        var total = 70000000;
        var required = 30000000;
        var unused = total - _rootDirectory.Size;

        var allDirs = _rootDirectory.Directories.SelectManyRecursive(d => d.Directories).OrderBy(d => d.Size);

        foreach (var dir in allDirs) // first one we hit is the good one
        {
            if (unused + dir.Size >= required)
                return dir.Size;
        }

        return -1; // not found
    }

    private void VerifyDirectory(string name)
    {
        var existing = _currentDirectory.Directories.SingleOrDefault(d => d.Name == name) != null;

        if (existing) return;

        _currentDirectory.Directories.Add(new Directory() { Name = name, Parent = _currentDirectory });
    }

    private void VerifyFile(string name, int size)
    {
        var existing = _currentDirectory.Files.SingleOrDefault(f => f.Name == name) != null;

        if (existing) return;

        _currentDirectory.Files.Add(new AFile() { Name = name, Size = size });
    }

    private void ChangeDirectory(string name)
    {
        if (name == "/")
        {
            _currentDirectory = _rootDirectory;
        }
        else if (name == "..")
        {
            _currentDirectory = _currentDirectory.Parent;
        }
        else
        {
            foreach (var dir in _currentDirectory.Directories)
            {
                if (dir.Name == name)
                {
                    _currentDirectory = dir;
                    break;
                }
            }
        }
    }

    private void PrintStructure(Directory dir, int level = 0)
    {
        var padding = string.Empty.PadRight(level * 3, ' ');

        foreach (var subDir in dir.Directories)
        {
            Console.WriteLine($"{padding}{subDir.Name} (dir) ({subDir.Size})");
            PrintStructure(subDir, level + 1);
        }

        foreach (var subFile in dir.Files)
        {
            Console.WriteLine($"{padding}{subFile.Name} ({subFile.Size})");
        }
    }

    public class AFile
    {
        public string Name { get; set; }
        public int Size { get; set; }
    }

    public class Directory
    {
        public Directory Parent { get; set; }

        public string Name { get; set; }
        public int Size => Files.Sum(f => f.Size) + Directories.Sum(d => d.Size);

        public IList<AFile> Files { get; set; } = new List<AFile>();
        public IList<Directory> Directories { get; set; } = new List<Directory>();
    }
}

public static class Ext
{
    public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (selector == null) throw new ArgumentNullException("selector");

        return !source.Any() ? source :
            source.Concat(
                source
                .SelectMany(i => selector(i).EmptyIfNull())
                .SelectManyRecursive(selector)
            );
    }

    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
    {
        return source ?? Enumerable.Empty<T>();
    }
}