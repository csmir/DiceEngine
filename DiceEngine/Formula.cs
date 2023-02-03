using DiceEngine.Elements;
using DiceEngine.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiceEngine
{
    /// <summary>
    ///     Represents a calculation formula for dice mathematics.
    /// </summary>
    public readonly struct Formula
    {
        private readonly static object _lock = new();

        /// <summary>
        ///     The value to be calculated.
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     The elements to be calculated.
        /// </summary>
        public IReadOnlyList<IElement> Elements { get; }

        private Formula(string v, IEnumerable<IElement> elements)
        {
            Value = v;
            Elements = elements.ToList();
        }

        /// <summary>
        ///     Calculates the formula from the given elements.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public OpResult Calculate(double a = 0d)
        {
            lock (_lock)
            {
                var report = $"Started calculating formula: {Value}\n";
                foreach (var element in Elements)
                {
                    var o = element.Calculate(a);
                    report += $"\n{o.Report}";
                    a = o.Output;
                }

                return OpResult.Create(a, report);
            }
        }

        /// <summary>
        ///     Attempts to parse a string input into a valid dice calculation.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static bool TryParse(string v, out Formula formula)
        {
            lock (_lock)
            {
                v = v.ToLowerInvariant()
                    .Replace(" ", "")
                    .Trim();

                formula = default;

                if (!v.IsValidCalculation())
                    return false;

                formula = new(v, v.CreateManyE().SortE());

                return formula.Elements.Any();
            }
        }
    }

    /// <summary>
    ///     Represents a number of extensions for creating formula's.
    /// </summary>
    public static partial class FormulaExtensions
    {
        private readonly static Regex _regex = MyRegex();

        /// <summary>
        ///     Checks if the formula is a valid calculation input.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsValidCalculation(this string v)
            => _regex.IsMatch(v);

        [GeneratedRegex("^[\\dd()+\\-*/^%]*$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
        private static partial Regex MyRegex();
    }
}
