using System;

namespace Blaster.CodeReview.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator operatorUnit, BoundExpression operand)
        {
            OperatorUnit = operatorUnit;
            Operand = operand;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Operand.Type;
        public BoundUnaryOperator OperatorUnit { get; }
        public BoundExpression Operand { get; }

    }
}