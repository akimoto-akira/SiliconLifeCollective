// Copyright (c) 2026 Hoshino Kennji
// 
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

// JsBuilder Design Rules:
// 1. All JS syntax elements inherit from JsSyntax
// 2. No ElseIf()/Else() methods - use Dictionary parameter instead
// 3. No Case()/AddBody() methods - use Dictionary parameter instead
// 4. Body/Cases elements must be JsSyntax, not string
// 5. No direct string concatenation for code generation
// 6. All parameters use callback form for debugging and tracing

using System.Text;

namespace SiliconLife.Default.Web;

public abstract class JsSyntax
{
    public abstract string Build();
    public JsPropAccess Prop(Func<string> property) => new(() => this, property);
    public JsCall Call(Func<string> method, params Func<JsSyntax>[] args)
    {
        var call = new JsCall(() => this, method);
        foreach (var arg in args) call.Args.Add(arg);
        return call;
    }
    public JsIndex Index(Func<JsSyntax> index) => new(() => this, index);
    public JsBinOp Op(Func<string> op, Func<JsSyntax> right) => new(() => (JsSyntax)this, op, right);
    public JsUnaryOp Not() => new(() => "!", () => (JsSyntax)this);

    public override bool Equals(object? obj) => obj is JsSyntax other && Build() == other.Build();
    public override int GetHashCode() => Build().GetHashCode();
    public static bool operator ==(JsSyntax? left, JsSyntax? right)
        => left is null ? right is null : left.Equals(right);
    public static bool operator !=(JsSyntax? left, JsSyntax? right) => !(left == right);
}

public class JsIdent : JsSyntax
{
    public Func<string> Name { get; }
    public JsIdent(Func<string> name) => Name = name;
    public override string Build() => Name();
}

public class JsString : JsSyntax
{
    public Func<string> Value { get; }
    public JsString(Func<string> value) => Value = value;
    public override string Build()
    {
        var value = Value();
        var sb = new StringBuilder();
        foreach (char c in value)
        {
            switch (c)
            {
                case '\\': sb.Append("\\\\"); break;
                case '"': sb.Append("\\\""); break;
                case '\n': sb.Append("\\n"); break;
                case '\r': sb.Append("\\r"); break;
                case '\t': sb.Append("\\t"); break;
                case '\b': sb.Append("\\b"); break;
                case '\f': sb.Append("\\f"); break;
                default: sb.Append(c); break;
            }
        }
        return $"\"{sb}\"";
    }
}

public class JsNumber : JsSyntax
{
    public Func<string> Value { get; }
    public JsNumber(Func<string> value) => Value = value;
    public override string Build() => Value();
}

public class JsArrayLiteral : JsSyntax
{
    private readonly List<Func<JsSyntax>> _elements = new();
    public JsArrayLiteral Add(Func<JsSyntax> element) { _elements.Add(element); return this; }
    public override string Build() => _elements.Count == 0 ? "[]" : $"[{string.Join(", ", _elements.Select(e => e().Build()))}]";
}

public class JsBool : JsSyntax
{
    public Func<bool> Value { get; }
    public JsBool(Func<bool> value) => Value = value;
    public override string Build() => Value() ? "true" : "false";
}

public class JsNull : JsSyntax
{
    public override string Build() => "null";
}

public class JsNew : JsSyntax
{
    public Func<JsSyntax> Constructor { get; }
    public List<Func<JsSyntax>> Args { get; } = new();
    public JsNew(Func<JsSyntax> constructor, params Func<JsSyntax>[] args)
    {
        Constructor = constructor;
        foreach (var arg in args) Args.Add(arg);
    }
    public override string Build() => $"new {Constructor().Build()}({string.Join(", ", Args.Select(a => a().Build()))})";
}

