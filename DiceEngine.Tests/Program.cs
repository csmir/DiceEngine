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