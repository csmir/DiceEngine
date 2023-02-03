using DiceEngine.Operations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiceEngine.Elements
{
    /// <summary>
    ///     
    /// </summary>
    public struct DiceElement : IElement
    {
        /// <inheritdoc/>
        public Operand Operand { get; set; }

        /// <inheritdoc/>
        public string Value { get; }

        /// <inheritdoc/>
        public Guid Id { get; }

        private DiceElement(Operand operand, string v)
        {
            Id = Guid.NewGuid();
            Operand = operand;
            Value = v;
        }

        /// <inheritdoc/>
        public OpResult Calculate(double a)
        {
            var v = Value;
            if (v.StartsWith('d'))
                v = v.Insert(0, "1");

            var split = v.Split('d');

            var rolls = int.Parse(split[0]);
            var report = "";

            foreach (var dice in split[1..])
            {
                var eyes = int.Parse(dice);

                var roll = rolls.CalculateDice(eyes);

                rolls = Convert.ToInt32(roll.Output);
                report += ("\n" + roll.Report);
            }

            var output = Operand.Calculate(a, rolls);
            output.AppendReport(report, true);

            return output;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="op"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static DiceElement Create(Operand op, string v)
            => new(op, v);

        #region overrides
        public override string ToString()
            => $"DiceElement ({Operand} {Value})";

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is IElement element && element.Id == Id)
                return true;
            return false;
        }

        public override int GetHashCode()
            => Id.GetHashCode();

        public static bool operator ==(DiceElement left, DiceElement right)
            => left.Equals(right);

        public static bool operator !=(DiceElement left, DiceElement right)
            => !(left == right);
        #endregion
    }

    /// <summary>
    ///     
    /// </summary>
    public static partial class DiceElementExtensions
    {
        private static readonly Random _random = Random.Shared;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="eyes"></param>
        /// <returns></returns>
        public static OpResult CalculateDice(this int amount, int eyes)
        {
            var report = $"Rolling a d{eyes} {amount} time(s)\n";

            var output = 0;
            for (int i = 0; i < amount; i++)
            {
                var result = _random.Next(1, eyes + 1);
                report += $"` {result} `, ";

                output += result;
            }
            report += $"\nOutput: {output}";

            return OpResult.Create(output, report);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static void CheckRoll(this string v)
        {
            if (!char.IsDigit(v[^1]))
                throw new InvalidOperationException("Only numbers and dice declarations are allowed in dice rolls.");
        }
    }
}
