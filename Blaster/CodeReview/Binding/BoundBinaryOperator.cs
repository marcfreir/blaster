using System;
using Blaster.CodeReview.Syntax;

namespace Blaster.CodeReview.Binding
{
    internal sealed class BoundBinaryOperator
    {
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind binaryOperatorKind, Type type) 
        : this(syntaxKind, binaryOperatorKind, type, type, type)
        {
        }
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind binaryOperatorKind, Type operandType, Type resultType) 
        : this(syntaxKind, binaryOperatorKind, operandType, operandType, resultType)
        {
        }
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind binaryOperatorKind, Type leftType, Type rightType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            BinaryOperatorKind = binaryOperatorKind;
            LeftType = leftType;
            RightType = rightType;
            Type = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind BinaryOperatorKind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type Type { get; }

        private static BoundBinaryOperator[] _binaryOperators = 
        {
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.MultiplyToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.DivideToken, BoundBinaryOperatorKind.Division, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equality, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.ExclamationEqualsToken, BoundBinaryOperatorKind.Inequality, typeof(int), typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equality, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.ExclamationEqualsToken, BoundBinaryOperatorKind.Inequality, typeof(bool)),
        };

        public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
        {
            foreach (var operatorUnit in _binaryOperators)
            {
                if (operatorUnit.SyntaxKind == syntaxKind && operatorUnit.LeftType == leftType && operatorUnit.RightType == rightType)
                {
                    return operatorUnit;
                }
            }

            return null;
        }
    }
}