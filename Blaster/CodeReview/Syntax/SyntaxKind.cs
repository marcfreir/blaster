namespace Blaster.CodeReview.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        WhitespaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        MultiplyToken,
        DivideToken,

        //BangToken,
        ExclamationToken,
        
        EqualsToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        EqualsEqualsToken,

        //BangEqualsToken,
        ExclamationEqualsToken,

        OpenParenthesisToken,
        CloseParenthesisToken,
        IdentifierToken,
        

        

        // Keywords
        FalseKeyword,
        TrueKeyword,


        // Expressions
        LiteralExpression,
        NameExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        AssignmentExpression,
        
    }
}