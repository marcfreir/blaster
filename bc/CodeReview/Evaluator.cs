using System;
using Blaster.CodeReview.Binding;
using Blaster.CodeReview.Syntax;

namespace Blaster.CodeReview
{

    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            this._root = root;
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

            //Evaluate UnaryExpression
            if (node is BoundUnaryExpression boundUnaryExpression)
            {
                var operand = (int) EvaluateExpression(boundUnaryExpression.Operand);

                switch (boundUnaryExpression.OperatorKind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -operand;
                    default:
                        throw new Exception($"Unexpected unary operator {boundUnaryExpression.OperatorKind}");
                }
            }
            
            //Evaluate BinaryExpression
            if (node is BoundBinaryExpression boundBinaryExpression)
            {
                var leftSide = (int) EvaluateExpression(boundBinaryExpression.LeftSide);
                var rightSide = (int) EvaluateExpression(boundBinaryExpression.RightSide);

                switch (boundBinaryExpression.OperatorKind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return leftSide + rightSide;
                    case BoundBinaryOperatorKind.Subtraction:
                        return leftSide - rightSide;
                    case BoundBinaryOperatorKind.Multiplication:
                        return leftSide * rightSide;
                    case BoundBinaryOperatorKind.Division:
                        return leftSide / rightSide;
                    default:
                        throw new Exception($"Unexpected binary operator {boundBinaryExpression.OperatorKind}");
                }
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}