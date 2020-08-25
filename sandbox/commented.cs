/*delete this comment tag
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blaster
{
    class Program
    {
        static void Main(string[] args)
        {
            //For the $udo commands
            bool _showTree = false;
            //bool _hideTree = true;

            
            while (true)
            {
                //For the terminal inicialization
                Console.Write("$blaster>> ");
                var inputLine = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputLine))
                {
                    return;
                }
                delete this comment tag*/

                /**********************************$UDO COMMANDS LIST - START************************************/
                
                /*delete this comment tag
                //Basic commands for terminal operation ($udo)
                if (inputLine == "$udo")
                {
                    var _sudo_message = "  $udo <command expected>!";
                    Console.WriteLine(_sudo_message);
                    continue;
                }

                //Showing the Tree
                if (inputLine == "$udo showTree")
                {
                    _showTree = true;
                    Console.WriteLine(_showTree ? "Showing parse trees ENABLED." : "Showing parse trees DISABLED.");
                    continue;
                }
                //Disable Showing the Tree
                if (inputLine == "$udo hideTree")
                {
                    //if (_showTree)
                    //{
                        _showTree = false;
                        Console.WriteLine(_showTree ? "Showing parse trees ENABLED." : "Showing parse trees DISABLED.");
                        continue;
                    //}
                }
                
                //Showing helper commands (Help Library)
                if (inputLine == "$udo help" || inputLine == "$udo HELP")
                {
                    var _help_command = "[You typed >> $help]";
                    var _type_line = "Just type the command...\n";
                    var _exit_message = "$udo exit -> to exit the terminal. ";
                    var _help_message = "$udo help -> to help hints. ";
                    Console.WriteLine($"{_help_command}\n\n{_type_line}\n{_exit_message}\n{_help_message}");
                    continue;
                }

                //Clear the terminal
                if (inputLine == "$udo clear")
                {
                    Console.Clear();
                    continue;
                }

                //Just to exit the terminal
                if (inputLine == "exit" || inputLine =="EXIT" || inputLine == "Exit" || inputLine == "$udo exit")
                {
                    Console.WriteLine(">>Blaster Terminal terminated with code 0<<");
                    Environment.Exit(0);
                }
                delete this comment tag*/

                /**********************************$UDO COMMANDS LIST - END**************************************/
                
                /*delete this comment tag
                //var parser = new Parser(inputLine);
                var syntaxTree = SyntaxTree.Parse(inputLine);

                if (_showTree)
                {
                    var textColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    
                    PrettyPrint(syntaxTree.Root);
                    Console.ForegroundColor = textColor;
                }

                if (!syntaxTree.Diagnostics.Any())
                {
                    var evaluator = new Evaluator(syntaxTree.Root);
                    var evaluationResult = evaluator.Evaluate();
                    Console.WriteLine(evaluationResult);
                }
                else
                {
                    var textColor = Console.ForegroundColor;
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
        BinaryExpression,
        ParenthesizedExpression
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

    sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closeParenthesisToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;

        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseParenthesisToken { get; }


        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthesisToken;
            yield return Expression;
            yield return CloseParenthesisToken;
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

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
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

        private ExpressionSyntax ParseExpression()
        {
            return ParseTermExpression();
        }

        public SyntaxTree Parse()
        {
            var expression = ParseTermExpression();
            var endOfFileToken = Match(SyntaxKind.EndOfFileToken);

            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        private ExpressionSyntax ParseTermExpression()
        {
            var leftSide = ParseFactorExpression();

            while (CurrentToken.Kind == SyntaxKind.PlusToken || CurrentToken.Kind == SyntaxKind.MinusToken)
            {
                var operatorToken = NextToken();
                var rightSide = ParseFactorExpression();
                leftSide = new BinaryExpressionSyntax(leftSide, operatorToken, rightSide);
            }

            return leftSide;
        }

        private ExpressionSyntax ParseFactorExpression()
        {
            var leftSide = ParsePrimaryExpression();

            while (CurrentToken.Kind == SyntaxKind.MultiplyToken || CurrentToken.Kind == SyntaxKind.DivideToken)
            {
                var operatorToken = NextToken();
                var rightSide = ParsePrimaryExpression();
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
                var rightSide = Match(SyntaxKind.CloseParenthesisToken);

                return new ParenthesizedExpressionSyntax(leftSide, expression, rightSide);
            }
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

            if (node is ParenthesizedExpressionSyntax parenthesizedExpressionSyntax)
            {
                return EvaluateExpression(parenthesizedExpressionSyntax.Expression);
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }

}
delete this comment tag*/