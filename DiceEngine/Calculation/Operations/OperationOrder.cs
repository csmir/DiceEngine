using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceEngine.Calculation.Operations
{
    /// <summary>
    ///     
    /// </summary>
    public enum OperationOrder
    {
        /// <summary>
        ///     
        /// </summary>
        Exp,

        /// <summary>
        ///     
        /// </summary>
        MulDivRem,

        /// <summary>
        ///     
        /// </summary>
        AddSub,
    }

    /// <summary>
    ///     
    /// </summary>
    public static class OperationOrderExtensions
    {
        /// <summary>
        ///     
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static OperationOrder GetOperationOrder(this Operand op)
            => op switch
            {
                Operand.Exp => OperationOrder.Exp,
                Operand.Mul or Operand.Div or Operand.Rem => OperationOrder.MulDivRem,
                Operand.Add or Operand.Sub => OperationOrder.AddSub,
                _ => throw new InvalidOperationException(),
            };
    }
}
