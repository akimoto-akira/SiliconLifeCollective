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

namespace SiliconLife.Fast.Web.Component;

/// <summary>
/// JS helper class - provides type-safe JS code generation
/// </summary>
public static class Js
{
    /// <summary>
    /// Get DOM element by ID
    /// </summary>
    public static JsExpression Id(string id)
    {
        return new JsExpression($"document.getElementById('{id}')");
    }

    /// <summary>
    /// Get DOM element (querySelector)
    /// </summary>
    public static JsExpression Query(string selector)
    {
        return new JsExpression($"document.querySelector('{selector}')");
    }

    /// <summary>
    /// Call method on target
    /// </summary>
    public static JsExpression Call(JsExpression target, string method, params JsExpression[] args)
    {
        var argsStr = string.Join(", ", args.Select(a => a.Code));
        return new JsExpression($"{target.Code}.{method}({argsStr})");
    }

    /// <summary>
    /// Call global function
    /// </summary>
    public static JsExpression Call(string functionName, params JsExpression[] args)
    {
        var argsStr = string.Join(", ", args.Select(a => a.Code));
        return new JsExpression($"{functionName}({argsStr})");
    }

    /// <summary>
    /// String literal
    /// </summary>
    public static JsExpression Str(Func<string> valueFunc)
    {
        return new JsExpression($"'{valueFunc().Replace("'", "\\'")}'");
    }

    /// <summary>
    /// String literal
    /// </summary>
    public static JsExpression Str(string value)
    {
        return new JsExpression($"'{value.Replace("'", "\\'")}'");
    }

    /// <summary>
    /// Number literal
    /// </summary>
    public static JsExpression Num(int value)
    {
        return new JsExpression(value.ToString());
    }

    /// <summary>
    /// Get element value
    /// </summary>
    public static JsExpression Value(JsExpression element)
    {
        return new JsExpression($"{element.Code}.value");
    }

    /// <summary>
    /// Set element value
    /// </summary>
    public static JsExpression SetValue(JsExpression element, JsExpression value)
    {
        return new JsExpression($"{element.Code}.value = {value.Code}");
    }

    /// <summary>
    /// Validation expression
    /// </summary>
    public static JsValidation Validate(JsExpression condition)
    {
        return new JsValidation(condition);
    }
}

/// <summary>
/// JS expression
/// </summary>
public class JsExpression
{
    public string Code { get; }

    public JsExpression(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Logical NOT
    /// </summary>
    public JsExpression Not()
    {
        return new JsExpression($"!({Code})");
    }

    /// <summary>
    /// Equal
    /// </summary>
    public JsExpression Eq(JsExpression other)
    {
        return new JsExpression($"{Code} === {other.Code}");
    }

    /// <summary>
    /// Not equal
    /// </summary>
    public JsExpression Neq(JsExpression other)
    {
        return new JsExpression($"{Code} !== {other.Code}");
    }

    /// <summary>
    /// Greater than
    /// </summary>
    public JsExpression Gt(JsExpression other)
    {
        return new JsExpression($"{Code} > {other.Code}");
    }

    /// <summary>
    /// Addition operator
    /// </summary>
    public static JsExpression operator +(JsExpression a, JsExpression b)
    {
        return new JsExpression($"{a.Code} + {b.Code}");
    }

    /// <summary>
    /// Check if empty
    /// </summary>
    public JsExpression IsEmpty()
    {
        return new JsExpression($"{Code}.trim() === ''");
    }

    /// <summary>
    /// Check if not empty
    /// </summary>
    public JsExpression NotEmpty()
    {
        return new JsExpression($"{Code}.trim() !== ''");
    }

    public override string ToString() => Code;
}

/// <summary>
/// JS validation expression
/// </summary>
public class JsValidation
{
    public JsExpression Condition { get; }

    public JsValidation(JsExpression condition)
    {
        Condition = condition;
    }
}
