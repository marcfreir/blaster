using System;
using System.Collections.Generic;
using Blaster.CodeReview.Syntax;

namespace Blaster.CodeReview.Binding
{

    internal sealed class Binder
    {
        private readonly List<string> _diagnostics = new List<string>();
        public IEnumerable<string> Diagnostics => _diagnostics;
        public BoundExpression BindExpression(ExpressionSyntax expressionSyntax)
        {
            switch (expressionSyntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    {
                        return BindLiteralExpression((LiteralExpressionSyntax)expressionSyntax);
                    }
                case SyntaxKind.UnaryExpression:
                    {
                        return BindUnaryExpression((UnaryExpressionSyntax)expressionSyntax);
                    }
                case SyntaxKind.BinaryExpression:
                    {
                        return BindBinaryExpression((BinaryExpressionSyntax)expressionSyntax);
                    }
                default:
                    {
                        throw new Exception($"Unexpected syntax {expressionSyntax.Kind}");
                    }
            }
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax expressionSyntax)
        {
            var value = expressionSyntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax expressionSyntax)
        {
            var boundOperand = BindExpression(expressionSyntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(expressionSyntax.OperatorToken.Kind, boundOperand.Type);
            
            if (boundOperator == null)
            {
                _diagnostics.Add($"Unary operator <<{expressionSyntax.OperatorToken.Text}>> is not defined for type {boundOperand.Type}.");
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax expressionSyntax)
        {
            var boundLeftSide = BindExpression(expressionSyntax.LeftSide);
            var boundRightSide = BindExpression(expressionSyntax.RightSide);
            var boundOperator = BoundBinaryOperator.Bind(expressionSyntax.OperatorToken.Kind, boundLeftSide.Type, boundRightSide.Type);
            
            if (boundOperator == null)
            {
                _diagnostics.Add($"Binary operator <<{expressionSyntax.OperatorToken.Text}>> is not defined for types {boundLeftSide.Type} and {boundRightSide.Type}.");
                return boundLeftSide;
            }

            return new BoundBinaryExpression(boundLeftSide, boundOperator, boundRightSide);
        }
    }
}