using System;
using Blaster.CodeReview.Syntax;

namespace Blaster.CodeReview.Binding
{
    internal sealed class BoundUnaryOperator
    {
        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind unaryOperatorKind, Type operandType) 
        : this(syntaxKind, unaryOperatorKind, operandType, operandType)
        {
        }

        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind unaryOperatorKind, Type operandType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            UnaryOperatorKind = unaryOperatorKind;
            OperandType = operandType;
            ResultType = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind UnaryOperatorKind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }

        private static BoundUnaryOperator[] _unaryOperators = 
        {
            new BoundUnaryOperator(SyntaxKind.ExclamationToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int))
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type operandType)
        {
            foreach (var operatorUnit in _unaryOperators)
            {
                if (operatorUnit.SyntaxKind == syntaxKind && operatorUnit.OperandType == operandType)
                {
                    return operatorUnit;
                }
            }

            return null;
        }
    }
}