public class JsProp : JsSyntax
{
    public Func<string> Key { get; }
    public Func<JsSyntax> Value { get; }
    public JsProp(Func<string> key, Func<JsSyntax> value)
    {
        Key = key;
        Value = value;
    }
    private static bool IsValidIdentifier(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        if (!char.IsLetter(name[0]) && name[0] != '_' && name[0] != '$') return false;
        for (int i = 1; i < name.Length; i++)
        {
            if (!char.IsLetterOrDigit(name[i]) && name[i] != '_' && name[i] != '$') return false;
        }
        return true;
    }
    public override string Build()
    {
        var key = Key();
        var keyStr = IsValidIdentifier(key) ? key : $"\"{key}\"";
        return $"{keyStr}: {Value().Build()}";
    }
}

public class JsObj : JsSyntax
{
    private readonly List<Func<JsProp>> _props = new();
    public JsObj() { }
    public JsObj Prop(Func<string> key, Func<JsSyntax> value)
    {
        _props.Add(() => new JsProp(key, value));
        return this;
    }
    public override string Build() => $"{{ {string.Join(", ", _props.Select(p => p().Build()))} }}";
}

public class JsCall : JsSyntax
{
    public Func<JsSyntax> Target { get; }
    public Func<string> Method { get; }
    public List<Func<JsSyntax>> Args { get; } = new();
    public JsCall(Func<JsSyntax> target, Func<string> method)
    {
        Target = target;
        Method = method;
    }
    public override string Build() => $"{Target().Build()}.{Method()}({string.Join(", ", Args.Select(a => a().Build()))})";
}

public class JsFuncCall : JsSyntax
{
    public Func<JsSyntax> Func { get; }
    public List<Func<JsSyntax>> Args { get; } = new();
    public JsFuncCall(Func<JsSyntax> func, params Func<JsSyntax>[] args)
    {
        Func = func;
        foreach (var arg in args) Args.Add(arg);
    }
    public override string Build() => $"{Func().Build()}({string.Join(", ", Args.Select(a => a().Build()))})";
}

public class JsBinOp : JsSyntax
{
    public Func<JsSyntax> Left { get; }
    public Func<string> Operator { get; }
    public Func<JsSyntax> Right { get; }
    public JsBinOp(Func<JsSyntax> left, Func<string> op, Func<JsSyntax> right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }
    public override string Build() => $"{Left().Build()} {Operator()} {Right().Build()}";
}

public class JsUnaryOp : JsSyntax
{
    public Func<string> Operator { get; }
    public Func<JsSyntax> Value { get; }
    public JsUnaryOp(Func<string> op, Func<JsSyntax> value)
    {
        Operator = op;
        Value = value;
    }
    public override string Build() => $"{Operator()}{Value().Build()}";
}

public class JsParen : JsSyntax
{
    public Func<JsSyntax> Inner { get; }
    public JsParen(Func<JsSyntax> inner) => Inner = inner;
    public override string Build() => $"({Inner().Build()})";
}

public class JsTernary : JsSyntax
{
    public Func<JsSyntax> Condition { get; }
    public Func<JsSyntax> TrueValue { get; }
    public Func<JsSyntax> FalseValue { get; }
    public JsTernary(Func<JsSyntax> condition, Func<JsSyntax> trueValue, Func<JsSyntax> falseValue)
    {
        Condition = condition;
        TrueValue = trueValue;
        FalseValue = falseValue;
    }
    public override string Build() => $"({Condition().Build()} ? {TrueValue().Build()} : {FalseValue().Build()})";
}

public class JsIndex : JsSyntax
{
    public Func<JsSyntax> Target { get; }
    public Func<JsSyntax> Index { get; }
    public JsIndex(Func<JsSyntax> target, Func<JsSyntax> index)
    {
        Target = target;
        Index = index;
    }
    public override string Build() => $"{Target().Build()}[{Index().Build()}]";
}

