using AdventOfCode2022;
using System.Runtime.CompilerServices;

Console.WriteLine("------------------------------------------");
Console.WriteLine($"Results");
Console.WriteLine("------------------------------------------");
Console.WriteLine("");
Console.WriteLine("Task 1");
Console.WriteLine("-----------");
Console.WriteLine("Test");
Console.WriteLine(new Day12().GetResult("input-test.txt", true));
Console.WriteLine("");
Console.WriteLine("Final");
Console.WriteLine(new Day12().GetResult("input-main.txt", true));

Console.WriteLine("");
Console.WriteLine("Task 2");
Console.WriteLine("-----------");
Console.WriteLine("Test");
Console.WriteLine(new Day12().GetResult("input-test.txt", false));
Console.WriteLine("");
Console.WriteLine("Final");
Console.WriteLine(new Day12().GetResult("input-main.txt", false));
