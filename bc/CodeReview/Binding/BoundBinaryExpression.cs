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

        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
        public override Type Type => OperatorUnit.Type;
        public BoundExpression LeftSide { get; }
        public BoundBinaryOperator OperatorUnit { get; }
        public BoundExpression RightSide { get; }

    }
}