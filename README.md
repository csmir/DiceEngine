# DiceEngine

This is a mathematical engine that can calculate dice rolls alongside normal calculations.

## Usage

```cs
using DiceEngine;

while (true)
{
    var input = Console.ReadLine()!;

    if (!Formula.TryParse(input, out var formula))
        Console.WriteLine("Calculation has invalid format.");

    var output = formula.Calculate();

    Console.WriteLine(output.Report);
    Console.WriteLine(output.Output);
}
```

### ⚠️ Disclaimer

This engine is not perfect. While I lack no skill in programming, my mathematical experience lacks. If you find errors while using this program, feel free to open a PR or issue so it can be resolved. 
