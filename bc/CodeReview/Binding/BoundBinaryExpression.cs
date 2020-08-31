using System;

namespace Blaster.CodeReview.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression leftSide, BoundBinaryOperator operatorUnit, BoundExpression rightSide)
        {
            LeftSide = leftSide;
            OperatorUnit = operatorUnit;
            RightSide = rightSide;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => LeftSide.Type;
        public BoundExpression LeftSide { get; }
        public BoundBinaryOperator OperatorUnit { get; }
        public BoundExpression RightSide { get; }

    }
}