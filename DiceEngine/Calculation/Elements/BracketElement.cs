using DiceEngine.Calculation.Operations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceEngine.Calculation.Elements
{
    /// <summary>
    ///     
    /// </summary>
    public struct BracketElement : IElement
    {
        /// <inheritdoc/>
        public Operand Operand { get; set; }

        /// <inheritdoc/>
        public string Value { get; }

        /// <inheritdoc/>
        public Guid Id { get; }

        /// <summary>
        ///     
        /// </summary>
        public IReadOnlyList<IElement> Elements { get; }

        private BracketElement(string v, Operand op, IEnumerable<IElement> elements)
        {
            Id = Guid.NewGuid();
            Value = v;
            Operand = op;
            Elements = elements.ToList();
        }

        /// <inheritdoc/>
        public OpResult Calculate(double a)
        {
            var b = 0d;

            var report = $"Started calculating bracket entry: {Value}\n";

            foreach (var element in Elements)
            {
                var ol = element.Calculate(b);
                report += $"{ol.Report}";
                b = ol.Output;
            }

            var og = Operand.Calculate(a, b);
            report += og.Report;
            a = og.Output;

            return OpResult.Create(a, report);
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="v"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static BracketElement Create(string v, Operand op)
        {
            v = v[1..^1];
            var elements = v.CreateManyE().SortE();

            return new(v, op, elements);
        }

        #region overrides
        public override string ToString()
            => $"BracketElement ({Operand} {Value}). Elements: {Elements.Count}";

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is IElement element && element.Id == Id)
                return true;
            return false;
        }

        public override int GetHashCode()
            => Id.GetHashCode();

        public static bool operator ==(BracketElement left, BracketElement right)
            => left.Equals(right);

        public static bool operator !=(BracketElement left, BracketElement right)
            => !(left == right);
        #endregion
    }
}
