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

public class CssBuilder
{
    private readonly StringBuilder _sb = new();
    private readonly Stack<string> _selectorStack = new();
    private readonly Dictionary<string, string> _variables = new();
    private int _indentLevel = 0;

    public static CssBuilder Create() => new();

    public CssBuilder WithVariable(string name, string value)
    {
        _variables[name] = value;
        return this;
    }

    public CssBuilder Selector(string selector)
    {
        if (_selectorStack.Count > 0)
        {
            _sb.AppendLine();
        }
        _sb.Append(new string(' ', _indentLevel * 4));
        _sb.Append(selector);
        _sb.AppendLine(" {");
        _selectorStack.Push(selector);
        _indentLevel++;
        return this;
    }

    public CssBuilder Property(string name, string value)
    {
        _sb.Append(new string(' ', (_indentLevel + 1) * 4));
        _sb.AppendLine($"{name}: {value};");
        return this;
    }

    public CssBuilder EndSelector()
    {
        if (_selectorStack.Count > 0)
        {
            _selectorStack.Pop();
            _indentLevel--;
            _sb.Append(new string(' ', _indentLevel * 4));
            _sb.AppendLine("}");
        }
        return this;
    }

    public CssBuilder Comment(string text)
    {
        _sb.Append(new string(' ', _indentLevel * 4));
        _sb.AppendLine($"/* {text} */");
        return this;
    }

    public CssBuilder Media(string condition)
    {
        _sb.Append(new string(' ', _indentLevel * 4));
        _sb.AppendLine($"@media {condition} {{");
        _indentLevel++;
        return this;
    }

    public CssBuilder EndMedia()
    {
        _indentLevel--;
        _sb.Append(new string(' ', _indentLevel * 4));
        _sb.AppendLine("}");
        return this;
    }

    public CssBuilder FontFace(string fontFamily, string source)
    {
        _sb.AppendLine($"@font-face {{");
        _sb.AppendLine($"  font-family: {fontFamily};");
        _sb.AppendLine($"  src: {source};");
        _sb.AppendLine("}");
        return this;
    }

    public CssBuilder Keyframes(string name, Action<KeyframeBuilder> configure)
    {
        _sb.AppendLine($"@keyframes {name} {{");
        var builder = new KeyframeBuilder(_sb, 1);
        configure(builder);
        _sb.AppendLine("}");
        return this;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        if (_variables.Count > 0)
        {
            sb.AppendLine(":root {");
            foreach (var kv in _variables)
            {
                sb.AppendLine($"  --{kv.Key}: {kv.Value};");
            }
            sb.AppendLine("}");
            sb.AppendLine();
        }
        
        sb.Append(_sb.ToString());
        return sb.ToString();
    }

    public string Build() => ToString();
}

public class KeyframeBuilder
{
    private readonly StringBuilder _sb;
    private readonly int _baseIndent;

    public KeyframeBuilder(StringBuilder sb, int baseIndent)
    {
        _sb = sb;
        _baseIndent = baseIndent;
    }

    public KeyframeBuilder At(string percentage, Action<CssPropertyBuilder> configure)
    {
        _sb.Append(new string(' ', _baseIndent * 4));
        _sb.AppendLine($"{percentage} {{");
        var builder = new CssPropertyBuilder(_sb, _baseIndent + 1);
        configure(builder);
        _sb.Append(new string(' ', _baseIndent * 4));
        _sb.AppendLine("}");
        return this;
    }
}

public class CssPropertyBuilder
{
    private readonly StringBuilder _sb;
    private readonly int _baseIndent;

    public CssPropertyBuilder(StringBuilder sb, int baseIndent)
    {
        _sb = sb;
        _baseIndent = baseIndent;
    }

    public CssPropertyBuilder Property(string name, string value)
    {
        _sb.Append(new string(' ', (_baseIndent + 1) * 4));
        _sb.AppendLine($"{name}: {value};");
        return this;
    }
}
