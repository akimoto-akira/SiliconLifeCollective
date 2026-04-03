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

using System.Text;

namespace SiliconLife.Default.Web;

public class JsBuilder
{
    private readonly StringBuilder _sb = new();
    internal int _indentLevel = 0;
    private readonly Stack<string> _blockStack = new();

    public static JsBuilder Create() => new();

    public JsBuilder Variable(string name, string value)
    {
        Indent();
        _sb.AppendLine($"const {name} = {value};");
        return this;
    }

    public JsBuilder Let(string name, string value)
    {
        Indent();
        _sb.AppendLine($"let {name} = {value};");
        return this;
    }

    public JsBuilder Const(string name, string value)
    {
        Indent();
        _sb.AppendLine($"const {name} = {value};");
        return this;
    }

    public JsBuilder Function(string name, Action<FunctionBuilder> configure)
    {
        Indent();
        _sb.Append($"function {name}(");
        var builder = new FunctionBuilder(this, _sb);
        configure(builder);
        return this;
    }

    public JsBuilder ArrowFunction(string name, Action<FunctionBuilder> configure)
    {
        Indent();
        _sb.Append($"const {name} = (");
        var builder = new FunctionBuilder(this, _sb);
        configure(builder);
        _sb.AppendLine(" => {");
        _blockStack.Push("arrow");
        _indentLevel++;
        return this;
    }

    public JsBuilder AnonymousFunction(Action<FunctionBuilder> configure)
    {
        Indent();
        _sb.Append("function (");
        var builder = new FunctionBuilder(this, _sb);
        configure(builder);
        _sb.AppendLine(" {");
        _blockStack.Push("function");
        _indentLevel++;
        return this;
    }

    public JsBuilder AnonymousArrowFunction(Action<FunctionBuilder> configure)
    {
        Indent();
        _sb.Append("(");
        var builder = new FunctionBuilder(this, _sb);
        configure(builder);
        _sb.AppendLine(" => {");
        _blockStack.Push("arrow");
        _indentLevel++;
        return this;
    }

    public JsBuilder Return(string? value = null)
    {
        Indent();
        if (value == null)
            _sb.AppendLine("return;");
        else
            _sb.AppendLine($"return {value};");
        return this;
    }

    public JsBuilder Call(string name, params string[] args)
    {
        Indent();
        _sb.Append(name);
        _sb.Append("(");
        _sb.Append(string.Join(", ", args));
        _sb.AppendLine(");");
        return this;
    }

    public JsBuilder CallMethod(string obj, string method, params string[] args)
    {
        Indent();
        _sb.Append($"{obj}.{method}(");
        _sb.Append(string.Join(", ", args));
        _sb.AppendLine(");");
        return this;
    }

    public JsBuilder If(string condition)
    {
        Indent();
        _sb.Append($"if ({condition}) {{");
        _sb.AppendLine();
        _blockStack.Push("if");
        _indentLevel++;
        return this;
    }

    public JsBuilder ElseIf(string condition)
    {
        _indentLevel--;
        Indent();
        _sb.Append($"else if ({condition}) {{");
        _sb.AppendLine();
        _blockStack.Push("if");
        _indentLevel++;
        return this;
    }

    public JsBuilder Else()
    {
        _indentLevel--;
        Indent();
        _sb.AppendLine("else {");
        _blockStack.Push("else");
        _indentLevel++;
        return this;
    }

    public JsBuilder For(string init, string condition, string increment)
    {
        Indent();
        _sb.Append($"for ({init}; {condition}; {increment}) {{");
        _sb.AppendLine();
        _blockStack.Push("for");
        _indentLevel++;
        return this;
    }

    public JsBuilder ForIn(string variable, string iterable)
    {
        Indent();
        _sb.Append($"for (const {variable} in {iterable}) {{");
        _sb.AppendLine();
        _blockStack.Push("for");
        _indentLevel++;
        return this;
    }

    public JsBuilder ForOf(string variable, string iterable)
    {
        Indent();
        _sb.Append($"for (const {variable} of {iterable}) {{");
        _sb.AppendLine();
        _blockStack.Push("for");
        _indentLevel++;
        return this;
    }

    public JsBuilder While(string condition)
    {
        Indent();
        _sb.Append($"while ({condition}) {{");
        _sb.AppendLine();
        _blockStack.Push("while");
        _indentLevel++;
        return this;
    }

    public JsBuilder Switch(string expression)
    {
        Indent();
        _sb.Append($"switch ({expression}) {{");
        _sb.AppendLine();
        _blockStack.Push("switch");
        _indentLevel++;
        return this;
    }

