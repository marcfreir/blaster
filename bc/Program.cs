using System;

namespace bc
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("$blaster>> ");
                var inputLine = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputLine))
                {
                    return;
                }

                var lexer = new Lexer(inputLine);
                while (true)
                {
                    var token = lexer.NextToken();
                    if (token.Kind == SyntaxKind.EndOfFileToken)
                    {
                        break;
                    }
                    Console.Write($"{token.Kind}: '{token.Text}'");

                    if (token.Value != null)
                    {
                        Console.Write($" {token.Value}");
                    }

                    Console.WriteLine();
                }
            }
        }
    }

    enum SyntaxKind
    {
        NumberToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        MultiplyToken,
        DivideToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        EndOfFileToken
    }

    class SyntaxToken
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
    }

    class Lexer
    {
        private readonly string _text;
        private int _position;
        public Lexer(string text)
        {
            this._text = text;
        }

        private char CurrentChar
        {
            get
            {
                if (_position >= _text.Length)
                {
                    return '\0';
                }

                return _text[_position];
            }
        }

        private void NextChar()
        {
            _position++;
        }

        public SyntaxToken NextToken()
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
                int.TryParse(text, out var value);
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

            if (CurrentChar == '+')
            {
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            }
            else if (CurrentChar == '-')
            {
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            }
            else if (CurrentChar == '*')
            {
                return new SyntaxToken(SyntaxKind.MultiplyToken, _position++, "*", null);
            }
            else if (CurrentChar == '/')
            {
                return new SyntaxToken(SyntaxKind.DivideToken, _position++, "/", null);
            }
            else if (CurrentChar == '(')
            {
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
            }
            else if (CurrentChar == ')')
            {
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
            }

            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}
