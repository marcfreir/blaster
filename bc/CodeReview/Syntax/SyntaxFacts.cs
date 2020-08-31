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
                    {
                        return 3;
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
                        return 2;
                    }
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
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