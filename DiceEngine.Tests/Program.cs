using DiceEngine;

while (true)
{
    // Get input
    var input = Console.ReadLine()!;

    // Parse formula
    if (!Formula.TryParse(input, out var formula))
        Console.WriteLine("Calculation has invalid format.");

    // Calculate formula
    var output = formula.Calculate();

    Console.WriteLine(output.Report);
    Console.WriteLine(output.Output);
}