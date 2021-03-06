﻿﻿using System;
using System.Collections.Generic;
using Blaster.CodeReview.Binding;

namespace Blaster.CodeReview
{

    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;
        private readonly Dictionary<VariableSymbol, object> _variables;

        public Evaluator(BoundExpression root, Dictionary<VariableSymbol, object> variables)
        {
            this._root = root;
            this._variables = variables;
            //
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            
            //Evaluate LiteralExpression
            if (node is BoundLiteralExpression boundLiteralExpression)
            {
                return boundLiteralExpression.Value;
            }

            if (node is BoundVariableExpression boundVariableExpression)
                return _variables[boundVariableExpression.Variable];

            if (node is BoundAssignmentExpression boundAssignmentExpression)
            {
                var value = EvaluateExpression(boundAssignmentExpression.Expression);
                _variables[boundAssignmentExpression.Variable] = value;
                return value;
            }

            //Evaluate UnaryExpression
            if (node is BoundUnaryExpression boundUnaryExpression)
            {
                var operand = EvaluateExpression(boundUnaryExpression.Operand);

                switch (boundUnaryExpression.OperatorUnit.UnaryOperatorKind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return (int) operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -(int) operand;
                    case BoundUnaryOperatorKind.LogicalNegation:
                        return !(bool) operand;
                    default:
                        throw new Exception($"Unexpected unary operator {boundUnaryExpression.OperatorUnit}");
                }
            }
            
            //Evaluate BinaryExpression
            if (node is BoundBinaryExpression boundBinaryExpression)
            {
                var leftSide = EvaluateExpression(boundBinaryExpression.LeftSide);
                var rightSide = EvaluateExpression(boundBinaryExpression.RightSide);

                switch (boundBinaryExpression.OperatorUnit.BinaryOperatorKind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return (int) leftSide + (int) rightSide;
                    case BoundBinaryOperatorKind.Subtraction:
                        return (int) leftSide - (int) rightSide;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (int) leftSide * (int) rightSide;
                    case BoundBinaryOperatorKind.Division:
                        return (int) leftSide / (int) rightSide;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool) leftSide && (bool) rightSide;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool) leftSide || (bool) rightSide;
                    case BoundBinaryOperatorKind.Equality:
                        return Equals(leftSide, rightSide);
                    case BoundBinaryOperatorKind.Inequality:
                        return !Equals(leftSide, rightSide);
                    default:
                        throw new Exception($"Unexpected binary operator {boundBinaryExpression.OperatorUnit}");
                }
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}