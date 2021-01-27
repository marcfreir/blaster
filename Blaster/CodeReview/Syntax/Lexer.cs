using System;
using System.Collections.Generic;

namespace Blaster.CodeReview.Syntax
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private DiagnosticBag _diagnostics = new DiagnosticBag();
        public Lexer(string text)
        {
            this._text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

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

            var start = _position;

            if (char.IsDigit(CurrentChar))
            {
                //var start = _position;

                while (char.IsDigit(CurrentChar))
                {
                    NextChar();
                }

                var length = _position - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                {
                    _diagnostics.ReportInvalidNumber(new TextSpan(start, length), _text, typeof(int));
                    //_diagnostics.Add($"The number {_text} is NOT a valid Int32.");
                    //var textHint = "[Hint]: (Int32 is an immutable value type that represents signed 32-bit integers with values that range from negative 2,147,483,648 through positive 2,147,483,647.)";
                    //_diagnostics.Add(textHint);
                    Console.WriteLine();
                    Console.WriteLine("[Hint]: (Int32 is an immutable value type that represents signed 32-bit integers with values that range from negative 2,147,483,648 through positive 2,147,483,647.)");
                }

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(CurrentChar))
            {
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
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, start, "&&", null);
                    }
                    break;
                }
                case '|':
                {
                    if (LookAhead == '|')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.PipePipeToken, start, "||", null);
                    }
                    break;
                }
                case '=':
                {
                    if (LookAhead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, start, "==", null);
                    }
                    else
                    {
                        _position += 1;
                        return new SyntaxToken(SyntaxKind.EqualsToken, start, "=", null);
                    }
                }
                case '!':
                {
                    if (LookAhead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.ExclamationEqualsToken, start, "!=", null);
                    }
                    else
                    {
                        _position += 1;
                        return new SyntaxToken(SyntaxKind.ExclamationToken, start, "!", null);
                    }
                }
                
            }

            _diagnostics.ReportBadCharacter(_position, CurrentChar);

            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}