using System;
using System.Collections.Generic;
using System.Linq;
using Blaster.CodeReview.Syntax;

namespace Blaster.CodeReview.Binding
{

    internal sealed class Binder
    {
        private readonly Dictionary<VariableSymbol, object> _variables;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public Binder(Dictionary<VariableSymbol, object> variables)
        {
            _variables = variables;
        }
        public DiagnosticBag Diagnostics => _diagnostics;


        public BoundExpression BindExpression(ExpressionSyntax expressionSyntax)
        {
            switch (expressionSyntax.Kind)
            {
                case SyntaxKind.ParenthesizedExpression:
                    {
                        return BindParenthesizedExpression((ParenthesizedExpressionSyntax)expressionSyntax);
                    }
                case SyntaxKind.LiteralExpression:
                    {
                        return BindLiteralExpression((LiteralExpressionSyntax)expressionSyntax);
                    }
                case SyntaxKind.NameExpression:
                    {
                        return BindNameExpression((NameExpressionSyntax)expressionSyntax);
                    }
                case SyntaxKind.AssignmentExpression:
                    {
                        return BindAssignmentExpression((AssignmentExpressionSyntax)expressionSyntax);
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

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax expressionSyntax)
        {
            return BindExpression(expressionSyntax.Expression);
        }


        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax expressionSyntax)
        {
            var value = expressionSyntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax expressionSyntax)
        {
            var name = expressionSyntax.IdentifierToken.Text;
            
            var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);

            if (variable == null)
            {
                _diagnostics.ReportUndefinedName(expressionSyntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }

            return new BoundVariableExpression(variable);
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax expressionSyntax)
        {
            var name = expressionSyntax.IdentifierToken.Text;
            var boundExpression = BindExpression(expressionSyntax.Expression);

            var existingVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);
            if (existingVariable != null)
                _variables.Remove(existingVariable);

            
            var variable = new VariableSymbol(name, boundExpression.Type);
            _variables[variable] = null;

            return new BoundAssignmentExpression(variable, boundExpression);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax expressionSyntax)
        {
            var boundOperand = BindExpression(expressionSyntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(expressionSyntax.OperatorToken.Kind, boundOperand.Type);
            
            if (boundOperator == null)
            {
                _diagnostics.ReportUndefinedUnaryOperator(expressionSyntax.OperatorToken.Span, expressionSyntax.OperatorToken.Text, boundOperand.Type);
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
                _diagnostics.ReportUndefinedBinaryOperator(expressionSyntax.OperatorToken.Span, expressionSyntax.OperatorToken.Text, boundLeftSide.Type, boundRightSide.Type);
                return boundLeftSide;
            }

            return new BoundBinaryExpression(boundLeftSide, boundOperator, boundRightSide);
        }
    }
}