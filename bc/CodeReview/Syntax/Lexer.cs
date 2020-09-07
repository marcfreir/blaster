using System.Collections.Generic;

namespace Blaster.CodeReview.Syntax
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>();
        public Lexer(string text)
        {
            this._text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char CurrentChar => Peek(0);
        private char LookAhead => Peek(1);

        private char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
            {
                return '\0';
            }

            return _text[index];
        }

        private void NextChar()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {
            // <numbers>
            // + - * / ()
            // <whitespaces>

            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }

            if (char.IsDigit(CurrentChar))
            {
                var start = _position;

                while (char.IsDigit(CurrentChar))
                {
                    NextChar();
                }

                var length = _position - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                {
                    _diagnostics.Add($"The number {_text} is NOT a valid Int32.");
                    var textHint = "[Hint]: (Int32 is an immutable value type that represents signed 32-bit integers with values that range from negative 2,147,483,648 through positive 2,147,483,647.)";
                    _diagnostics.Add(textHint);
                }

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(CurrentChar))
            {
                var start = _position;

                while (char.IsWhiteSpace(CurrentChar))
                {
                    NextChar();
                }

                var length = _position - start;
                var text = _text.Substring(start, length);
                //int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            // True
            // False
            if (char.IsLetter(CurrentChar))
            {
                var start = _position;

                while (char.IsLetter(CurrentChar))
                {
                    NextChar();
                }

                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = SyntaxFacts.GetKeywordKind(text);
                //int.TryParse(text, out var value);
                return new SyntaxToken(kind, start, text, null);
            }

            switch (CurrentChar)
            {
                case '+':
                {
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                }
                case '-':
                {
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                }
                case '*':
                {
                    return new SyntaxToken(SyntaxKind.MultiplyToken, _position++, "*", null);
                }
                case '/':
                {
                    return new SyntaxToken(SyntaxKind.DivideToken, _position++, "/", null);
                }
                case '(':
                {
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
                }
                case ')':
                {
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
                }
                case '&':
                {
                    if (LookAhead == '&')
                    {
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, _position += 2, "&&", null);
                    }
                    break;
                }
                case '|':
                {
                    if (LookAhead == '|')
                    {
                        return new SyntaxToken(SyntaxKind.PipePipeToken, _position += 2, "||", null);
                    }
                    break;
                }
                case '=':
                {
                    if (LookAhead == '=')
                    {
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, _position += 2, "==", null);
                    }
                    break;
                }
                case '!':
                {
                    if (LookAhead == '=')
                    {
                        return new SyntaxToken(SyntaxKind.ExclamationEqualsToken, _position += 2, "!=", null);
                    }
                    else
                    {
                        return new SyntaxToken(SyntaxKind.ExclamationToken, _position++, "!", null);
                    }
                }
                
            }

            _diagnostics.Add($"ERROR:: Bad character input: '{CurrentChar}'");

            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}