namespace AdventOfCode2022;

internal class Day1
{
    public class Elf
    {
        public ICollection<int> Calories { get; set; } = new List<int>();
        public int TotalCalories => Calories.Sum();
    }

    public int GetResult(string filename, int top = 1)
    {

        var lines = File.ReadAllLines(@$"Day1\{filename}").ToList();
        return SumOfGroups(lines).OrderByDescending(a => a).Take(top).Sum();

        //string[] lines = File.ReadAllLines(@$"Day1\{filename}");

        //var elves = new List<Elf>();

        //Elf currentElf = null;

        //foreach (var line in lines)
        //{
        //    if (currentElf == null) 
        //    { 
        //        currentElf = new Elf();
        //        elves.Add(currentElf);
        //    }

        //    if (string.IsNullOrEmpty(line))
        //    {
        //        currentElf = null;
        //        continue;
        //    }

        //    currentElf.Calories.Add(int.Parse(line));
        //}

        //return elves.OrderByDescending(e => e.TotalCalories).Take(top).Sum(e => e.TotalCalories);
    }

    // Define a function that calculates the sum of each group of numbers in a list
    public List<int> SumOfGroups(List<string> numbers)
    {
        // Create a list to store the sum of each group
        List<int> sums = new List<int>();

        // Keep track of the current sum and the current group
        int currentSum = 0;
        int currentGroup = 0;

        // Loop through each number in the list
        foreach (string number in numbers)
        {
            // If the number is an empty string, start a new group
            if (string.IsNullOrEmpty(number))
            {
                // Add the current sum to the list of sums
                sums.Add(currentSum);

                // Reset the current sum and group
                currentSum = 0;
                currentGroup++;
            }
            // Otherwise, add the number to the current sum
            else
            {
                currentSum += int.Parse(number);
            }
        }

        // Return the list of sums
        return sums;
    }


}


