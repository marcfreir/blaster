using System;
using System.Collections.Generic;
using System.Linq;
using Blaster.CodeReview;
using Blaster.CodeReview.Binding;
using Blaster.CodeReview.Syntax;

namespace Blaster
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //For the $udo commands
            var _showTree = false;
            
            //For the header (template)
            var _blasterLineStart = "#####################################################";
            var _blasterWelcome = "Welcome to Blaster Terminal";
            var _blasterVersion = "version 1.0";
            var _blasterAuthor = "Developed by: Marc Freir - marcfreir@outlook.com";
            var _blasterDate = "August-2020";
            var _blasterLineEnd = "#####################################################";

            Console.WriteLine($"{_blasterLineStart}\n{_blasterWelcome}\n{_blasterVersion}\n{_blasterAuthor}\n{_blasterDate}\n{_blasterLineEnd}\n");

            var variables = new Dictionary<VariableSymbol, object>();

            while (true)
            {
                //For the terminal inicialization
                Console.Write("$blaster>> ");
                var inputLine = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputLine))
                {
                    return;
                }

                /**********************************$UDO COMMANDS LIST - START************************************/

                //Basic commands for terminal operation ($udo)
                switch (inputLine)
                {
                    case "$udo":
                        {
                            var _sudo_message = "  $udo <command expected>!";
                            Console.WriteLine(_sudo_message);
                            continue;
                        }

                    case "$udo showTree":
                        _showTree = true;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(_showTree ? "Showing parse trees ENABLED." : "Showing parse trees DISABLED.");
                        Console.ResetColor();
                        continue;
                    case "$udo hideTree":
                        //if (_showTree)
                        //{
                        _showTree = false;
                        Console.WriteLine(_showTree ? "Showing parse trees ENABLED." : "Showing parse trees DISABLED.");
                        continue;
                    case "$udo help":
                    case "$udo HELP":
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            var _help_command = "[You typed >> $help]";
                            var _type_line = "Just type the command...\n";
                            var _exit_message = "$udo exit -> to exit the terminal. ";
                            var _help_message = "$udo help -> to help hints. ";
                            Console.WriteLine($"{_help_command}\n\n{_type_line}\n{_exit_message}\n{_help_message}");
                            Console.ResetColor();
                            continue;
                        }

                    case "$udo clear":
                        Console.Clear();
                        continue;
                    case "exit":
                    case "EXIT":
                    case "Exit":
                    case "$udo exit":
                        Console.WriteLine(">>Blaster Terminal terminated with code 0<<");
                        Environment.Exit(0);
                        break;
                }

                /**********************************$UDO COMMANDS LIST - END**************************************/

                //var parser = new Parser(inputLine);
                var syntaxTree = SyntaxTree.Parse(inputLine);
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variables);
                //var boundExpression = compilation.BindExpression(syntaxTree.Root);

                //var diagnostics = result.Diagnostics;

                if (_showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (!result.Diagnostics.Any())
                {
                    //var evaluator = new Evaluator(boundExpression);
                    //var evaluationResult = evaluator.Evaluate();
                    //Console.WriteLine(evaluationResult);
                    Console.WriteLine(result.Value);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        var prefix = inputLine.Substring(0, diagnostic.Span.Start);
                        var error = inputLine.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        var suffix = inputLine.Substring(diagnostic.Span.End);
                        Console.Write("    ");
                        Console.Write(prefix);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();
                        Console.Write(suffix);
                        Console.WriteLine();
                    }
                    Console.ResetColor();
                    Console.WriteLine();
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

}