    public JsBuilder Case(string value)
    {
        _indentLevel--;
        Indent();
        _sb.AppendLine($"case {value}:");
        _indentLevel++;
        return this;
    }

    public JsBuilder Default()
    {
        _indentLevel--;
        Indent();
        _sb.AppendLine("default:");
        _indentLevel++;
        return this;
    }

    public JsBuilder Break()
    {
        Indent();
        _sb.AppendLine("break;");
        return this;
    }

    public JsBuilder Continue()
    {
        Indent();
        _sb.AppendLine("continue;");
        return this;
    }

    public JsBuilder Throw(string message)
    {
        Indent();
        _sb.AppendLine($"throw new Error({message});");
        return this;
    }

    public JsBuilder Try(Action<JsBuilder> configure)
    {
        Indent();
        _sb.AppendLine("try {");
        _blockStack.Push("try");
        _indentLevel++;
        configure(this);
        return this;
    }

    public JsBuilder Catch(string? variable = null)
    {
        _indentLevel--;
        Indent();
        if (variable == null)
            _sb.AppendLine("catch {");
        else
            _sb.AppendLine($"catch ({variable}) {{");
        _blockStack.Push("catch");
        _indentLevel++;
        return this;
    }

    public JsBuilder Finally()
    {
        _indentLevel--;
        Indent();
        _sb.AppendLine("finally {");
        _blockStack.Push("finally");
        _indentLevel++;
        return this;
    }

    public JsBuilder Class(string name, string? extends = null)
    {
        Indent();
        if (extends == null)
            _sb.AppendLine($"class {name} {{");
        else
            _sb.AppendLine($"class {name} extends {extends} {{");
        _blockStack.Push("class");
        _indentLevel++;
        return this;
    }

    public JsBuilder Method(string name, Action<FunctionBuilder> configure)
    {
        Indent();
        _sb.Append($"{name}(");
        var builder = new FunctionBuilder(this, _sb);
        configure(builder);
        _sb.AppendLine(" {");
        _blockStack.Push("method");
        _indentLevel++;
        return this;
    }

    public JsBuilder Getter(string name)
    {
        Indent();
        _sb.Append($"get {name}() {{");
        _sb.AppendLine();
        _blockStack.Push("getter");
        _indentLevel++;
        return this;
    }

    public JsBuilder Setter(string name)
    {
        Indent();
        _sb.Append($"set {name}(");
        var builder = new FunctionBuilder(this, _sb);
        builder.Param("value");
        _sb.AppendLine(" {");
        _blockStack.Push("setter");
        _indentLevel++;
        return this;
    }

    public JsBuilder StaticMethod(string name, Action<FunctionBuilder> configure)
    {
        Indent();
        _sb.Append($"static {name}(");
        var builder = new FunctionBuilder(this, _sb);
        configure(builder);
        _sb.AppendLine(" {");
        _blockStack.Push("static");
        _indentLevel++;
        return this;
    }

    public JsBuilder New(string className, params string[] args)
    {
        Indent();
        _sb.Append($"new {className}(");
        _sb.Append(string.Join(", ", args));
        _sb.AppendLine(");");
        return this;
    }

    public JsBuilder Object(Action<ObjectBuilder> configure)
    {
        Indent();
        _sb.Append("{");
        _sb.AppendLine();
        _blockStack.Push("object");
        _indentLevel++;
        var builder = new ObjectBuilder(this, _sb);
        configure(builder);
        _indentLevel--;
        Indent();
        _sb.AppendLine("},");
        _blockStack.Pop();
        return this;
    }

    public JsBuilder Array(string[] items)
    {
        Indent();
        _sb.Append("[");
        _sb.Append(string.Join(", ", items));
        _sb.AppendLine("];");
        return this;
    }

    public JsBuilder Await(string expression)
    {
        Indent();
        _sb.AppendLine($"await {expression};");
        return this;
    }

    public JsBuilder Async(string name, Action<FunctionBuilder> configure)
    {
        Indent();
        _sb.Append($"async function {name}(");
        var builder = new FunctionBuilder(this, _sb);
        configure(builder);
        return this;
    }

    public JsBuilder AsyncArrowFunction(string name, Action<FunctionBuilder> configure)
    {
        Indent();
        _sb.Append($"const {name} = async (");
        var builder = new FunctionBuilder(this, _sb);
        configure(builder);
        _sb.AppendLine(" => {");
        _blockStack.Push("async-arrow");
        _indentLevel++;
        return this;
    }

