using System;
using System.Collections.Generic;
using System.Linq;

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

                var parser = new Parser(inputLine);
                var syntaxTree = parser.Parse();

                var textColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                
                PrettyPrint(syntaxTree.Root);
                Console.ForegroundColor = textColor;

                if (!syntaxTree.Diagnostics.Any())
                {
                    var evaluator = new Evaluator(syntaxTree.Root);
                    var evaluationResult = evaluator.Evaluate();
                    Console.WriteLine(evaluationResult);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in syntaxTree.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }
                    Console.ForegroundColor = textColor;
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLastChild = true)
        {
            // └──>
            // ├──>
            // │

            
            var marker = isLastChild ? "└──>" : "├──>";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

            if (node is SyntaxToken syntaxToken && syntaxToken.Value != null)
            {
                Console.Write(" ");
                Console.Write(syntaxToken.Value);
            }

            Console.WriteLine();

            //indent += "    ";

            indent += isLastChild ? "    " : "│   ";
            
            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                PrettyPrint(child, indent, child == lastChild);
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
        EndOfFileToken,
        NumberExpression,
        BinaryExpression
    }

    class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }

    class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>();
        public Lexer(string text)
        {
            this._text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

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

            _diagnostics.Add($"ERROR:: Bad character input: '{CurrentChar}'");

            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }

    abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }

    abstract class ExpressionSyntax : SyntaxNode
    {
        
    }

    sealed class NumberExpressionSyntax : ExpressionSyntax
    {
        public NumberExpressionSyntax(SyntaxToken numberToken)
        {
            NumberToken = numberToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;

        public SyntaxToken NumberToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return NumberToken;
        }
    }

    sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax leftSide, SyntaxToken operatorToken, ExpressionSyntax rightSide)
        {
            LeftSide = leftSide;
            OperatorToken = operatorToken;
            RightSide = rightSide;
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public ExpressionSyntax LeftSide { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax RightSide { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LeftSide;
            yield return OperatorToken;
            yield return RightSide;
        }
    }

    sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }
    }

    class Parser
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
                token = lexer.NextToken();

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

        private SyntaxToken Match(SyntaxKind kind)
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
            var endOfFileToken = Match(SyntaxKind.EndOfFileToken);

            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression()
        {
            var leftSide = ParsePrimaryExpression();

            while (CurrentToken.Kind == SyntaxKind.PlusToken || CurrentToken.Kind == SyntaxKind.MinusToken)
            {
                var operatorToken = NextToken();
                var rightSide = ParsePrimaryExpression();
                leftSide = new BinaryExpressionSyntax(leftSide, operatorToken, rightSide);
            }

            return leftSide;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            var numberToken = Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }

    class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            this._root = root;
            //
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            
            //Evaluate NumberExpression
            if (node is NumberExpressionSyntax numberExpressionSyntax)
            {
                return (int) numberExpressionSyntax.NumberToken.Value;
            }
            
            //Evaluate BinaryExpression
            if (node is BinaryExpressionSyntax binaryExpressionSyntax)
            {
                var leftSide = EvaluateExpression(binaryExpressionSyntax.LeftSide);
                var rightSide = EvaluateExpression(binaryExpressionSyntax.RightSide);

                if (binaryExpressionSyntax.OperatorToken.Kind == SyntaxKind.PlusToken)
                {
                    return leftSide + rightSide;
                }
                else if (binaryExpressionSyntax.OperatorToken.Kind == SyntaxKind.MinusToken)
                {
                    return leftSide - rightSide;
                }
                else if (binaryExpressionSyntax.OperatorToken.Kind == SyntaxKind.MultiplyToken)
                {
                    return leftSide * rightSide;
                }
                else if (binaryExpressionSyntax.OperatorToken.Kind == SyntaxKind.DivideToken)
                {
                    return leftSide / rightSide;
                }
                else
                {
                    throw new Exception($"Unexpected binary operator {binaryExpressionSyntax.OperatorToken.Kind}");
                }
            }
            throw new Exception($"Unexpected node {node.Kind}");
        }
    }

}