public class JsPropAccess : JsSyntax
{
    public Func<JsSyntax> Target { get; }
    public Func<string> Property { get; }
    public JsPropAccess(Func<JsSyntax> target, Func<string> property)
    {
        Target = target;
        Property = property;
    }
    public override string Build() => $"{Target().Build()}.{Property()}";
}

public class JsIf : JsSyntax
{
    public Func<List<(JsSyntax? Condition, List<JsSyntax> Body)>> Branches { get; }

    public JsIf(Func<List<(JsSyntax? Condition, List<JsSyntax> Body)>> branches)
    {
        Branches = branches;
    }

    public static JsIf If(Func<List<(JsSyntax? Condition, List<JsSyntax> Body)>> branches) => new(branches);

    public override string Build()
    {
        var sb = new StringBuilder();
        var isFirst = true;
        foreach (var (condition, body) in Branches())
        {
            if (isFirst)
            {
                sb.Append($"if ({condition?.Build()}) {{ {string.Join(" ", body.Select(b => b.Build()))} }}");
                isFirst = false;
            }
            else if (condition == null)
            {
                sb.Append($" else {{ {string.Join(" ", body.Select(b => b.Build()))} }}");
            }
            else
            {
                sb.Append($" else if ({condition.Build()}) {{ {string.Join(" ", body.Select(b => b.Build()))} }}");
            }
        }
        return sb.ToString();
    }
}

public class JsSwitch : JsSyntax
{
    public Func<JsSyntax> Expression { get; }
    public Func<List<(JsSyntax? Value, List<JsSyntax> Body)>> Cases { get; }

    public JsSwitch(Func<JsSyntax> expression, Func<List<(JsSyntax? Value, List<JsSyntax> Body)>> cases)
    {
        Expression = expression;
        Cases = cases;
    }

    public static JsSwitch Switch(Func<JsSyntax> expression, Func<List<(JsSyntax? Value, List<JsSyntax> Body)>> cases)
        => new(expression, cases);

    public override string Build()
    {
        var sb = new StringBuilder();
        sb.Append($"switch ({Expression().Build()}) {{");
        foreach (var (value, body) in Cases())
        {
            if (value == null)
                sb.Append($"\n  default: {{\n    {string.Join("\n    ", body.Select(b => b.Build()))}\n  }}");
            else
                sb.Append($"\n  case {value.Build()}: {{\n    {string.Join("\n    ", body.Select(b => b.Build()))}\n  }}");
        }
        sb.Append("\n}");
        return sb.ToString();
    }
}

public class JsBlock : JsSyntax
{
    internal readonly List<Func<JsSyntax>> _statements = new();
    public JsBlock() { }
    public JsBlock Add(Func<JsSyntax> stmt)
    {
        _statements.Add(stmt);
        return this;
    }
    public override string Build() => string.Join("\n", _statements.Select(s => s().Build()));
}

public class JsConst : JsSyntax
{
    public Func<string> Name { get; }
    public Func<JsSyntax> Value { get; }
    public JsConst(Func<string> name, Func<JsSyntax> value) { Name = name; Value = value; }
    public override string Build() => $"const {Name()} = {Value().Build()};";
}

public class JsLet : JsSyntax
{
    public Func<string> Name { get; }
    public Func<JsSyntax> Value { get; }
    public JsLet(Func<string> name, Func<JsSyntax> value) { Name = name; Value = value; }
    public override string Build() => $"let {Name()} = {Value().Build()};";
}

public class JsAssign : JsSyntax
{
    public Func<JsSyntax> Target { get; }
    public Func<JsSyntax> Value { get; }
    public JsAssign(Func<JsSyntax> target, Func<JsSyntax> value) { Target = target; Value = value; }
    public override string Build() => $"{Target().Build()} = {Value().Build()};";
}

public class JsReturn : JsSyntax
{
    public Func<JsSyntax> Value { get; }
    public JsReturn(Func<JsSyntax> value) => Value = value;
    public override string Build() => $"return {Value().Build()};";
}

