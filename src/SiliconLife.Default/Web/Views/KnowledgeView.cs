// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public class KnowledgeView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as KnowledgeViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleKnowledge, "knowledge", vm.Localization, body, GetScripts(vm.Localization), GetStyles());
    }

    private static H RenderBody(KnowledgeViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.KnowledgePageHeader)
            ).Class("page-header"),
            H.Div(
                H.Div().Id("stats-panel").Class("stats-panel"),
                H.Create("canvas").Id("graphCanvas").Class("graph-canvas"),
                H.Div().Id("loading").Class("loading").Text("Loading...")
            ).Class("knowledge-content")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".knowledge-content")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "20px")
            .EndSelector()
            .Selector(".stats-panel")
                .Property("display", "flex")
                .Property("gap", "20px")
                .Property("padding", "15px")
                .Property("background", "var(--bg-card)")
                .Property("border-radius", "8px")
                .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".stat-item")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("align-items", "center")
            .EndSelector()
            .Selector(".stat-value")
                .Property("font-size", "24px")
                .Property("font-weight", "bold")
                .Property("color", "var(--accent)")
            .EndSelector()
            .Selector(".stat-label")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".graph-container")
                .Property("position", "relative")
                .Property("background", "var(--bg-card)")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
                .Property("min-height", "600px")
            .EndSelector()
            .Selector("#graphCanvas")
                .Property("width", "100%")
                .Property("height", "600px")
                .Property("background", "#1a1a2e")
                .Property("border-radius", "8px")
            .EndSelector()
            .Selector(".loading")
                .Property("text-align", "center")
                .Property("padding", "20px")
                .Property("color", "var(--text-secondary)")
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var script = Js.Block();

        // ===== Global variables =====
        script.Add(() => Js.Let(() => "nodes", () => Js.Array()));
        script.Add(() => Js.Let(() => "edges", () => Js.Array()));
        script.Add(() => Js.Let(() => "canvas", () => Js.Null()));
        script.Add(() => Js.Let(() => "ctx", () => Js.Null()));
        script.Add(() => Js.Let(() => "dragNode", () => Js.Null()));
        script.Add(() => Js.Let(() => "dragOffX", () => Js.Num(() => "0")));
        script.Add(() => Js.Let(() => "dragOffY", () => Js.Num(() => "0")));
        script.Add(() => Js.Const(() => "NODE_R", () => Js.Num(() => "30")));

        // ===== initCanvas =====
        var initCanvasBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "canvas"), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "graphCanvas"))))
            .Add(() => Js.Assign(() => Js.Id(() => "canvas").Prop(() => "width"), () => Js.Id(() => "canvas").Prop(() => "offsetWidth")))
            .Add(() => Js.Assign(() => Js.Id(() => "canvas").Prop(() => "height"), () => Js.Num(() => "600")))
            .Add(() => Js.Assign(() => Js.Id(() => "ctx"), () => Js.Id(() => "canvas").Call(() => "getContext", () => Js.Str(() => "2d"))))
            .Add(() => Js.Id(() => "canvas").Call(() => "addEventListener", () => Js.Str(() => "mousedown"), () => Js.Id(() => "onMouseDown")).Stmt())
            .Add(() => Js.Id(() => "canvas").Call(() => "addEventListener", () => Js.Str(() => "mousemove"), () => Js.Id(() => "onMouseMove")).Stmt())
            .Add(() => Js.Id(() => "canvas").Call(() => "addEventListener", () => Js.Str(() => "mouseup"), () => Js.Id(() => "onMouseUp")).Stmt());
        script.Add(() => Js.Func(() => "initCanvas", () => new List<string>(), () => initCanvasBody));

        // ===== layoutNodes: circular layout based on canvas size =====
        var layoutBody = Js.Block()
            .Add(() => Js.Let(() => "cnt", () => Js.Id(() => "nodes").Prop(() => "length")))
            .Add(() => Js.Let(() => "cx", () => Js.Id(() => "canvas").Prop(() => "width").Op(() => "/", () => Js.Num(() => "2"))))
            .Add(() => Js.Let(() => "cy", () => Js.Id(() => "canvas").Prop(() => "height").Op(() => "/", () => Js.Num(() => "2"))))
            .Add(() => Js.Let(() => "rad", () => Js.Id(() => "Math").Call(() => "min", () => Js.Id(() => "cx"), () => Js.Id(() => "cy")).Op(() => "*", () => Js.Num(() => "0.7"))))
            .Add(() => Js.For(
                () => Js.Let(() => "i", () => Js.Num(() => "0")),
                () => Js.Id(() => "i").Op(() => "<", () => Js.Id(() => "cnt")),
                () => Js.Assign(() => Js.Id(() => "i"), () => Js.Id(() => "i").Op(() => "+", () => Js.Num(() => "1"))),
                () => Js.Block()
                    .Add(() => Js.Let(() => "a", () =>
                        Js.Num(() => "2").Op(() => "*", () => Js.Id(() => "Math").Prop(() => "PI"))
                            .Op(() => "*", () => Js.Id(() => "i"))
                            .Op(() => "/", () => Js.Id(() => "Math").Call(() => "max", () => Js.Id(() => "cnt"), () => Js.Num(() => "1")))))
                    .Add(() => Js.Assign(() => Js.Id(() => "nodes").Index(() => Js.Id(() => "i")).Prop(() => "x"),
                        () => Js.Id(() => "cx").Op(() => "+", () => Js.Id(() => "rad").Op(() => "*", () => Js.Id(() => "Math").Call(() => "cos", () => Js.Id(() => "a"))))))
                    .Add(() => Js.Assign(() => Js.Id(() => "nodes").Index(() => Js.Id(() => "i")).Prop(() => "y"),
                        () => Js.Id(() => "cy").Op(() => "+", () => Js.Id(() => "rad").Op(() => "*", () => Js.Id(() => "Math").Call(() => "sin", () => Js.Id(() => "a"))))))
            ));
        script.Add(() => Js.Func(() => "layoutNodes", () => new List<string>(), () => layoutBody));

        // ===== drawGraph =====
        var drawGraphBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "ctx").Op(() => "===", () => Js.Null()), new List<JsSyntax> { Js.Return(() => Js.Null()) })
            }))
            .Add(() => Js.Id(() => "ctx").Call(() => "clearRect",
                () => Js.Num(() => "0"), () => Js.Num(() => "0"),
                () => Js.Id(() => "canvas").Prop(() => "width"), () => Js.Id(() => "canvas").Prop(() => "height")).Stmt())
            .Add(() => Js.Id(() => "drawEdges").Invoke().Stmt())
            .Add(() => Js.Id(() => "drawNodes").Invoke().Stmt());
        script.Add(() => Js.Func(() => "drawGraph", () => new List<string>(), () => drawGraphBody));

        // ===== drawEdges with arrows =====
        var edgeDrawStatements = new List<JsSyntax>
        {
            // Line from center to center (node circles will cover the overlap)
            Js.Id(() => "ctx").Call(() => "beginPath").Stmt(),
            Js.Id(() => "ctx").Call(() => "moveTo", () => Js.Id(() => "fn").Prop(() => "x"), () => Js.Id(() => "fn").Prop(() => "y")).Stmt(),
            Js.Id(() => "ctx").Call(() => "lineTo", () => Js.Id(() => "tn").Prop(() => "x"), () => Js.Id(() => "tn").Prop(() => "y")).Stmt(),
            Js.Id(() => "ctx").Prop(() => "strokeStyle").Assign(() => Js.Str(() => "rgba(100, 150, 255, 0.6)")),
            Js.Id(() => "ctx").Prop(() => "lineWidth").Assign(() => Js.Num(() => "2")),
            Js.Id(() => "ctx").Call(() => "stroke").Stmt(),
            // Arrowhead at target edge
            Js.Let(() => "dx", () => Js.Id(() => "tn").Prop(() => "x").Op(() => "-", () => Js.Id(() => "fn").Prop(() => "x"))),
            Js.Let(() => "dy", () => Js.Id(() => "tn").Prop(() => "y").Op(() => "-", () => Js.Id(() => "fn").Prop(() => "y"))),
            Js.Let(() => "ang", () => Js.Id(() => "Math").Call(() => "atan2", () => Js.Id(() => "dy"), () => Js.Id(() => "dx"))),
            Js.Let(() => "ex", () => Js.Id(() => "tn").Prop(() => "x").Op(() => "-", () =>
                Js.Id(() => "NODE_R").Op(() => "*", () => Js.Id(() => "Math").Call(() => "cos", () => Js.Id(() => "ang"))))),
            Js.Let(() => "ey", () => Js.Id(() => "tn").Prop(() => "y").Op(() => "-", () =>
                Js.Id(() => "NODE_R").Op(() => "*", () => Js.Id(() => "Math").Call(() => "sin", () => Js.Id(() => "ang"))))),
            Js.Id(() => "ctx").Call(() => "beginPath").Stmt(),
            Js.Id(() => "ctx").Call(() => "moveTo", () => Js.Id(() => "ex"), () => Js.Id(() => "ey")).Stmt(),
            Js.Id(() => "ctx").Call(() => "lineTo",
                () => Js.Id(() => "ex").Op(() => "-", () => Js.Num(() => "12").Op(() => "*", () =>
                    Js.Id(() => "Math").Call(() => "cos", () => Js.Id(() => "ang").Op(() => "-", () => Js.Num(() => "0.4"))))),
                () => Js.Id(() => "ey").Op(() => "-", () => Js.Num(() => "12").Op(() => "*", () =>
                    Js.Id(() => "Math").Call(() => "sin", () => Js.Id(() => "ang").Op(() => "-", () => Js.Num(() => "0.4")))))
            ).Stmt(),
            Js.Id(() => "ctx").Call(() => "lineTo",
                () => Js.Id(() => "ex").Op(() => "-", () => Js.Num(() => "12").Op(() => "*", () =>
                    Js.Id(() => "Math").Call(() => "cos", () => Js.Id(() => "ang").Op(() => "+", () => Js.Num(() => "0.4"))))),
                () => Js.Id(() => "ey").Op(() => "-", () => Js.Num(() => "12").Op(() => "*", () =>
                    Js.Id(() => "Math").Call(() => "sin", () => Js.Id(() => "ang").Op(() => "+", () => Js.Num(() => "0.4")))))
            ).Stmt(),
            Js.Id(() => "ctx").Call(() => "closePath").Stmt(),
            Js.Id(() => "ctx").Prop(() => "fillStyle").Assign(() => Js.Str(() => "rgba(100, 150, 255, 0.8)")),
            Js.Id(() => "ctx").Call(() => "fill").Stmt(),
            // Edge label at midpoint
            Js.Let(() => "mx", () => Js.Id(() => "fn").Prop(() => "x").Op(() => "+", () => Js.Id(() => "tn").Prop(() => "x")).Paren().Op(() => "/", () => Js.Num(() => "2"))),
            Js.Let(() => "my", () => Js.Id(() => "fn").Prop(() => "y").Op(() => "+", () => Js.Id(() => "tn").Prop(() => "y")).Paren().Op(() => "/", () => Js.Num(() => "2"))),
            Js.Id(() => "ctx").Prop(() => "font").Assign(() => Js.Str(() => "11px sans-serif")).Stmt(),
            Js.Id(() => "ctx").Prop(() => "textAlign").Assign(() => Js.Str(() => "center")).Stmt(),
            Js.Id(() => "ctx").Prop(() => "textBaseline").Assign(() => Js.Str(() => "bottom")).Stmt(),
            Js.Id(() => "ctx").Prop(() => "fillStyle").Assign(() => Js.Str(() => "rgba(200, 220, 255, 0.9)")).Stmt(),
            Js.Id(() => "ctx").Call(() => "fillText",
                () => Js.Id(() => "edge").Prop(() => "label").Op(() => "||", () => Js.Str(() => "")),
                () => Js.Id(() => "mx"),
                () => Js.Id(() => "my").Op(() => "-", () => Js.Num(() => "4"))
            ).Stmt()
        };

        var drawEdgesBody = Js.Block()
            .Add(() => Js.For(
                () => Js.Let(() => "i", () => Js.Num(() => "0")),
                () => Js.Id(() => "i").Op(() => "<", () => Js.Id(() => "edges").Prop(() => "length")),
                () => Js.Assign(() => Js.Id(() => "i"), () => Js.Id(() => "i").Op(() => "+", () => Js.Num(() => "1"))),
                () => Js.Block()
                    .Add(() => Js.Let(() => "edge", () => Js.Id(() => "edges").Index(() => Js.Id(() => "i"))))
                    .Add(() => Js.Let(() => "fn", () => Js.Id(() => "nodes").Call(() => "find", () => Js.Arrow(
                        () => new List<string> { "n" },
                        () => Js.Id(() => "n").Prop(() => "id").Op(() => "===", () => Js.Id(() => "edge").Prop(() => "from"))
                    ))))
                    .Add(() => Js.Let(() => "tn", () => Js.Id(() => "nodes").Call(() => "find", () => Js.Arrow(
                        () => new List<string> { "n" },
                        () => Js.Id(() => "n").Prop(() => "id").Op(() => "===", () => Js.Id(() => "edge").Prop(() => "to"))
                    ))))
                    .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "fn").Op(() => "&&", () => Js.Id(() => "tn")), edgeDrawStatements)
                    }))
            ));
        script.Add(() => Js.Func(() => "drawEdges", () => new List<string>(), () => drawEdgesBody));

        // ===== drawNodes =====
        var drawNodesBody = Js.Block()
            .Add(() => Js.For(
                () => Js.Let(() => "i", () => Js.Num(() => "0")),
                () => Js.Id(() => "i").Op(() => "<", () => Js.Id(() => "nodes").Prop(() => "length")),
                () => Js.Assign(() => Js.Id(() => "i"), () => Js.Id(() => "i").Op(() => "+", () => Js.Num(() => "1"))),
                () => Js.Block()
                    .Add(() => Js.Let(() => "nd", () => Js.Id(() => "nodes").Index(() => Js.Id(() => "i"))))
                    // Circle fill
                    .Add(() => Js.Id(() => "ctx").Call(() => "beginPath").Stmt())
                    .Add(() => Js.Id(() => "ctx").Call(() => "arc",
                        () => Js.Id(() => "nd").Prop(() => "x"), () => Js.Id(() => "nd").Prop(() => "y"),
                        () => Js.Id(() => "NODE_R"), () => Js.Num(() => "0"),
                        () => Js.Id(() => "Math").Prop(() => "PI").Op(() => "*", () => Js.Num(() => "2"))
                    ).Stmt())
                    .Add(() => Js.Id(() => "ctx").Prop(() => "fillStyle").Assign(() => Js.Str(() => "#4a6fa5")).Stmt())
                    .Add(() => Js.Id(() => "ctx").Call(() => "fill").Stmt())
                    // Circle stroke
                    .Add(() => Js.Id(() => "ctx").Prop(() => "strokeStyle").Assign(() => Js.Str(() => "#6fa3ef")).Stmt())
                    .Add(() => Js.Id(() => "ctx").Prop(() => "lineWidth").Assign(() => Js.Num(() => "2")).Stmt())
                    .Add(() => Js.Id(() => "ctx").Call(() => "stroke").Stmt())
                    // Label
                    .Add(() => Js.Id(() => "ctx").Prop(() => "font").Assign(() => Js.Str(() => "bold 12px sans-serif")).Stmt())
                    .Add(() => Js.Id(() => "ctx").Prop(() => "textAlign").Assign(() => Js.Str(() => "center")).Stmt())
                    .Add(() => Js.Id(() => "ctx").Prop(() => "textBaseline").Assign(() => Js.Str(() => "middle")).Stmt())
                    .Add(() => Js.Id(() => "ctx").Prop(() => "fillStyle").Assign(() => Js.Str(() => "#ffffff")).Stmt())
                    .Add(() => Js.Id(() => "ctx").Call(() => "fillText",
                        () => Js.Id(() => "nd").Prop(() => "label"),
                        () => Js.Id(() => "nd").Prop(() => "x"),
                        () => Js.Id(() => "nd").Prop(() => "y")
                    ).Stmt())
            ));
        script.Add(() => Js.Func(() => "drawNodes", () => new List<string>(), () => drawNodesBody));

        // ===== findNodeAt: hit test =====
        var findBody = Js.Block()
            .Add(() => Js.For(
                () => Js.Let(() => "i", () => Js.Num(() => "0")),
                () => Js.Id(() => "i").Op(() => "<", () => Js.Id(() => "nodes").Prop(() => "length")),
                () => Js.Assign(() => Js.Id(() => "i"), () => Js.Id(() => "i").Op(() => "+", () => Js.Num(() => "1"))),
                () => Js.Block()
                    .Add(() => Js.Let(() => "dx", () => Js.Id(() => "mx").Op(() => "-", () => Js.Id(() => "nodes").Index(() => Js.Id(() => "i")).Prop(() => "x"))))
                    .Add(() => Js.Let(() => "dy", () => Js.Id(() => "my").Op(() => "-", () => Js.Id(() => "nodes").Index(() => Js.Id(() => "i")).Prop(() => "y"))))
                    .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "dx").Op(() => "*", () => Js.Id(() => "dx"))
                            .Op(() => "+", () => Js.Id(() => "dy").Op(() => "*", () => Js.Id(() => "dy")))
                            .Op(() => "<", () => Js.Id(() => "NODE_R").Op(() => "*", () => Js.Id(() => "NODE_R"))),
                        new List<JsSyntax> { Js.Return(() => Js.Id(() => "nodes").Index(() => Js.Id(() => "i"))) })
                    }))
            ))
            .Add(() => Js.Return(() => Js.Null()));
        script.Add(() => Js.Func(() => "findNodeAt", () => new List<string> { "mx", "my" }, () => findBody));

        // ===== onMouseDown =====
        var mouseDownBody = Js.Block()
            .Add(() => Js.Let(() => "nd", () => Js.Id(() => "findNodeAt").Invoke(
                () => Js.Id(() => "e").Prop(() => "offsetX"), () => Js.Id(() => "e").Prop(() => "offsetY"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "nd"), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "dragNode"), () => Js.Id(() => "nd")),
                    Js.Assign(() => Js.Id(() => "dragOffX"), () => Js.Id(() => "e").Prop(() => "offsetX").Op(() => "-", () => Js.Id(() => "nd").Prop(() => "x"))),
                    Js.Assign(() => Js.Id(() => "dragOffY"), () => Js.Id(() => "e").Prop(() => "offsetY").Op(() => "-", () => Js.Id(() => "nd").Prop(() => "y"))),
                    Js.Id(() => "canvas").Prop(() => "style").Prop(() => "cursor").Assign(() => Js.Str(() => "grabbing"))
                })
            }));
        script.Add(() => Js.Func(() => "onMouseDown", () => new List<string> { "e" }, () => mouseDownBody));

        // ===== onMouseMove =====
        var mouseMoveBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "dragNode"), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "dragNode").Prop(() => "x"), () => Js.Id(() => "e").Prop(() => "offsetX").Op(() => "-", () => Js.Id(() => "dragOffX"))),
                    Js.Assign(() => Js.Id(() => "dragNode").Prop(() => "y"), () => Js.Id(() => "e").Prop(() => "offsetY").Op(() => "-", () => Js.Id(() => "dragOffY"))),
                    Js.Id(() => "drawGraph").Invoke().Stmt(),
                    Js.Return(() => Js.Null())
                }),
                (null, new List<JsSyntax>
                {
                    Js.Let(() => "n", () => Js.Id(() => "findNodeAt").Invoke(
                        () => Js.Id(() => "e").Prop(() => "offsetX"), () => Js.Id(() => "e").Prop(() => "offsetY"))),
                    Js.Assign(() => Js.Id(() => "canvas").Prop(() => "style").Prop(() => "cursor"),
                        () => Js.Ternary(() => Js.Id(() => "n"), () => Js.Str(() => "grab"), () => Js.Str(() => "default")))
                })
            }));
        script.Add(() => Js.Func(() => "onMouseMove", () => new List<string> { "e" }, () => mouseMoveBody));

        // ===== onMouseUp =====
        var mouseUpBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "dragNode"), () => Js.Null()))
            .Add(() => Js.Assign(() => Js.Id(() => "canvas").Prop(() => "style").Prop(() => "cursor"), () => Js.Str(() => "default")));
        script.Add(() => Js.Func(() => "onMouseUp", () => new List<string> { "e" }, () => mouseUpBody));

        // ===== updateStats =====
        var updateStatsBody = Js.Block()
            .Add(() => Js.Let(() => "sp", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stats-panel"))))
            .Add(() => Js.Assign(() => Js.Id(() => "sp").Prop(() => "innerHTML"), () =>
                Js.Str(() => "<div class='stat-item'><div class='stat-value'>")
                    .Op(() => "+", () => Js.Id(() => "nodes").Prop(() => "length"))
                    .Op(() => "+", () => Js.Str(() => "</div><div class='stat-label'>Nodes</div></div><div class='stat-item'><div class='stat-value'>"))
                    .Op(() => "+", () => Js.Id(() => "edges").Prop(() => "length"))
                    .Op(() => "+", () => Js.Str(() => "</div><div class='stat-label'>Edges</div></div>"))));
        script.Add(() => Js.Func(() => "updateStats", () => new List<string>(), () => updateStatsBody));

        // ===== loadGraph =====
        var fetchExpr = Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/knowledge/graph"));
        var then1 = Js.Call(() => fetchExpr, () => "then", () => Js.Arrow(
            () => new List<string> { "r" },
            () => Js.Id(() => "r").Call(() => "json")
        ));

        var successBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "nodes"), () => Js.Id(() => "data").Prop(() => "nodes").Op(() => "||", () => Js.Array())))
            .Add(() => Js.Assign(() => Js.Id(() => "edges"), () => Js.Id(() => "data").Prop(() => "edges").Op(() => "||", () => Js.Array())))
            .Add(() => Js.Id(() => "initCanvas").Invoke().Stmt())
            .Add(() => Js.Id(() => "layoutNodes").Invoke().Stmt())
            .Add(() => Js.Id(() => "updateStats").Invoke().Stmt())
            .Add(() => Js.Id(() => "drawGraph").Invoke().Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "loading")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")));

        var then2 = Js.Call(() => then1, () => "then", () => Js.Arrow(
            () => new List<string> { "data" },
            () => successBody
        ));

        var errorBody = Js.Block()
            .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Failed to load graph"), () => Js.Id(() => "err")).Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "loading")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")));

        var catchExpr = Js.Call(() => then2, () => "catch", () => Js.Arrow(
            () => new List<string> { "err" },
            () => errorBody
        ));

        var loadGraphBody = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "loading")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")))
            .Add(() => catchExpr);
        script.Add(() => Js.Func(() => "loadGraph", () => new List<string>(), () => loadGraphBody));

        // ===== window.onload =====
        script.Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadGraph").Invoke())));

        return script;
    }
}
