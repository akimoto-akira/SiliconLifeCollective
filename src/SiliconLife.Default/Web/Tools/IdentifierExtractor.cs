// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SiliconLife.Default.Web.Tools;

/// <summary>
/// Roslyn Identifier Extractor - Uses syntax tree analysis to extract complete identifiers
/// </summary>
public static class IdentifierExtractor
{
    /// <summary>
    /// Extracts the complete identifier at the specified position from code
    /// </summary>
    /// <param name="code">Source code</param>
    /// <param name="line">Line number (1-based)</param>
    /// <param name="column">Column number (1-based)</param>
    /// <returns>Identifier extraction result</returns>
    public static IdentifierResult ExtractIdentifier(string code, int line, int column)
    {
        var result = new IdentifierResult();

        try
        {
            // Parse syntax tree (tolerant mode)
            var tree = CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(kind: SourceCodeKind.Regular));
            var root = tree.GetRoot();

            // Calculate character position
            var position = GetPositionFromLineColumn(root, line, column);
            if (position == -1)
                return result;

            // Find the syntax node at this position
            var token = root.FindToken(position);
            if (token == default)
                return result;

            // Check if position is within a comment
            if (IsInComment(root, position))
            {
                // Word in comment: extract word and determine if it's a C# keyword
                var word = GetWordAtPosition(code, line, column);
                if (!string.IsNullOrEmpty(word))
                {
                    result.FullIdentifier = word;
                    result.WordType = IsCSharpKeyword(word) ? "keyword" : "comment";
                    result.Success = true;
                }
                return result;
            }

            // Extract identifier
            var identifierInfo = ExtractIdentifierFromToken(token, root, position);
            if (identifierInfo != null)
            {
                result.FullIdentifier = identifierInfo.FullIdentifier;
                result.WordType = identifierInfo.WordType;
                result.Success = true;
            }
        }
        catch (Exception)
        {
            // Tolerant handling: return empty result on parse failure
        }

