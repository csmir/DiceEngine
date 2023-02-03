using DiceEngine.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceEngine.Operations
{
    /// <summary>
    ///     Represents the operand applied to the value.
    /// </summary>
    public enum Operand
    {
        /// <summary>
        ///     Exponent.
        /// </summary>
        Exp = 1,

        /// <summary>
        ///     Multiplication.
        /// </summary>
        Mul = 2,

        /// <summary>
        ///     Division.
        /// </summary>
        Div = 3,

        /// <summary>
        ///     Remainder Division.
        /// </summary>
        Rem = 4,

        /// <summary>
        ///     Addition.
        /// </summary>
        Add = 5,

        /// <summary>
        ///     Subtraction.
        /// </summary>
        Sub = 6,
    }

    /// <summary>
    ///     Represents extensions for operands.
    /// </summary>
    public static class OperandExtensions
    {
        /// <summary>
        ///     Calculates the values from an operand.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static OpResult Calculate(this Operand op, double a, double b)
            => op switch
            {
                Operand.Exp 
                    => OpResult.Create(Math.Pow(a, b), $"Raised {a} to the power of {b}.\n"),

                Operand.Mul 
                    => OpResult.Create((a is 0 ? 1 : a) * b, $"Multiplied {(a is 0 ? 1 : a)} by {b}.\n"),

                Operand.Div 
                    => OpResult.Create((a is 0 ? 1 : a) / b, $"Divided {(a is 0 ? 1 : a)} by {b}.\n"),

                Operand.Rem 
                    => OpResult.Create((a is 0 ? 1 : a) % b, $"Divided {(a is 0 ? 1 : a)} by {b} and output remainder.\n"),

                Operand.Add
                    => OpResult.Create(a + b, $"Added {b} to {a}.\n"),

                Operand.Sub 
                    => OpResult.Create(a - b, $"Subtracted {b} from {a}.\n"),

                _ => throw new NotImplementedException(),
            };

        /// <summary>
        ///     Tries to determine the operand in the first occurrence of this string.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool TryDetermineOperand(this string v, out Operand op)
        {
            switch (v[0])
            {
                case '^':
                    op = Operand.Exp;
                    return true;
                case '*':
                    op = Operand.Mul;
                    return true;
                case '/':
                    op = Operand.Div;
                    return true;
                case '%':
                    op = Operand.Rem;
                    return true;
                case '+':
                    op = Operand.Add;
                    return true;
                case '-':
                    op = Operand.Sub;
                    return true;
                case '(':
                case ')':
                    op = Operand.Mul;
                    return false;
                default:
                    op = Operand.Add;
                    return false;
            }
        }

        /// <summary>
        ///     Tries to find the next operator in the current string.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Operand DetermineNextOperand(this string v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                switch (v[i])
                {
                    case '*':
                    case '(':
                    case ')':
                        return Operand.Mul;
                    default:
                        continue;
                }
            }

            return Operand.Add;
        }

        /// <summary>
        ///     Tries to determine the operand from provided character.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool TryDetermineOperand(this char v, out Operand op)
        {
            switch (v)
            {
                case '^':
                    op = Operand.Exp;
                    return true;
                case '*':
                case '(':
                case ')':
                    op = Operand.Mul;
                    return true;
                case '/':
                    op = Operand.Div;
                    return true;
                case '%':
                    op = Operand.Rem;
                    return true;
                case '+':
                    op = Operand.Add;
                    return true;
                case '-':
                    op = Operand.Sub;
                    return true;
                default:
                    op = Operand.Add;
                    return false;
            }
        }
    }
}
