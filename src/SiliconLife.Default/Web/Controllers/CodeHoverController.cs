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

using SiliconLife.Collective;
using SiliconLife.Default.Web.Models;
using SiliconLife.Default.Web.Tools;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SiliconLife.Default.Web;

[WebCode]
public class CodeHoverController : Controller
{
    private readonly LocalizationManager _localizationManager;
    
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CodeHoverController()
    {
        var locator = ServiceLocator.Instance;
        _localizationManager = LocalizationManager.Instance;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/api/code/hover";

        if (path == "/api/code/hover")
            GetHoverTip();
        else if (path == "/api/code/register")
            RegisterEditor();
        else if (path == "/api/code/update")
            UpdateCode();
        else if (path == "/api/code/unregister")
            UnregisterEditor();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void RegisterEditor()
    {
        if (Request.HttpMethod != "POST")
        {
            Response.StatusCode = 405;
            Response.Close();
            return;
        }

        try
        {
            using var reader = new StreamReader(Request.InputStream);
            var body = reader.ReadToEnd();
            var requestData = JsonSerializer.Deserialize<RegisterRequest>(body, _jsonOptions);
            
            if (requestData == null || string.IsNullOrEmpty(requestData.Language))
            {
                RenderJson(new { error = "Language parameter is required" });
                return;
            }

            var editorGuid = CodeEditorCacheModel.Register(requestData.Language);
            RenderJson(new { editorGuid });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message });
        }
    }

    private void UpdateCode()
    {
        if (Request.HttpMethod != "POST")
        {
            Response.StatusCode = 405;
            Response.Close();
            return;
        }

        try
        {
            using var reader = new StreamReader(Request.InputStream);
            var body = reader.ReadToEnd();
            var requestData = JsonSerializer.Deserialize<UpdateRequest>(body, _jsonOptions);
            
            if (requestData == null || string.IsNullOrEmpty(requestData.EditorGuid))
            {
                RenderJson(new { error = "EditorGuid parameter is required" });
                return;
            }

            var success = CodeEditorCacheModel.Update(requestData.EditorGuid, requestData.Code ?? string.Empty);
            RenderJson(new { success });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message });
        }
    }

    private void UnregisterEditor()
    {
        if (Request.HttpMethod != "POST")
        {
            Response.StatusCode = 405;
            Response.Close();
            return;
        }

        try
        {
            using var reader = new StreamReader(Request.InputStream);
            var body = reader.ReadToEnd();
            var requestData = JsonSerializer.Deserialize<UnregisterRequest>(body, _jsonOptions);
            
            if (requestData == null || string.IsNullOrEmpty(requestData.EditorGuid))
            {
                RenderJson(new { error = "EditorGuid parameter is required" });
                return;
            }

            var success = CodeEditorCacheModel.Unregister(requestData.EditorGuid);
            RenderJson(new { success });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message });
        }
    }

    private void GetHoverTip()
    {
        // Prefer to get parameters from POST body
        string? editorGuid = null;
        string? word = null;
        string language = "csharp";
        int line = 0;
        int col = 0;
        bool parsedFromPost = false;

        if (Request.HttpMethod == "POST")
        {
            try
            {
                using var reader = new StreamReader(Request.InputStream);
                var body = reader.ReadToEnd();
                var requestData = JsonSerializer.Deserialize<HoverRequest>(body, _jsonOptions);
                
                if (requestData != null)
                {
                    editorGuid = requestData.EditorGuid;
                    word = requestData.Word;
                    language = requestData.Language ?? "csharp";
                    line = requestData.Line;
                    col = requestData.Column;
                    parsedFromPost = true;
                }
            }
            catch
            {
                // Fallback to query parameters if parsing fails
            }
        }

        // Only fallback to query parameters if POST parsing failed (backward compatibility)
        if (!parsedFromPost)
        {
            word = Request.QueryString["word"];
            language = Request.QueryString["lang"] ?? "csharp";
            line = int.TryParse(Request.QueryString["line"], out var l) ? l : 0;
            col = int.TryParse(Request.QueryString["col"], out var c) ? c : 0;
        }

        if (string.IsNullOrEmpty(word))
        {
            RenderJson(new { error = "Word parameter is required" });
            return;
        }

        // If editorGuid is provided, use Roslyn analysis
        string fullIdentifier = word;
        string wordType = "identifier";

        if (!string.IsNullOrEmpty(editorGuid))
        {
            var code = CodeEditorCacheModel.GetCode(editorGuid);
            var lang = CodeEditorCacheModel.GetLanguage(editorGuid);
            
            if (code != null && lang?.ToLower() == "csharp")
            {
                var result = IdentifierExtractor.ExtractIdentifier(code, line, col);
                if (result.Success)
                {
                    fullIdentifier = result.FullIdentifier;
                    wordType = result.WordType;
                }
                // Roslyn analysis completed
            }
        }

        // Get localization instance for current language
        var currentLang = Config.Instance?.Data?.Language ?? Language.ZhCN;
        var loc = _localizationManager.GetLocalization(currentLang) as DefaultLocalizationBase;
        
        // Generate HTML hover content based on word type and language
        var hoverHtml = GenerateHoverHtml(fullIdentifier, word, language, line, col, wordType, loc!);
        
        // Return HTML content
        Response.ContentType = "text/html; charset=utf-8";
        RenderHtml(hoverHtml);
    }

    private string GenerateHoverHtml(string fullIdentifier, string displayWord, string language, int line, int col, string wordType, DefaultLocalizationBase loc)
    {
        string wordTypeLabel = loc.GetCodeHoverWordTypeLabel(wordType);
        string wordTypeDesc;
        
        // Try full identifier match first (e.g., System.Net.IPAddress)
        var localizationKey = $"{language.ToLower()}:{fullIdentifier}";
        wordTypeDesc = loc.GetTranslation(localizationKey);
        
        // If not found, try lowercase match
        if (string.IsNullOrEmpty(wordTypeDesc))
        {
            var lowerKey = $"{language.ToLower()}:{fullIdentifier.ToLower()}";
            wordTypeDesc = loc.GetTranslation(lowerKey);
        }
        
        // If still not found, use original logic
        if (string.IsNullOrEmpty(wordTypeDesc))
        {
            if (wordType == "keyword")
            {
                var keywordDesc = loc.GetCodeHoverKeywordDesc(language.ToLower(), displayWord);
                wordTypeDesc = string.IsNullOrEmpty(keywordDesc) 
                    ? loc.GetCodeHoverWordTypeDesc(wordType, displayWord) 
                    : keywordDesc;
            }
            else
            {
                wordTypeDesc = loc.GetCodeHoverWordTypeDesc(wordType, displayWord);
            }
        }

        var tipTypeClass = wordType switch
        {
            "variable" => "variable",
            "function" => "function",
            "class" => "class",
            "keyword" => "keyword",
            "comment" => "comment",
            "namespace" => "namespace",
            "parameter" => "variable",
            _ => "identifier"
        };

        var hoverElement = H.Div(
            H.Div(
                H.Span(wordTypeLabel).Class($"tip-type {tipTypeClass}"),
                H.Span(System.Net.WebUtility.HtmlEncode(fullIdentifier)).Class("tip-word")
            ).Class("tip-header"),
            H.Div(
                H.P(wordTypeDesc),
                H.Div(
                    H.Span($"Line: {line}"),
                    H.Span($"Col: {col}")
                ).Class("tip-meta")
            ).Class("tip-content")
        ).Class("code-hover-tip");

        return hoverElement.Build();
    }
}

// Request/Response data model
file class RegisterRequest
{
    public string? Language { get; set; }
}

file class UpdateRequest
{
    public string? EditorGuid { get; set; }
    public string? Code { get; set; }
}

file class UnregisterRequest
{
    public string? EditorGuid { get; set; }
}

file class HoverRequest
{
    public string? EditorGuid { get; set; }
    public string? Word { get; set; }
    public string? Language { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }
}