        return result;
    }

    private static bool IsInComment(SyntaxNode root, int position)
    {
        var token = root.FindToken(position, findInsideTrivia: false);

        // Check current token's leading and trailing trivia
        if (CheckTriviaForComment(token.LeadingTrivia, position) ||
            CheckTriviaForComment(token.TrailingTrivia, position))
            return true;

        // Check previous token's trailing trivia (handles end-of-line comments
        // where FindToken returns the next line's token)
        var prevToken = token.GetPreviousToken();
        if (prevToken != default && CheckTriviaForComment(prevToken.TrailingTrivia, position))
            return true;

        // Handle structured doc comments (/// and /** */)
        // FindToken with findInsideTrivia:true returns tokens inside doc comment XML
        var innerToken = root.FindToken(position, findInsideTrivia: true);
        if (innerToken != token && innerToken.Parent != null)
        {
            var node = innerToken.Parent;
            while (node != null)
            {
                if (node is DocumentationCommentTriviaSyntax)
                    return true;
                node = node.Parent;
            }
        }

        return false;
    }

    private static bool CheckTriviaForComment(SyntaxTriviaList triviaList, int position)
    {
        foreach (var trivia in triviaList)
        {
            if (position < trivia.SpanStart)
                break;

            if (position < trivia.Span.End)
            {
                if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                    trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
                    trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
                    trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia))
                    return true;
            }
        }
        return false;
    }

    private static string? GetWordAtPosition(string code, int line, int column)
    {
        try
        {
            var lines = code.Split('\n');
            if (line < 1 || line > lines.Length)
                return null;

            var lineContent = lines[line - 1];
            if (column < 1 || column > lineContent.Length)
                return null;

            // Expand forward and backward from current position to extract complete word
            int start = column - 1;
            int end = column - 1;

            while (start > 0 && (char.IsLetterOrDigit(lineContent[start - 1]) || lineContent[start - 1] == '_'))
                start--;

            while (end < lineContent.Length && (char.IsLetterOrDigit(lineContent[end]) || lineContent[end] == '_'))
                end++;

            if (start >= end)
                return null;

            return lineContent.Substring(start, end - start);
        }
        catch
        {
            return null;
        }
    }

    private static bool IsCSharpKeyword(string word)
    {
        var keywords = new HashSet<string>
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
            "char", "checked", "class", "const", "continue", "decimal", "default",
            "delegate", "do", "double", "else", "enum", "event", "explicit",
            "extern", "false", "finally", "fixed", "float", "for", "foreach",
            "goto", "if", "implicit", "in", "int", "interface", "internal",
            "is", "lock", "long", "namespace", "new", "null", "object",
            "operator", "out", "override", "params", "private", "protected",
            "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
            "sizeof", "stackalloc", "static", "string", "struct", "switch",
            "this", "throw", "true", "try", "typeof", "uint", "ulong",
            "unchecked", "unsafe", "ushort", "using", "virtual", "void",
            "volatile", "while", "var", "dynamic", "async", "await", "nameof"
        };

        return keywords.Contains(word);
    }

    private static int GetPositionFromLineColumn(SyntaxNode root, int line, int column)
    {
        try
        {
            var text = root.GetText();
            var linePosition = new LinePosition(line - 1, column - 1);
            return text.Lines.GetPosition(linePosition);
        }
        catch
        {
            return -1;
        }
    }

    private static IdentifierInfo? ExtractIdentifierFromToken(SyntaxToken token, SyntaxNode root, int position)
    {
        // If the token itself is an identifier
        if (token.IsKind(SyntaxKind.IdentifierToken) && !string.IsNullOrEmpty(token.ValueText))
        {
            var parent = token.Parent;
            if (parent == null)
                return null;

            // Extract complete identifier chain (e.g., System.Net.IPAddress)
            var fullIdentifier = BuildFullIdentifier(token, parent);
            var wordType = DetermineWordType(parent);

            return new IdentifierInfo
            {
                FullIdentifier = fullIdentifier,
                WordType = wordType
            };
        }

        return null;
    }

    private static string BuildFullIdentifier(SyntaxToken token, SyntaxNode parent)
    {
        // For name syntax nodes (part of qualified name chains like System.Net.IPAddress),
        // walk up to find the outermost qualified expression
        if (parent is NameSyntax || parent is MemberAccessExpressionSyntax)
        {
            var topNode = parent;
            while (topNode.Parent is MemberAccessExpressionSyntax ||
                   topNode.Parent is QualifiedNameSyntax ||
                   topNode.Parent is AliasQualifiedNameSyntax)
            {
                topNode = topNode.Parent;
            }
            return CollectIdentifierParts(topNode);
        }

        // For declarations and other contexts, return the token text directly
        return token.ValueText;
    }

    private static string CollectIdentifierParts(SyntaxNode node)
    {
        return node switch
        {
            MemberAccessExpressionSyntax ma =>
                $"{CollectIdentifierParts(ma.Expression)}.{ma.Name.Identifier.ValueText}",
            QualifiedNameSyntax qn =>
                $"{CollectIdentifierParts(qn.Left)}.{qn.Right.Identifier.ValueText}",
            AliasQualifiedNameSyntax aq =>
                $"{aq.Alias.Identifier.ValueText}::{aq.Name.Identifier.ValueText}",
            GenericNameSyntax gn => gn.Identifier.ValueText,
            SimpleNameSyntax sn => sn.Identifier.ValueText,
            _ => node.ToString()
        };
    }

    private static string DetermineWordType(SyntaxNode node)
    {
        // Walk up past intermediate name/access nodes to find the meaningful parent
        var current = node;
        while (current is IdentifierNameSyntax || current is GenericNameSyntax ||
               current is QualifiedNameSyntax || current is MemberAccessExpressionSyntax ||
               current is AliasQualifiedNameSyntax)
        {
            if (current.Parent != null)
                current = current.Parent;
            else
                break;
        }

        // Determine identifier type based on the meaningful parent node type
        return current switch
        {
            ClassDeclarationSyntax => "class",
            StructDeclarationSyntax => "struct",
            InterfaceDeclarationSyntax => "interface",
            EnumDeclarationSyntax => "enum",
            MethodDeclarationSyntax => "method",
            PropertyDeclarationSyntax => "property",
            FieldDeclarationSyntax => "field",
            VariableDeclaratorSyntax => "variable",
            VariableDeclarationSyntax => "variable",
            ParameterSyntax => "parameter",
            InvocationExpressionSyntax => "function",
            ObjectCreationExpressionSyntax => "class",
            UsingDirectiveSyntax => "namespace",
            NamespaceDeclarationSyntax => "namespace",
            FileScopedNamespaceDeclarationSyntax => "namespace",
            BaseTypeSyntax => "class",
            TypeArgumentListSyntax => "type",
            _ => "identifier"
        };
    }

    public record IdentifierResult
    {
        public bool Success { get; set; }
        public string FullIdentifier { get; set; } = string.Empty;
        public string WordType { get; set; } = "identifier";
    }

    private record IdentifierInfo
    {
        public string FullIdentifier { get; set; } = string.Empty;
        public string WordType { get; set; } = "identifier";
    }
}
