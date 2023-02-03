using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiceEngine.Calculation.Operations;

namespace DiceEngine.Calculation.Elements
{
    /// <summary>
    ///     
    /// </summary>
    public struct BaseElement : IElement
    {
        /// <inheritdoc/>
        public Operand Operand { get; set; }

        /// <inheritdoc/>
        public string Value { get; }

        /// <inheritdoc/>
        public Guid Id { get; }

        private BaseElement(Operand operand, string v)
        {
            Id = Guid.NewGuid();
            Operand = operand;
            Value = v;
        }

        /// <inheritdoc/>
        public OpResult Calculate(double baseValue)
            => Operand.Calculate(baseValue, double.Parse(Value));

        /// <summary>
        ///     
        /// </summary>
        /// <param name="op"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static BaseElement Create(Operand op, string v)
            => new(op, v);

        #region overrides
        public override string ToString()
            => $"BaseElement ({Operand} {Value})";

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is IElement element && element.Id == Id)
                return true;
            return false;
        }

        public override int GetHashCode()
            => Id.GetHashCode();

        public static bool operator ==(BaseElement left, BaseElement right)
            => left.Equals(right);

        public static bool operator !=(BaseElement left, BaseElement right)
            => !(left == right);
        #endregion
    }
}
