using System.Collections.Generic;

namespace Blaster.CodeReview
{
    sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax leftSide, SyntaxToken operatorToken, ExpressionSyntax rightSide)
        {
            LeftSide = leftSide;
            OperatorToken = operatorToken;
            RightSide = rightSide;
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public ExpressionSyntax LeftSide { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax RightSide { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LeftSide;
            yield return OperatorToken;
            yield return RightSide;
        }
    }
}