using DiceEngine.Calculation.Operations;
using System;
using System.ComponentModel.DataAnnotations;

namespace DiceEngine.Calculation.Elements
{
    /// <summary>
    ///     An element in a formula.
    /// </summary>
    public interface IElement
    {
        /// <summary>
        ///     The operand that applies to this element.
        /// </summary>
        public Operand Operand { get; set; }

        /// <summary>
        ///     The value of this element, to use in calculations.
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     The ID of this element, to use in sort comparisons.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        ///     Calculate the element's value against the provided value 'a'.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public OpResult Calculate(double a);

        /// <summary>
        ///     
        /// </summary>
        /// <returns></returns>
        public virtual OperationOrder GetOrder()
        {
            return Operand.GetOperationOrder();
        }
    }

    /// <summary>
    ///     A number of extensions for creating and sorting elements.
    /// </summary>
    public static class ElementExtensions
    {
        /// <summary>
        ///     Create an element from the provided value and operand.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static IElement CreateE(this string v, Operand op)
        {
            if (v.Contains('d'))
                return DiceElement.Create(op, v);
            else
                return BaseElement.Create(op, v);
        }

        /// <summary>
        ///     Create a number of elements from the provided formula.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<IElement> CreateManyE(this string v)
        {
            if (v.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(v));

            if (v.TryDetermineOperand(out _))
                v = v[1..];

            var bracket = "";
            var bracketBus = 0;

            var element = "";
            var operand = v.DetermineNextOperand();

            for (int i = 0; i < v.Length; i++)
            {
                // Append bracket
                if (v[i] == '(')
                {
                    bracket += v[i];
                    bracketBus++;
                }

                else if (v[i] == ')')
                {
                    bracket += v[i];
                    bracketBus--;
                }

                else if (bracketBus != 0)
                {
                    bracket += v[i];
                }
                // End append bracket

                // CreateE bracket
                if (bracketBus == 0 && bracket != "")
                {
                    if (!bracket[0].TryDetermineOperand(out var bracketOperand))
                        bracketOperand = Operand.Mul;

                    Console.WriteLine(bracket);
                    var bracketElement = BracketElement.Create(bracket, bracketOperand);
                    bracket = "";

                    yield return bracketElement;
                }
                // End create bracket

                // Create element
                if (bracketBus == 0 && bracket == "")
                {
                    if (v[i].TryDetermineOperand(out var newOperand))
                    {
                        if (element.Length is > 0)
                            yield return element.CreateE(operand);

                        operand = newOperand;
                        element = "";
                    }

                    if (!v[i].TryDetermineOperand(out _))
                    {
                        element += v[i];
                        if (i == v.Length - 1)
                            yield return element.CreateE(operand);
                    }
                }
                // End create element
            }
        }

        /// <summary>
        ///     Sorts a number of elements in the correct order.
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static IEnumerable<IElement> SortE(this IEnumerable<IElement> elements)
        {
            if (!elements.Any())
                return elements;

            var origin = elements.ToList();
            var sorted = elements.OrderBy(x => x.GetOrder()).ToList();

            var ioo = origin.IndexOf(sorted.First(x => x is not BracketElement));

            if (ioo is <= 0)
                return sorted;

            else
            {
                var move = origin[ioo - 1];
                var ios = sorted.IndexOf(move);

                sorted.RemoveAt(ios);
                sorted.Insert(ioo - 1, move);

                return sorted;
            }
        }
    }
}
