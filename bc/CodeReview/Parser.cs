using System.Collections.Generic;

namespace Blaster.CodeReview
{
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokenList;
        private List<string> _diagnostics = new List<string>();
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

        public IEnumerable<string> Diagnostics => _diagnostics;

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
            
            _diagnostics.Add($"ERROR:: Unexpected token <{CurrentToken.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, CurrentToken.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            var expression = ParseExpression();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);

            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }


        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            var leftSide = ParsePrimaryExpression();
            
            while (true)
            {
                var precedence = CurrentToken.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence)
                {
                    break;
                }

                var operatorToken = NextToken();
                var rightSide = ParseExpression(precedence);
                leftSide = new BinaryExpressionSyntax(leftSide, operatorToken, rightSide);
            }
            return leftSide;
        }


        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (CurrentToken.Kind == SyntaxKind.OpenParenthesisToken)
            {
                var leftSide = NextToken();
                var expression = ParseExpression();
                var rightSide = MatchToken(SyntaxKind.CloseParenthesisToken);

                return new ParenthesizedExpressionSyntax(leftSide, expression, rightSide);
            }
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}