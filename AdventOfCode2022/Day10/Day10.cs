using System.Text;

namespace AdventOfCode2022;

internal class Day10
{
    private abstract class Instruction
    {
        public abstract int NumCycles { get; }
    }

    private class NoopInstruction : Instruction
    { 
        public override int NumCycles => 1;
    }

    private class AddInstruction : Instruction
    {
        public override int NumCycles => 2;
        public int Value { get; set; }
    }

    private class InstructionExecution
    {
        private Instruction _instruction;
        private int _cyclesLeft;

        public InstructionExecution(Instruction instruction)
        {
            _instruction = instruction;
            _cyclesLeft = instruction.NumCycles;
        }

        public Instruction Instruction => _instruction;
        public int CyclesLeft => _cyclesLeft;

        public void BeginCycle() { _cyclesLeft--; }
    }

    private Dictionary<int, int> _RegisterHistoryValues = new Dictionary<int, int>(); // value at the beginning of the cycle
    
    private readonly int CRT_WIDTH = 40;
    private readonly int CRT_HEIGHT = 6;

    public int GetResult(string filename, bool firstTask = true)
    {
        var lines = File.ReadAllLines(@$"{GetType().Name}\{filename}");

        var instructions = ParseInstructions(lines);

        // queue instructions
        var queue = new Queue<Instruction>();
        instructions.ToList().ForEach(instruction => queue.Enqueue(instruction));

        // execute queue
        ExecuteInstructions(queue);
        //PrintRegisterHistory();

        if (firstTask)
        {
            return _RegisterHistoryValues
                .Where(r => new List<int>() { 20, 60, 100, 140, 180, 220 }.Contains(r.Key))
                .Select(r => r.Key * r.Value)
                .Sum();
        }
        else
        {
            var sb = new StringBuilder();

            for (int line = 0; line < CRT_HEIGHT; line++)
            {
                for (int pos = 0; pos < CRT_WIDTH; pos++)
                {
                    var registerIndex = pos + (line * 40) + 1;

                    var registerValue = _RegisterHistoryValues[registerIndex];

                    if (pos >= registerValue - 1 && pos <= registerValue + 1) sb.Append("#");
                    else sb.Append(".");
                }

                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());

            // no result, just printing to the console to get the answer
            return 0;
        }
    }

    private IEnumerable<Instruction> ParseInstructions(IList<string> instructionLines)
    {
        foreach (var line in instructionLines)
        {
            var parts = line.Split(' ');

            switch (parts[0])
            {
                case "noop":
                    yield return new NoopInstruction();
                    break;
                case "addx":
                    yield return new AddInstruction() { Value = int.Parse(parts[1]) };
                    break;
                default:
                    throw new NotSupportedException($"Instruction {parts[0]} not supported");
            }
        }
    }

    private void ExecuteInstructions(Queue<Instruction> instructions)
    {
        var cycle = 0;
        int registerValue = 1;

        InstructionExecution currentExecution = null;

        while (instructions.Any())
        {
            cycle++;            
            _RegisterHistoryValues[cycle] = registerValue; // save register value at the beginning of the cycle

            if (currentExecution == null || currentExecution.CyclesLeft == 0)
                currentExecution = new InstructionExecution(instructions.Dequeue()); // dequeue a new instruction

            currentExecution.BeginCycle();

            if (currentExecution.Instruction is NoopInstruction)
            {
                continue;
            }
            else if (currentExecution.Instruction is AddInstruction addInstruction)
            {
                if (currentExecution.CyclesLeft == 0) registerValue += addInstruction.Value; // if cycles completed, we apply the value
            }
            else throw new NotSupportedException($"Instruction {currentExecution.Instruction.GetType()} not supported");
        }
    }

    private void PrintRegisterHistory()
    {
        foreach (var registerValue in _RegisterHistoryValues)
        {
            Console.WriteLine($"[{registerValue.Key}] = {registerValue.Value}");
        }
    }
}
