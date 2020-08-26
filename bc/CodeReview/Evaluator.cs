﻿using System;

namespace Blaster.CodeReview
{

    public sealed class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            this._root = root;
            //
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            
            //Evaluate LiteralExpression
            if (node is LiteralExpressionSyntax literalExpressionSyntax)
            {
                return (int) literalExpressionSyntax.LiteralToken.Value;
            }
            
            //Evaluate BinaryExpression
            if (node is BinaryExpressionSyntax binaryExpressionSyntax)
            {
                var leftSide = EvaluateExpression(binaryExpressionSyntax.LeftSide);
                var rightSide = EvaluateExpression(binaryExpressionSyntax.RightSide);

                if (binaryExpressionSyntax.OperatorToken.Kind == SyntaxKind.PlusToken)
                {
                    return leftSide + rightSide;
                }
                else if (binaryExpressionSyntax.OperatorToken.Kind == SyntaxKind.MinusToken)
                {
                    return leftSide - rightSide;
                }
                else if (binaryExpressionSyntax.OperatorToken.Kind == SyntaxKind.MultiplyToken)
                {
                    return leftSide * rightSide;
                }
                else if (binaryExpressionSyntax.OperatorToken.Kind == SyntaxKind.DivideToken)
                {
                    return leftSide / rightSide;
                }
                else
                {
                    throw new Exception($"Unexpected binary operator {binaryExpressionSyntax.OperatorToken.Kind}");
                }
            }

            if (node is ParenthesizedExpressionSyntax parenthesizedExpressionSyntax)
            {
                return EvaluateExpression(parenthesizedExpressionSyntax.Expression);
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}