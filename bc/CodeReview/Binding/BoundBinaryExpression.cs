using System;

namespace Blaster.CodeReview.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression leftSide, BoundBinaryOperatorKind operatorKind, BoundExpression rightSide)
        {
            LeftSide = leftSide;
            OperatorKind = operatorKind;
            RightSide = rightSide;
            //
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => LeftSide.Type;
        public BoundExpression LeftSide { get; }
        public BoundBinaryOperatorKind OperatorKind { get; }
        public BoundExpression RightSide { get; }

    }
}