public class JsExprStmt : JsSyntax
{
    public Func<JsSyntax> Expr { get; }
    public JsExprStmt(Func<JsSyntax> expr) => Expr = expr;
    public override string Build() => $"{Expr().Build()};";
}

public class JsFuncDecl : JsSyntax
{
    public Func<string> Name { get; }
    public Func<List<string>> Params { get; }
    public Func<JsSyntax> Body { get; }
    public JsFuncDecl(Func<string> name, Func<List<string>> @params, Func<JsSyntax> body)
    {
        Name = name;
        Params = @params;
        Body = body;
    }
    public override string Build() => $"function {Name()}({string.Join(", ", Params())}) {{ {Body().Build()} }}";
}

public class JsArrowFunc : JsSyntax
{
    public Func<List<string>> Params { get; }
    public Func<JsSyntax> Body { get; }
    public JsArrowFunc(Func<List<string>> @params, Func<JsSyntax> body)
    {
        Params = @params;
        Body = body;
    }
    public override string Build()
    {
        var body = Body();
        if (body is JsBlock block)
        {
            var stmts = string.Join("\n", block._statements.Select(s => s().Build()));
            return $"({string.Join(", ", Params())}) => {{\n{stmts}\n}}";
        }
        var bodyStr = body.Build();
        if (bodyStr.EndsWith(";"))
            bodyStr = bodyStr.Substring(0, bodyStr.Length - 1);
        // Wrap object literals in parentheses to avoid being parsed as block statements
        if (body is JsObj)
            return $"({string.Join(", ", Params())}) => ({bodyStr})";
        return $"({string.Join(", ", Params())}) => {bodyStr}";
    }
}

public class JsRegex : JsSyntax
{
    public Func<string> Pattern { get; }
    public Func<string> Flags { get; }
    public JsRegex(Func<string> pattern, Func<string> flags)
    {
        Pattern = pattern;
        Flags = flags;
    }
    public override string Build() => string.IsNullOrEmpty(Flags()) ? $"/{Pattern()}/" : $"/{Pattern()}/{Flags()}";
}

public class JsBreak : JsSyntax
{
    public override string Build() => "break;";
}

public class JsFor : JsSyntax
{
    public Func<JsSyntax> Init { get; }
    public Func<JsSyntax> Condition { get; }
    public Func<JsSyntax> Increment { get; }
    public Func<JsSyntax> Body { get; }

    public JsFor(Func<JsSyntax> init, Func<JsSyntax> condition, Func<JsSyntax> increment, Func<JsSyntax> body)
    {
        Init = init;
        Condition = condition;
        Increment = increment;
        Body = body;
    }

    public override string Build()
    {
        var init = StripTrailingSemicolon(Init().Build());
        var condition = StripTrailingSemicolon(Condition().Build());
        var increment = StripTrailingSemicolon(Increment().Build());
        return $"for ({init}; {condition}; {increment}) {{ {Body().Build()} }}";
    }

    private static string StripTrailingSemicolon(string s) => s.EndsWith(";") ? s.Substring(0, s.Length - 1) : s;
}

public class JsWhile : JsSyntax
{
    public Func<JsSyntax> Condition { get; }
    public Func<JsSyntax> Body { get; }

    public JsWhile(Func<JsSyntax> condition, Func<JsSyntax> body)
    {
        Condition = condition;
        Body = body;
    }

    public override string Build()
    {
        var condition = StripTrailingSemicolon(Condition().Build());
        return $"while ({condition}) {{ {Body().Build()} }}";
    }

    private static string StripTrailingSemicolon(string s) => s.EndsWith(";") ? s.Substring(0, s.Length - 1) : s;
}

