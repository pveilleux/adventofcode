namespace AdventOfCode2022;

internal class Day2
{
    public enum Pick
    {
        Rock = 1,
        Paper = 2,
        Scisor = 3
    }

    public int GetResult(string filename, bool useStrategy = false)
    {
        string[] lines = File.ReadAllLines(@$"Day2\{filename}");

        var totalPoints = 0;

        foreach (var line in lines)
        {
            var picks = line.Split(' ');
            var yourPick = ParsePick(picks[0]);
            var myPick = ParsePick(picks[1]);

            if (useStrategy)
            {
                if (myPick == Pick.Rock) myPick = GetLosingPick(yourPick); // must lose
                else if (myPick == Pick.Paper) myPick = yourPick; // must draw
                else if (myPick == Pick.Scisor) myPick = GetWinningPick(yourPick); // must win
            }

            var currentPoints = 0;
            
            if (myPick == yourPick) currentPoints = 3; // draw
            else if (IsWinning(myPick, yourPick)) currentPoints = 6; // win

            totalPoints += currentPoints;
            totalPoints += (int)myPick;
        }

        return totalPoints;
    }

    private Pick GetLosingPick(Pick yourPick)
    {
        if (yourPick == Pick.Rock) return Pick.Scisor;
        else if (yourPick == Pick.Paper) return Pick.Rock;
        else if (yourPick == Pick.Scisor) return Pick.Paper;

        throw new Exception();
    }

    private Pick GetWinningPick(Pick yourPick)
    {
        if (yourPick == Pick.Rock) return Pick.Paper;
        else if (yourPick == Pick.Paper) return Pick.Scisor;
        else if (yourPick == Pick.Scisor) return Pick.Rock;

        throw new Exception();
    }

    private bool IsWinning(Pick myPick, Pick youPick)
    {
        if (myPick == Pick.Rock && youPick == Pick.Scisor) return true;
        else if (myPick == Pick.Paper && youPick == Pick.Rock) return true;
        else if (myPick == Pick.Scisor && youPick == Pick.Paper) return true;

        return false;
    }

    private Pick ParsePick(string val)
    {
        switch (val)
        {
            case "A":
            case "X":
                return Pick.Rock;
            case "B":
            case "Y":
                return Pick.Paper;
            case "C":
            case "Z":
                return Pick.Scisor;
            default:
                throw new Exception();
        }
    }
}
