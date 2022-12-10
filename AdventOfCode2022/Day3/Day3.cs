namespace AdventOfCode2022;

internal class Day3
{
    public int GetResult(string filename)
    {
        string[] lines = File.ReadAllLines(@$"Day3\{filename}");

        var sumPriorities = 0;

        foreach (var line in lines)
        {
            var items = line.ToList();

            var compartiment1 = items.GetRange(0, items.Count() / 2);
            var compartiment2 = items.GetRange(items.Count() / 2, items.Count() / 2);

            var sameItem = compartiment1.Intersect(compartiment2).Single(); // crash if not single

            sumPriorities += GetPriorityValue(sameItem);
        }        

        return sumPriorities;
    }

    public int GetResult2(string filename)
    {
        string[] lines = File.ReadAllLines(@$"Day3\{filename}");

        var sumPriorities = 0;

        var list = lines.ToList().Select(l => l.ToList()).ToList();
        var listLines = Split(list); // split by group of 3

        foreach (var groupLines in listLines)
        {
            if (groupLines.Count != 3) throw new Exception();

            var sameItem = groupLines.ElementAt(0).Intersect(groupLines.ElementAt(1).Intersect(groupLines.ElementAt(2))).Single(); // crash if not single

            sumPriorities += GetPriorityValue(sameItem);
        }

        return sumPriorities;
    }

    public static List<List<T>> Split<T>(IList<T> source)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / 3)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }

    private int GetPriorityValue(char c)
    {
        if (char.IsLower(c)) return (int)c - (int)'a' + 1;
        else return (int)c - (int)'A' + 27;
    }
}


//I have a list of lines. Each line has a list of characters. Each line must be split into 2 groups and we need to find which character is the same between the 2 lists.

//static void Main(string[] args)
//{
//    // Define the list of lines, where each line is a list of characters.
//    var lines = new List<List<char>>
//            {
//                new List<char> { 'a', 'b', 'c' },
//                new List<char> { 'd', 'e', 'f' },
//                new List<char> { 'g', 'h', 'i' }
//            };

//    // Loop through each line in the list.
//    foreach (var line in lines)
//    {
//        // Split the line into two groups of characters.
//        var group1 = line.Take(line.Count / 2);
//        var group2 = line.Skip(line.Count / 2);

//        // Use the Intersect() method to find the characters that are the same
//        // between the two groups.
//        var commonChars = group1.Intersect(group2);

//        // Print the common characters.
//        foreach (char c in commonChars)
//        {
//            Console.WriteLine(c);
//        }
//    }
//}

//A need a function that returns the value of a character.Characters can only be letters.Lowercase character values from a to z are 1 to 27. Uppercase
//characters from A to Z are 28 to 52.

//static void Main(string[] args)
//{
//    // Define some test characters.
//    char lowercaseA = 'a';
//    char lowercaseZ = 'z';
//    char uppercaseA = 'A';
//    char uppercaseZ = 'Z';

//    // Print the values of the test characters.
//    Console.WriteLine(GetCharacterValue(lowercaseA)); // Output: 1
//    Console.WriteLine(GetCharacterValue(lowercaseZ)); // Output: 26
//    Console.WriteLine(GetCharacterValue(uppercaseA)); // Output: 27
//    Console.WriteLine(GetCharacterValue(uppercaseZ)); // Output: 52
//}

//static int GetCharacterValue(char c)
//{
//    // Convert the character to lowercase or uppercase.
//    char lowercase = char.ToLower(c);
//    char uppercase = char.ToUpper(c);

//    // Convert the lowercase or uppercase character to a character array.
//    char[] lowercaseChars = lowercase.ToCharArray();
//    char[] uppercaseChars = uppercase.ToCharArray();

//    // Return the value of the character.
//    if (lowercaseChars[0] >= 'a' && lowercaseChars[0] <= 'z')
//    {
//        return lowercaseChars[0] - 'a' + 1;
//    }
//    else if (uppercaseChars[0] >= 'A' && uppercaseChars[0] <= 'Z')
//    {
//        return uppercaseChars[0] - 'A' + 28;
//    }
//    else
//    {
//        return 0;
//    }
//}
