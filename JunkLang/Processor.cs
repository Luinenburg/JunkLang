namespace JunkLang;

public class Processor
{
    private int position = 0;
    private readonly Dictionary<string, float> Registers = new Dictionary<string, float>();
    private readonly Dictionary<string, int> Labels = new Dictionary<string, int>();
    private Dictionary<string, int> Functions = new Dictionary<string, int>();
    private Queue<(string, int, int)> Calls = new Queue<(string, int, int)>();

    public Processor()
    {
        Registers.Add("#IREG", 0f);
        Registers.Add("#FREG", 0f);
        Registers.Add("#SWAP", 0f);
        Registers.Add("#INTR", 0f);
    }

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
        if (int.TryParse(name, out var value)) return;
        Labels.Add(name, position);
    }

    public void Goto(string name)
    {
        if (int.TryParse(name, out var value))
        {
            position = value;
            return;
        }
        position = Labels[name];
    }

    public void Func(string name)
    {
        Functions.Add(name, position);
    }

    public void Call(string name, int parameter)
    {
        Calls.Enqueue((name, position, parameter));
        position = Functions.GetValueOrDefault(name, -1);
    }
}