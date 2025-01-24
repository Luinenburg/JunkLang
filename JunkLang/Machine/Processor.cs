namespace JunkLang.Machine;

public record Function(int CallPosition, int Parameter) : IFlowStatement;
public record Logic (int CallPosition) : IFlowStatement;

public class Processor (int position)
{
    public int Position { get; private set; } = position;
    
    // Stores the registers available to the processor
    private readonly Dictionary<ERegisters, float> Registers = new()
    {
        [ERegisters.IntegerRegister] = 0f,
        [ERegisters.FloatRegister] = 0f,
        [ERegisters.SwapRegister] = 0f,
        [ERegisters.IntermediaryRegister] = 0f
    };
    
    // Stores the labels and their positions
    private readonly Dictionary<string, int> Labels = new();
    
    // Stores the functions and their positions
    private Dictionary<string, int> Functions = new();
    
    // Stores the position of all call and logic instructions
    private Stack<IFlowStatement> DoneStack = new();

    public static bool TryStringToReg(string register, out ERegisters value)
    {
        switch (register)
        {
            case "#IREG":
                value = ERegisters.IntegerRegister;
                return true;
            case "#FREG":
                value = ERegisters.FloatRegister;
                return true;
            case "#SWAP":
                value = ERegisters.SwapRegister;
                return true;
            case "#INTR":
                value = ERegisters.IntermediaryRegister;
                return true;
            default:
                value = ERegisters.NonRegister;
                return false;
        };
    }

    private bool TryStringToRegValue(string register, out float value)
    {
        switch (register)
        {
            case "#IREG":
                value = Registers[ERegisters.IntegerRegister];
                return true;
            case "#FREG":
                value = Registers[ERegisters.FloatRegister];
                return true;
            case "#SWAP":
                value = Registers[ERegisters.SwapRegister];
                return true;
            case "#INTR":
                value = Registers[ERegisters.IntermediaryRegister];
                return true;
            default:
                value = float.NaN;
                return false;
        };
    }

    public void Swap(ERegisters reg1, ERegisters reg2)
    {
        (Registers[reg1], Registers[reg2]) = (Registers[reg2], Registers[reg1]);
    }

    public void Copy(ERegisters reg1, ERegisters reg2)
    {
        Registers[reg2] = Registers[reg1];
    }

    public void Set(float value, ERegisters reg)
    {
        Registers[reg] = value;
    }

    public void Arithmetic(string left, ERegisters reg, Func<float, float, float> equation)
    {
        if (float.TryParse(left, out var value))
        {
            Registers[reg] += value;
            return;
        }

        if (TryStringToRegValue(left, out value)) Registers[reg] = equation(Registers[reg], value);
    }

    public void Label(string name)
    {
        if (int.TryParse(name, out _)) return;
        Labels.Add(name, Position);
    }

    public void Goto(string name)
    {
        Position = int.TryParse(name, out var value) ? value : Labels[name];
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
    
    public bool If(string left, string right, Func<float, float, bool> condition)
    {
        // This is the reason I don't hear from God very much anymore.
        var leftNum = float.TryParse(left, out var lresult) ? lresult : TryStringToRegValue(left, out var llresult) ? llresult : float.NaN;
        var rightNum = float.TryParse(left, out var rresult) ? rresult : TryStringToRegValue(right, out var rrresult) ? rrresult : float.NaN;
        
        if (leftNum is float.NaN || rightNum is float.NaN || !condition(leftNum, rightNum)) return false;
        DoneStack.Push(new Logic(Position));
        return true;
    }

    public void OutputRegister(ERegisters register)
    {
        Console.Write(Registers[register]);
    }

    public void InputRegister(ERegisters register)
    {
        Registers[register] = float.TryParse(Console.ReadLine(), out var b) ? b : Registers[register];
    }
}