using System;

namespace Blaster.CodeReview.Syntax
{
    internal static class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxKind syntaxKind)
        {
            switch (syntaxKind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.ExclamationToken:
                    {
                        return 6;
                    }
                
                default:
                    {
                        return 0;
                    }
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxKind syntaxKind)
        {
            switch (syntaxKind)
            {
                case SyntaxKind.MultiplyToken:
                case SyntaxKind.DivideToken:
                    {
                        return 5;
                    }
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    {
                        return 4;
                    }
                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.ExclamationEqualsToken:
                    {
                        return 3;
                    }
                case SyntaxKind.AmpersandAmpersandToken:
                    {
                        return 2;
                    }
                case SyntaxKind.PipePipeToken:
                    {
                        return 1;
                    }
                
                default:
                    {
                        return 0;
                    }
            }
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            switch (text)
            {
                case "true":
                    {
                        return SyntaxKind.TrueKeyword;
                    }
                case "false":
                    {
                        return SyntaxKind.FalseKeyword;
                    }
                default:
                    {
                        return SyntaxKind.IdentifierToken;
                    }
            }
        }
    }
}