namespace JunkLang.Machine;

public record Function(int CallPosition, int Parameter) : IFlowStatement;
public record Logic (int CallPosition) : IFlowStatement;

public class Processor (int position)
{
    public int Position { get; private set; } = position;
    
    // Stores the registers available to the processor
    private readonly Dictionary<string, float> Registers = new()
    {
        ["#IREG"] = 0f,
        ["#FREG"] = 0f,
        ["#SWAP"] = 0f,
        ["#INTR"] = 0f
    };
    
    // Stores the labels and their positions
    private readonly Dictionary<string, int> Labels = new();
    
    // Stores the functions and their positions
    private Dictionary<string, int> Functions = new();
    
    // Stores the position of all call and logic instructions
    private Stack<IFlowStatement> DoneStack = new();

    public void Swap(string reg1, string reg2)
    {
        (Registers[reg1], Registers[reg2]) = (Registers[reg2], Registers[reg1]);
    }

    public void Copy(string reg1, string reg2)
    {
        Registers[reg2] = Registers[reg1];
    }

    public void Set(float value, string reg)
    {
        Registers[reg] = value;
    }

    public void Add(string left, string reg)
    {
        if (float.TryParse(left, out var value))
        {
            Registers[reg] += value;
            return;
        }

        Registers[reg] += Registers[left];
    }
    
    public void Sub(string left, string reg)
    {
        if (float.TryParse(left, out var value))
        {
            Registers[reg] -= value;
            return;
        }

        Registers[reg] -= Registers[left];
    }
    public void Mult(string left, string reg)
    {
        if (float.TryParse(left, out var value))
        {
            Registers[reg] *= value;
            return;
        }

        Registers[reg] *= Registers[left];
    }
    public void Div(string left, string reg)
    {
        if (float.TryParse(left, out var value))
        {
            Registers[reg] /= value;
            return;
        }

        Registers[reg] /= Registers[left];
    }
    public void SubReverse(string left, string reg)
    {
        if (float.TryParse(left, out var value))
        {
            Registers[reg] = value - Registers[reg];
            return;
        }

        Registers[reg] = Registers[left] - Registers[reg];
    }
    public void DivReverse(string left, string reg)
    {
        if (float.TryParse(left, out var value))
        {
            Registers[reg] = value / Registers[reg];
            return;
        }

        Registers[reg] = Registers[left] / Registers[reg];
    }

    public void Label(string name)
    {
        if (int.TryParse(name, out _)) return;
        Labels.Add(name, Position);
    }

    public void Goto(string name)
    {
        if (int.TryParse(name, out var value))
        {
            Position = value;
            return;
        }
        Position = Labels[name];
    }

    public void Func(string name)
    {
        Functions.Add(name, Position);
    }

    public void Call(string name, int parameter)
    {
        DoneStack.Push(new Function(Position, parameter));
        Position = Functions.GetValueOrDefault(name, -1);
    }

    public void IfEquals(string left, string right)
    {
        var intLeft = float.TryParse(left, out var a) ? a : Registers[left];
        var intRight = float.TryParse(right, out var b) ? b : Registers[right];
    }
}