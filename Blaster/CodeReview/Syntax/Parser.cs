using System.Collections.Generic;

namespace Blaster.CodeReview.Syntax
{
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokenList;
        private DiagnosticBag _diagnostics = new DiagnosticBag();
        private int _position;

        public Parser(string text)
        {
            var tokenList = new List<SyntaxToken>();
            var lexer = new Lexer(text);
            SyntaxToken token;

            do
            {
                token = lexer.Lex();

                if (token.Kind != SyntaxKind.WhitespaceToken && token.Kind != SyntaxKind.BadToken)
                {
                    tokenList.Add(token);
                }

            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _tokenList = tokenList.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;

            if (index >= _tokenList.Length)
            {
                return _tokenList[_tokenList.Length - 1];
            }

            return _tokenList[index];
        }

        private SyntaxToken CurrentToken => Peek(0);

        private SyntaxToken NextToken()
        {
            var currentToken = CurrentToken;
            _position++;
            return currentToken;
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (CurrentToken.Kind == kind)
            {
                return NextToken();
            }
            
            _diagnostics.ReportUnexpectedToken(CurrentToken.Span, CurrentToken.Kind, kind);
            return new SyntaxToken(kind, CurrentToken.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            var expression = ParseExpression();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);

            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private ExpressionSyntax ParseAssignmentExpression()
        {
            if (Peek(0).Kind == SyntaxKind.IdentifierToken && Peek(1).Kind == SyntaxKind.EqualsEqualsToken)
            {
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var rightSide = ParseAssignmentExpression();
                return new AssignmentExpressionSyntax(identifierToken, operatorToken, rightSide);
            }

            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax leftSide;
            var unaryOperatorPrecedence = CurrentToken.Kind.GetUnaryOperatorPrecedence();

            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseBinaryExpression(unaryOperatorPrecedence);
                leftSide = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                leftSide = ParsePrimaryExpression();
            }
            
            while (true)
            {
                var precedence = CurrentToken.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence)
                {
                    break;
                }

                var operatorToken = NextToken();
                var rightSide = ParseBinaryExpression(precedence);
                leftSide = new BinaryExpressionSyntax(leftSide, operatorToken, rightSide);
            }
            return leftSide;
        }


        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (CurrentToken.Kind)
            {
                case SyntaxKind.OpenParenthesisToken:
                    {
                        var leftSide = NextToken();
                        var expression = ParseExpression();
                        var rightSide = MatchToken(SyntaxKind.CloseParenthesisToken);

                        return new ParenthesizedExpressionSyntax(leftSide, expression, rightSide);
                    }

                case SyntaxKind.FalseKeyword:
                case SyntaxKind.TrueKeyword:
                    {
                        var keywordToken = NextToken();
                        var value = keywordToken.Kind == SyntaxKind.TrueKeyword;
                        return new LiteralExpressionSyntax(keywordToken, value);
                    }
                case SyntaxKind.IdentifierToken:
                    {
                        var identifierToken = NextToken();
                        return new NameExpressionSyntax(identifierToken);
                    }
                default:
                    {
                        var numberToken = MatchToken(SyntaxKind.NumberToken);
                        return new LiteralExpressionSyntax(numberToken);
                    }
            }

        }
    }
}