    public JsBuilder Import(string module, string? alias = null)
    {
        Indent();
        if (alias == null)
            _sb.AppendLine($"import {module};");
        else
            _sb.AppendLine($"import {alias} from {module};");
        return this;
    }

    public JsBuilder ImportDefault(string module, string alias)
    {
        Indent();
        _sb.AppendLine($"import {alias} from {module};");
        return this;
    }

    public JsBuilder Export(string name)
    {
        Indent();
        _sb.AppendLine($"export {name};");
        return this;
    }

    public JsBuilder ExportDefault(string name)
    {
        Indent();
        _sb.AppendLine($"export default {name};");
        return this;
    }

    public JsBuilder Comment(string text)
    {
        Indent();
        _sb.AppendLine($"// {text}");
        return this;
    }

    public JsBuilder MultiLineComment(string text)
    {
        Indent();
        _sb.AppendLine($"/* {text} */");
        return this;
    }

    public JsBuilder EmptyLine()
    {
        _sb.AppendLine();
        return this;
    }

    public JsBuilder Raw(string code)
    {
        Indent();
        _sb.AppendLine(code);
        return this;
    }

    public JsBuilder EndBlock()
    {
        if (_blockStack.Count > 0)
        {
            var blockType = _blockStack.Pop();
            _indentLevel--;
            Indent();
            _sb.AppendLine("}");
        }
        return this;
    }

    public JsBuilder EndAllBlocks()
    {
        while (_blockStack.Count > 0)
        {
            EndBlock();
        }
        return this;
    }

    private void Indent()
    {
        _sb.Append(new string(' ', _indentLevel * 4));
    }

    public override string ToString()
    {
        return _sb.ToString();
    }

    public string Build() => ToString();
}

public class FunctionBuilder
{
    private readonly JsBuilder _parent;
    private readonly StringBuilder _sb;
    private bool _firstParam = true;
    private bool _isFirstStatement = true;

    public FunctionBuilder(JsBuilder parent, StringBuilder sb)
    {
        _parent = parent;
        _sb = sb;
    }

    public FunctionBuilder Param(string name)
    {
        if (!_firstParam)
            _sb.Append(", ");
        _sb.Append(name);
        _firstParam = false;
        return this;
    }

    public FunctionBuilder Param(string name, string defaultValue)
    {
        if (!_firstParam)
            _sb.Append(", ");
        _sb.Append($"{name} = {defaultValue}");
        _firstParam = false;
        return this;
    }

    public JsBuilder EndParams()
    {
        _sb.Append(") {");
        _sb.AppendLine();
        return _parent;
    }
}

public class ObjectBuilder
{
    private readonly JsBuilder _parent;
    private readonly StringBuilder _sb;
    private bool _first = true;

    public ObjectBuilder(JsBuilder parent, StringBuilder sb)
    {
        _parent = parent;
        _sb = sb;
    }

    public ObjectBuilder Property(string key, string value)
    {
        if (!_first)
            _sb.AppendLine(",");
        _sb.Append(new string(' ', (_parent._indentLevel + 1) * 4));
        _sb.Append($"{key}: {value}");
        _first = false;
        return this;
    }

    public ObjectBuilder Property(string key, Action<JsBuilder> configure)
    {
        if (!_first)
            _sb.AppendLine(",");
        _sb.Append(new string(' ', (_parent._indentLevel + 1) * 4));
        _sb.Append($"{key}: ");
        _parent._indentLevel++;
        configure(_parent);
        _parent._indentLevel--;
        _first = false;
        _sb.AppendLine(",");
        return this;
    }

    public JsBuilder EndObject()
    {
        _sb.AppendLine();
        return _parent;
    }
}

public class JsModuleBuilder
{
    private readonly JsBuilder _js = new();
    private readonly List<string> _imports = new();
    private readonly List<string> _exports = new();

    public JsModuleBuilder Import(string module, string? alias = null)
    {
        if (alias == null)
            _imports.Add($"import {module};");
        else
            _imports.Add($"import {alias} from {module};");
        return this;
    }

    public JsModuleBuilder Export(string name)
    {
        _exports.Add(name);
        return this;
    }

    public JsBuilder Js => _js;

    public string Build()
    {
        var sb = new StringBuilder();
        
        foreach (var imp in _imports)
        {
            sb.AppendLine(imp);
        }
        
        if (_imports.Count > 0 && _exports.Count > 0)
        {
            sb.AppendLine();
        }
        
        sb.Append(_js.ToString());
        
        if (_exports.Count > 0)
        {
            sb.AppendLine();
            foreach (var exp in _exports)
            {
                sb.AppendLine($"export {exp};");
            }
        }
        
        return sb.ToString();
    }
}
