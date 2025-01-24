namespace JunkLang.Machine;

public record Function(int CallPosition, int Parameter) : IFlowStatement;
public record Logic (int CallPosition) : IFlowStatement;

public class Processor (int position)
{
    public int Position { get; private set; } = position;
    
    // Stores the registers available to the processor
    private readonly Dictionary<ERegisters, float> Registers = new()
    {
        [Machine.ERegisters.IntegerRegister] = 0f,
        [Machine.ERegisters.FloatRegister] = 0f,
        [Machine.ERegisters.SwapRegister] = 0f,
        [Machine.ERegisters.IntermediaryRegister] = 0f
    };
    
    // Stores the labels and their positions
    private readonly Dictionary<string, int> Labels = new();
    
    // Stores the functions and their positions
    private Dictionary<string, int> Functions = new();
    
    // Stores the position of all call and logic instructions
    private Stack<IFlowStatement> DoneStack = new();

    public static ERegisters? StringToReg(string register)
    {
        return register switch
        {
            "#IREG" => Machine.ERegisters.IntegerRegister,
            "#FREG" => Machine.ERegisters.FloatRegister,
            "#SWAP" => Machine.ERegisters.SwapRegister,
            "#INTR" => Machine.ERegisters.IntermediaryRegister,
            _ => null
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
        
        var leftReg = StringToReg(left);
        if (leftReg is not null) Registers[reg] = equation(Registers[reg], Registers[(ERegisters) leftReg]);
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
    
    public bool If(string left, string right, Func<float, float, bool> condition)
    {
        // This is the reason I don't hear from God very much anymore.
        float? leftNumIsh = float.TryParse(left, out var lresult) ? lresult : null;
        float? rightNumIsh = float.TryParse(left, out var rresult) ? rresult : null;
        var leftReg = StringToReg(left);
        var rightReg = StringToReg(right);
        var leftNum = leftNumIsh ?? (leftReg is null ? null : Registers[(ERegisters) leftReg]);
        var rightNum = rightNumIsh ?? (rightReg is null ? null : Registers[(ERegisters) rightReg]);
        
        if (leftNum is null || rightNum is null || condition((float) leftNum, (float) rightNum)) return false;
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