public static class Js
{
    public static JsIdent Id(Func<string> name) => new(name);
    public static JsString Str(Func<string> value) => new(value);
    public static JsNumber Num(Func<string> value) => new(value);
    public static JsBool Bool(Func<bool> value) => new(value);
    public static JsNull Null() => new();
    public static JsBreak Break() => new();
    public static JsSwitch Switch(Func<JsSyntax> expression, Func<List<(JsSyntax? Value, List<JsSyntax> Body)>> cases) => new(expression, cases);
    public static JsNew New(Func<JsSyntax> constructor, params Func<JsSyntax>[] args) => new(constructor, args);
    public static JsObj Obj() => new();
    public static JsArrayLiteral Array() => new();
    public static JsBlock Block() => new();
    public static JsConst Const(Func<string> name, Func<JsSyntax> value) => new(name, value);
    public static JsLet Let(Func<string> name, Func<JsSyntax> value) => new(name, value);
    public static JsAssign Assign(Func<JsSyntax> target, Func<JsSyntax> value) => new(target, value);
    public static JsReturn Return(Func<JsSyntax> value) => new(value);
    public static JsExprStmt Expr(Func<JsSyntax> expr) => new(expr);
    public static JsCall Call(Func<JsSyntax> target, Func<string> method, params Func<JsSyntax>[] args)
    {
        var call = new JsCall(target, method);
        foreach (var arg in args) call.Args.Add(arg);
        return call;
    }
    public static JsFuncCall Invoke(Func<JsSyntax> func, params Func<JsSyntax>[] args) => new(func, args);
    public static JsIndex Index(Func<JsSyntax> target, Func<JsSyntax> index) => new(target, index);
    public static JsPropAccess Prop(Func<JsSyntax> target, Func<string> property) => new(target, property);
    public static JsBinOp Op(Func<JsSyntax> left, Func<string> op, Func<JsSyntax> right) => new(left, op, right);
    public static JsUnaryOp Not(Func<JsSyntax> value) => new(() => "!", value);
    public static JsTernary Ternary(Func<JsSyntax> condition, Func<JsSyntax> trueValue, Func<JsSyntax> falseValue) => new(condition, trueValue, falseValue);
    public static JsIf If(Func<List<(JsSyntax? Condition, List<JsSyntax> Body)>> branches) => new(branches);
    public static JsFuncDecl Func(Func<string> name, Func<List<string>> @params, Func<JsSyntax> body) => new(name, @params, body);
    public static JsArrowFunc Arrow(Func<List<string>> @params, Func<JsSyntax> body) => new(@params, body);
    public static JsRegex Regex(Func<string> pattern, Func<string> flags) => new(pattern, flags);
    public static JsFor For(Func<JsSyntax> init, Func<JsSyntax> condition, Func<JsSyntax> increment, Func<JsSyntax> body) => new(init, condition, increment, body);
    public static JsWhile While(Func<JsSyntax> condition, Func<JsSyntax> body) => new(condition, body);
}

public static class JsSyntaxExtensions
{
    public static JsPropAccess Prop(this JsSyntax syntax, Func<string> property) => new(() => syntax, property);
    public static JsCall Call(this JsSyntax syntax, Func<string> method, params Func<JsSyntax>[] args)
    {
        var call = new JsCall(() => syntax, method);
        foreach (var arg in args) call.Args.Add(arg);
        return call;
    }
    public static JsFuncCall Invoke(this JsSyntax syntax, params Func<JsSyntax>[] args) => new(() => syntax, args);
    public static JsIndex Index(this JsSyntax syntax, Func<JsSyntax> index) => new(() => syntax, index);
    public static JsBinOp Op(this JsSyntax left, Func<string> op, Func<JsSyntax> right) => new(() => (JsSyntax)left, op, right);
    public static JsUnaryOp Not(this JsSyntax syntax) => new(() => "!", () => (JsSyntax)syntax);
    public static JsParen Paren(this JsSyntax syntax) => new(() => (JsSyntax)syntax);
    public static JsExprStmt Stmt(this JsSyntax syntax) => new(() => syntax);
    public static JsAssign Assign(this JsSyntax target, Func<JsSyntax> value) => new(() => target, value);
}
