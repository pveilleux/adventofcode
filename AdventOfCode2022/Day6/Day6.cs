namespace AdventOfCode2022;

internal class Day6
{
    public int GetResult(string filename, bool firstTask = true)
    {
        var num = firstTask ? 4 : 14;

        var lines = File.ReadAllLines(@$"{GetType().Name}\{filename}").ToList();

        var line = lines[0];

        for (int i=0; i<line.Length; i++)
        {
            var subs = line.Substring(i, num);

            var a = subs[0];
            var b = subs[1];
            var c = subs[2];
            var d = subs[3];
            var list = new List<char>() { a, b, c, d };

            if (!firstTask)
            {
                var e = subs[4];
                var f = subs[5];
                var g = subs[6];
                var h = subs[7];
                var ii = subs[8];
                var j = subs[9];
                var k = subs[10];
                var l = subs[11];
                var m = subs[12];
                var n = subs[13];
                list = new List<char>() { a, b, c, d, e, f, g, h, ii, j, k, l, m , n };
            }

            if (list.Distinct().Count() == num)
                return i + num;
        }

        return -1; // pas bon
    }
}


