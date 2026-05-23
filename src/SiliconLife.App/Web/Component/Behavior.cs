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

namespace SiliconLife.App.Web.Component;

/// <summary>
/// Behavior system - declarative definition of component interaction logic
/// </summary>
public class Behavior
{
    private readonly List<JsSyntax> _steps = new();
    private JsSyntax? _validation;
    private JsSyntax? _action;
    private JsSyntax? _onSuccess;
    private JsSyntax? _onError;

    /// <summary>
    /// Create simple call behavior
    /// </summary>
    public static Behavior Call(JsSyntax js)
    {
        return new Behavior().Then(js);
    }

    /// <summary>
    /// Create validation behavior
    /// </summary>
    public static Behavior Validate(JsSyntax validation)
    {
        var behavior = new Behavior();
        behavior._validation = validation;
        return behavior;
    }

    /// <summary>
    /// Set execution action
    /// </summary>
    public Behavior Then(JsSyntax action)
    {
        _action = action;
        return this;
    }

    /// <summary>
    /// Set success callback
    /// </summary>
    public Behavior OnSuccess(JsSyntax success)
    {
        _onSuccess = success;
        return this;
    }

    /// <summary>
    /// Set error callback
    /// </summary>
    public Behavior OnError(JsSyntax error)
    {
        _onError = error;
        return this;
    }

    /// <summary>
    /// Add custom JS step
    /// </summary>
    public Behavior Step(JsSyntax js)
    {
        _steps.Add(js);
        return this;
    }

    /// <summary>
    /// Build JS code
    /// </summary>
    public string Build()
    {
        var statements = new List<JsSyntax>();

        // If has validation
        if (_validation != null)
        {
            var ifBody = new List<JsSyntax>();
            
            if (_action != null)
                ifBody.Add(_action);
            
            if (_onSuccess != null)
                ifBody.Add(_onSuccess);

            var branches = new List<(JsSyntax? Condition, List<JsSyntax> Body)>();
            branches.Add((_validation, ifBody));

            if (_onError != null)
            {
                branches.Add((null, new List<JsSyntax> { _onError }));
            }

            statements.Add(JsIf.If(() => branches));
        }
        else
        {
            // No validation, execute directly
            if (_action != null)
                statements.Add(_action);
            
            if (_onSuccess != null)
                statements.Add(_onSuccess);
        }

        // Add custom steps
        statements.AddRange(_steps);

        // Build all statements and join with semicolon
        var builtStatements = statements.Select(s => s.Build()).ToList();
        return string.Join("; ", builtStatements);
    }
}
